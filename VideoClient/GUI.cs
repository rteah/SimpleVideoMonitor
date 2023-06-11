using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisioForge.Controls.UI.Dialogs.OutputFormats;
using VisioForge.MediaFramework.deviceio;
using VisioForge.MediaFramework.DSP;
using VisioForge.MediaFramework.RTSPSource;
using VisioForge.Shared.MediaFoundation;
using VisioForge.Shared.MediaFoundation.OPM;
using VisioForge.Shared.NAudio.Wave;
using VisioForge.Shared.SharpRaven.Data;
using VisioForge.Types.OutputFormat;
using VisioForge.Types.Sources;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace VideoClient
{
    public partial class GUI : Form
    {
        private TreeViewContainer ViewContainer;
        private MediaContainer    PlayerArea;
        private enum AutoPlayButtonState{
            Inactivated = 0,
            Activated   = 1,
        };
        private enum PlayButtonState{
            Paused  = 0,
            Playing = 1
        };
        public GUI()
        {
            InitializeComponent();

            /* TreeView Area */
            ViewContainer = new TreeViewContainer(this.Size.Width/8, this.Size.Height, this);
            ViewContainer.Location = new Point(0, 0);
            Controls.Add(this.ViewContainer);
            ViewContainer.Size = new Size(this.Size.Width/8, this.Size.Height);
            ViewContainer.treeView.AfterSelect += new TreeViewEventHandler(TreeView_AfterSelected);

            /* Media Player Area */
            PlayerArea = new MediaContainer(this.Size.Width/8*7+10, this.Size.Height); 
            Controls.Add(this.PlayerArea);
            
            /* four buttons */
            OnePlayerButton.Size  = new Size(25, 25);
            FourPlayerButton.Size = new Size(25, 25);
            NinePlayerButton.Size = new Size(25, 25);
            
            OnePlayerButton.Location  = new Point(this.Size.Width - 105, 0);
            FourPlayerButton.Location = new Point(this.Size.Width - 75, 0);
            NinePlayerButton.Location = new Point(this.Size.Width - 45, 0);

            OnePlayerButton.Click   += new EventHandler(PlayerNumButton_Click);
            FourPlayerButton.Click  += new EventHandler(PlayerNumButton_Click);
            NinePlayerButton.Click  += new EventHandler(PlayerNumButton_Click);

            /* GUI */
            this.StartPosition = FormStartPosition.CenterScreen;
            this.SizeChanged += new EventHandler(GUI_SizeChanged);
            
            GUI_SizeChanged(this, EventArgs.Empty);
        }

        /* Size Change of Mainform */
        private int ResizeTreeView(int x, int y, int width, int height)
        { 
            // treeview.width + mediacontainer.width = gui.width
            ViewContainer.SetLogicalSize(width, height);
            int RealWidth = ViewContainer.Paint();
            ViewContainer.SetBounds(x, y, RealWidth + 20, height);
            return RealWidth;
        }

        private int ResizeMediaView(int x, int y, int width, int height)
        {
            PlayerArea.SetLogicalSize(width, height);
            PlayerArea.Render();
            PlayerArea.SetBounds(x, y, width, height);
            return width;
        }

        public void GUI_SizeChanged(object sender, EventArgs e)
        {
            int width  = this.Size.Width;
            int height = this.Size.Height;  
            OnePlayerButton.Location    = new Point(width - 105, 0);
            FourPlayerButton.Location   = new Point(width - 75, 0);
            NinePlayerButton.Location   = new Point(width - 45, 0);

            AutoPlayButton.Location     = new Point(width / 2 - 45, 0);
            PlayButton.Location         = new Point(width / 2 - 5 , 0);
            ForceStopButton.Location    = new Point(width / 2 + 35, 0);

            int treeViewWidth       = ResizeTreeView(0, 0, width / 8, height);
            int MediaContainerWidth = ResizeMediaView(treeViewWidth, 30 , width - treeViewWidth, height - 30);
#if DEBUG
            Ultility.Debug("real size of MediaArea: " + MediaContainerWidth);
#endif  
        }

        /* GUI and players */
        public  void PlayerNumButton_Click(object sender, EventArgs e)
        {
            int prevNum = PlayerArea.kNumContainers;
            if (sender == OnePlayerButton)       // ->  one
            {
                PlayerArea.SetNumberContainer(1);
            }
            else if (sender == FourPlayerButton) // -> four
            {
                PlayerArea.SetNumberContainer(4);
            }
            else if (sender == NinePlayerButton) // -> nine
            {
                PlayerArea.SetNumberContainer(9);
            }

           if (prevNum != PlayerArea.kNumContainers) { 
                PlayerArea.RetrievePlayers();
                PlayerArea.Render();
            }
        }
        public  void TreeView_AfterSelected(object sender, TreeViewEventArgs e)
        {
#if DEBUG
            Ultility.Debug("Triggered");
#endif
            System.Windows.Forms.TreeNode selected = ((System.Windows.Forms.TreeView)sender).SelectedNode;
            if (selected != null)
            {
                PlayerArea.Play(selected.Text);
            }
        }
        private void AutoPlayButton_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Button button = sender as System.Windows.Forms.Button;
            var state = button.Equals(AutoPlayButton) ? AutoPlayButton.Tag : -1;
            switch (state)
            {
                case AutoPlayButtonState.Inactivated:
                    AutoPlayButton.Tag = AutoPlayButtonState.Activated;
                    AutoPlayButton.BackgroundImage = Image.FromFile("D:\\Projects\\C#\\VideoClient\\VideoClient\\resources\\autoplay_activated.png");
                    break;
                case AutoPlayButtonState.Activated:
                    // same as default 
                default:
                    AutoPlayButton.Tag = AutoPlayButtonState.Inactivated;
                    AutoPlayButton.BackgroundImage = Image.FromFile("D:\\Projects\\C#\\VideoClient\\VideoClient\\resources\\autoplay_inactivated.png");
                    break;
            }
        }
        private void PlayButton_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Button button = sender as System.Windows.Forms.Button;
            var state = button.Equals(PlayButton) ? PlayButton.Tag : -1;
            switch (state)
            {
                case PlayButtonState.Playing:
                    PlayButton.Tag = PlayButtonState.Paused;
                    PlayButton.BackgroundImage = Image.FromFile("D:\\Projects\\C#\\VideoClient\\VideoClient\\resources\\play.png");
                    PlayerArea.Pause();
                    break;
                case PlayButtonState.Paused:
                    // same as default 
                default:
                    PlayButton.Tag = PlayButtonState.Playing;
                    PlayButton.BackgroundImage = Image.FromFile("D:\\Projects\\C#\\VideoClient\\VideoClient\\resources\\pause.png");
                    PlayerArea.Resume();
                    break;
            }
        }
        public  void ForceStopButton_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Button button = PlayButton;
            
            PlayerArea.StopAll();
            PlayButton_Click(ForceStopButton, null);
            AutoPlayButton_Click(ForceStopButton, null);
        }
        private void NumChannelButtons_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Button selected = sender as System.Windows.Forms.Button;
            OnePlayerButton.BackgroundImage  = Image.FromFile("D:\\Projects\\C#\\VideoClient\\VideoClient\\resources\\1.png");
            FourPlayerButton.BackgroundImage = Image.FromFile("D:\\Projects\\C#\\VideoClient\\VideoClient\\resources\\4.png");
            NinePlayerButton.BackgroundImage = Image.FromFile("D:\\Projects\\C#\\VideoClient\\VideoClient\\resources\\9.png");
            if (selected == OnePlayerButton) { selected.BackgroundImage       = Image.FromFile("D:\\Projects\\C#\\VideoClient\\VideoClient\\resources\\1_black.png"); }
            else if (selected == FourPlayerButton) { selected.BackgroundImage = Image.FromFile("D:\\Projects\\C#\\VideoClient\\VideoClient\\resources\\4_black.png"); }
            else if (selected == NinePlayerButton) { selected.BackgroundImage = Image.FromFile("D:\\Projects\\C#\\VideoClient\\VideoClient\\resources\\9_black.png"); }
        }
    }
}

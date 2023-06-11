//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Diagnostics;
//using System.Drawing;
//using System.IO;
//using System.Linq;
//using System.Runtime.CompilerServices;
//using System.Security;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using VisioForge.MediaFramework.deviceio;
//using VisioForge.MediaFramework.DSP;
//using VisioForge.MediaFramework.RTSPSource;
//using VisioForge.Shared.MediaFoundation.OPM;
//using VisioForge.Shared.NAudio.Wave;
//using VisioForge.Shared.SharpRaven.Data;
//using VisioForge.Types.OutputFormat;
//using VisioForge.Types.Sources;

//namespace VideoClient
//{
//    public partial class GUI : Form
//    {
//        private TreeViewContainer ViewContainer;

//        // media players 
//        private ArrayList OneMedia;
//        private ArrayList FourMedia;
//        private ArrayList NineMedia;
//        private ArrayList currentMedia;
//        private Queue<MediaPlayer> availableMedia;

//        public GUI()
//        {
//            InitializeComponent();
//            this.StartPosition = FormStartPosition.CenterScreen;
//            availableMedia = new Queue<MediaPlayer>();

//            ViewContainer = new TreeViewContainer(this.Size.Width / 8, this.Size.Height);
//            ViewContainer.Location = new Point(0, 0);
//            ViewContainer.Size = new Size(this.Size.Width / 8, this.Size.Height);
//            Controls.Add(this.ViewContainer);

//            //RTSPButton.Image = new Bitmap(RTSPButton.Image, new Size(imageSize, imageSize));

//            //PlayerNumButton = new Button
//            //{
//            //    Size = new Size(30, 20),
//            //    Location = new Point(this.Size.Width - 55, 0),
//            //    Text = "1"
//            //};

//            OnePlayerButton.Size = new Size(25, 25);
//            FourPlayerButton.Size = new Size(25, 25);
//            NinePlayerButton.Size = new Size(25, 25);

//            OnePlayerButton.Location = new Point(this.Size.Width - 105, 0);
//            FourPlayerButton.Location = new Point(this.Size.Width - 75, 0);
//            NinePlayerButton.Location = new Point(this.Size.Width - 45, 0);

//            this.SizeChanged += new EventHandler(GUI_SizeChanged);
//            //FileButton.Click += new EventHandler(FileButton_Click);
//            //RTSPButton.Click += new EventHandler(RTSPButton_Click);
//            //PlayerNumButton.Click+= new EventHandler(PlayerNumButton_Click);
//            OnePlayerButton.Click += new EventHandler(PlayerNumButton_Click);
//            FourPlayerButton.Click += new EventHandler(PlayerNumButton_Click);
//            NinePlayerButton.Click += new EventHandler(PlayerNumButton_Click);
//            //treeView1.AfterSelect += new TreeViewEventHandler(TreeView_AfterSelect);
//            //Controls.Add(FileButton);
//            //Controls.Add(RTSPButton);
//            //Controls.Add(OnePlayerButton);
//            //Controls.Add(FourPlayerButton);
//            //Controls.Add(NinePlayerButton);

//            InitalizeMedia();
//        }

//        /* Size Change of Mainform */

//        private void ChangeLocation(ArrayList media, int num)
//        {
//            int startX = treeView1.Location.X + treeView1.Width;
//            int startY = treeView1.Location.Y;
//            int width = (this.ClientSize.Width - startX) / num;
//            int height = (this.ClientSize.Height - startY) / num;

//            for (int i = 0; i < media.Count; i++)
//            {
//                int row = i % num;
//                int col = i / num;
//                MediaPlayer t = media[i] as MediaPlayer;
//                t.setLocation(startX + width * row, startY + height * col, width, height);
//            }
//        }
//        private void GUI_SizeChanged(object sender, EventArgs e)
//        {
//            OnePlayerButton.Location = new Point(this.Size.Width - 105, 0);
//            FourPlayerButton.Location = new Point(this.Size.Width - 75, 0);
//            NinePlayerButton.Location = new Point(this.Size.Width - 45, 0);
//            treeView1.Size = new Size(150, this.Size.Height - 32);

//            // change the size of the media players
//            ChangeLocation(OneMedia, 1);
//            ChangeLocation(FourMedia, 2);
//            ChangeLocation(NineMedia, 3);
//        }

//        /* GUI and players */
//        public void PlayerNumButton_Click(object sender, EventArgs e)
//        {
//            ArrayList prevMedia = currentMedia;
//            if (sender == OnePlayerButton) // ->  one
//            {
//                currentMedia = OneMedia;
//            }
//            else if (sender == FourPlayerButton) // -> four
//            {
//                currentMedia = FourMedia;
//            }
//            else if (sender == NinePlayerButton) // -> nine
//            {
//                currentMedia = NineMedia;
//            }
//            hide(prevMedia);
//            repaint();
//        }

//        public void InitalizeMedia()
//        {
//            /* area */
//            int startX = treeView1.Location.X + treeView1.Width;
//            int startY = treeView1.Location.Y;
//            int width = this.ClientSize.Width - startX;
//            int height = this.ClientSize.Height - startY;

//            /* one player */
//            OneMedia = new ArrayList();
//            MediaPlayer temp = new MediaPlayer(this);
//            temp.setLocation(startX, startY, width, height);
//            temp.setVisible(true);
//            OneMedia.Add(temp);

//            /* four player */
//            FourMedia = new ArrayList();
//            int offset = 2;
//            int wid = width / offset;
//            int hei = height / offset;
//            for (int col = 0; col < offset; col++)
//            {
//                for (int row = 0; row < offset; row++)
//                {
//                    MediaPlayer t = new MediaPlayer(this);
//                    t.setLocation(startX + wid * row, startY + hei * col, wid, hei);
//                    t.setVisible(false);
//                    FourMedia.Add(t);
//                }
//            }

//            /* nine player */
//            NineMedia = new ArrayList();
//            offset = 3;
//            wid = width / offset;
//            hei = height / offset;
//            for (int col = 0; col < offset; col++)
//            {
//                for (int row = 0; row < offset; row++)
//                {
//                    MediaPlayer t = new MediaPlayer(this);
//                    t.setLocation(startX + wid * row, startY + hei * col, wid, hei);
//                    t.setVisible(false);
//                    NineMedia.Add(t);
//                }
//            }

//            currentMedia = OneMedia;
//            repaint();
//        }

//        public void hide(ArrayList toHidden)
//        {
//            Parallel.ForEach(toHidden.Cast<MediaPlayer>(), async player =>
//            {
//                await Task.Run(() =>
//                {
//                    player.hide();
//                });

//                await Task.Run(() =>
//                {
//                    availableMedia.Enqueue(player);
//                });

//                await Task.Run(() =>
//                {
//                    player.stop();
//                });
//            });
//        }

//        public void repaint()
//        {
//            // currentMedia :: arrayList
//            availableMedia = new Queue<MediaPlayer>();
//            foreach (MediaPlayer player in currentMedia)
//            {
//                player.setVisible(true);
//                availableMedia.Enqueue(player);
//            }
//            treeView1.Visible = false;
//        }

//        /* RTSP input form */

//        /* play and stop */
//        //private void TreeView_AfterSelect(Object sender, TreeViewEventArgs e)
//        //{
//        //    TreeViewHitTestInfo info = treeView1.HitTest(treeView1.PointToClient(Cursor.Position));
//        //    if (info != null)
//        //    {
//        //        string text = info.Node.Text;
//        //        playMedia(text);
//        //    }
//        //}

//        private void playMedia(string url)
//        {
//            //Ultility.Debug("Begin play " + url);

//            if (availableMedia.Count == 0)
//            {
//                Ultility.Debug("Current Media is Busy");
//            }
//            else
//            {
//                MediaPlayer player = availableMedia.First();
//                availableMedia.Dequeue();
//                player.Play(url);
//            }
//        }

//        private void stopMedia(MediaPlayer player)
//        {
//            player.stop();
//            availableMedia.Enqueue(player);
//            player.hide();
//        }
//    }
//}

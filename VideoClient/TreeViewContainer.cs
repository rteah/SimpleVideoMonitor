using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisioForge.Controls.UI.Dialogs.OutputFormats;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace VideoClient
{
    internal class TreeViewContainer : Panel
    {
        private bool IsExpanded = true;
        private int width;
        private int height;

        public System.Windows.Forms.TreeView treeView;
        public System.Windows.Forms.Button ControlButton;
        private System.Windows.Forms.Button OpenFileButton;
        private System.Windows.Forms.Button OpenURLButton;
        private System.Windows.Forms.Button ClearButton;
        private Label Text_Playlist;

        private readonly GUI OutEventHandler;
        public TreeViewContainer(int width, int height, GUI GUI)
        {
            OutEventHandler = GUI;
            treeView = new System.Windows.Forms.TreeView()
            {
                Font = new System.Drawing.Font("Calibri", 10F),
                Name = "treeview",
                Location = new Point(0, 20)
            };
            treeView.AfterSelect += new TreeViewEventHandler(TreeView_AfterSelect);

            ControlButton = new System.Windows.Forms.Button()
            {
                Size = new Size(15, 40),
            };
            ControlButton.Image = Image.FromFile("D:\\Projects\\C#\\VideoClient\\VideoClient\\resources\\left.png");
            ControlButton.ImageAlign = ContentAlignment.MiddleCenter;
            ControlButton.FlatStyle = FlatStyle.Flat;
            ControlButton.FlatAppearance.BorderSize = 0;
            ControlButton.FlatAppearance.MouseOverBackColor = ControlButton.BackColor;
            ControlButton.Click += new EventHandler(ControlButton_Click);
            
   

            OpenFileButton = new System.Windows.Forms.Button()
            {
                Size = new Size(20, 20)
            };
            OpenFileButton.Image = Image.FromFile("D:\\Projects\\C#\\VideoClient\\VideoClient\\resources\\add.png");
            OpenFileButton.ImageAlign = ContentAlignment.MiddleCenter;
            OpenFileButton.FlatStyle = FlatStyle.Flat;
            OpenFileButton.FlatAppearance.BorderSize = 0;
            OpenFileButton.Click += new EventHandler(OpenFileButton_Click);

            OpenURLButton = new System.Windows.Forms.Button()
            {
                Size = new Size(20, 20)
            };
            OpenURLButton.BackgroundImage = Image.FromFile("D:\\Projects\\C#\\VideoClient\\VideoClient\\resources\\link.png");
            OpenURLButton.BackgroundImageLayout = ImageLayout.Zoom;
            OpenURLButton.FlatStyle = FlatStyle.Flat;
            OpenURLButton.FlatAppearance.BorderSize = 0;
            OpenURLButton.Click += new EventHandler(OpenURLButton_Click);
            
            ClearButton = new System.Windows.Forms.Button()
            {
                Size = new Size(20, 20)
            };
            ClearButton.BackgroundImage = Image.FromFile("D:\\Projects\\C#\\VideoClient\\VideoClient\\resources\\clean.png");
            ClearButton.BackgroundImageLayout = ImageLayout.Zoom;
            ClearButton.FlatStyle = FlatStyle.Flat;
            ClearButton.FlatAppearance.BorderSize = 0;
            ClearButton.Click += new EventHandler(ClearButton_Click);

            //Text_Playlist = new Label()
            //{
            //    Text = "  Playlist",
            //    Font = new System.Drawing.Font("Calibri", 10F),
            //    Location = new Point(0,0)
            //};

            SetLogicalSize(width, height);
            Controls.Add(ControlButton); 
            Controls.Add(treeView);
            Controls.Add(OpenFileButton);
            Controls.Add(OpenURLButton);
            Controls.Add(ClearButton);
            //Controls.Add(Text_Playlist);
        }

        public void SetLogicalSize(int width, int height)
        {
            // reset the width and height
            // and repaint with current policy

            this.width  = width;
            this.height = height;

            Paint();
        }

        public int Paint()
        {
            //  paint all components according to 
            //          IsExpanded
            //          Height and Width
            
            int RealWidth = IsExpanded ? width : 0;
            
            treeView.Size = new Size(RealWidth - 20, this.height);
            OpenFileButton.Location = new Point(RealWidth - 65, 0);
            OpenURLButton.Location  = new Point(RealWidth - 40, 0);

            if (IsExpanded)
            {
                treeView.Visible        = true;
                OpenFileButton.Visible  = true;
                OpenURLButton.Visible   = true;
                ClearButton.Visible     = true; 
                ControlButton.Location = new Point(this.width - 20, this.height / 2 - 20);
            }
            else
            {
                treeView.Visible        = false; 
                OpenFileButton.Visible  = false;
                OpenURLButton.Visible   = false;
                ClearButton.Visible     = false;
                ControlButton.Location = new Point(0, this.height / 2 - 20);
            }
            return RealWidth;
        }

        public void ControlButton_Click(object sender, EventArgs e)
        {
            // only change the policy
            // and repaint with current location/Size

            IsExpanded = !IsExpanded;
            OutEventHandler.GUI_SizeChanged(this, e);
        }

        public void OpenFileButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = Ultility.ofd;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    AddNode(openFileDialog.FileName);
                }
                catch (SecurityException ex)
                {
#if DEBUG
                    // Ultility.Debug("Exception in Open File");
#endif
                }
            }
        }

        public void OpenURLButton_Click(object sender, EventArgs e)
        {
            string value = "";
            if (Ultility.InputBox("Input Box", "Input RTSP URL:\nExample: RTSP://127.0.0.1:544/1.mp4", ref value) == DialogResult.OK)
            {
                // add the value to the buffer
                if (value != "")
                {
                    if (Ultility.IsRTSP(value))
                    {
                        AddNode(value);
                    }
                    else
                    {
                        Ultility.Debug("Please input a valid RTSP link.");
                    }
                }
            }
        }
        
        public void ClearButton_Click(object sender, EventArgs e)
        {
            treeView.Nodes.Clear();
            OutEventHandler.ForceStopButton_Click(sender, null);
        }

        private void TreeView_AfterSelect(Object sender, TreeViewEventArgs e)
        {
            TreeViewHitTestInfo info = treeView.HitTest(treeView.PointToClient(Cursor.Position));
            if (info != null)
            {
                string text = info.Node.Text;
#if DEBUG
                Ultility.Debug(text);
#endif
            }
        }
        private void AddNode(string url)
        {
            // Add to the Treeview
            TreeNode tn = new TreeNode();
            tn.Text = url;
            tn.Tag  = url;
            treeView.Nodes.Add(tn);
        }
    }
}

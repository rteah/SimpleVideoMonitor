namespace VideoClient
{
    partial class GUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.PlayButton = new System.Windows.Forms.Button();
            this.NinePlayerButton = new System.Windows.Forms.Button();
            this.FourPlayerButton = new System.Windows.Forms.Button();
            this.OnePlayerButton = new System.Windows.Forms.Button();
            this.ForceStopButton = new System.Windows.Forms.Button();
            this.AutoPlayButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // PlayButton
            // 
            this.PlayButton.BackColor = System.Drawing.SystemColors.Control;
            this.PlayButton.BackgroundImage = global::VideoClient.Properties.Resources.play;
            this.PlayButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.PlayButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.PlayButton.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
            this.PlayButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            this.PlayButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PlayButton.ForeColor = System.Drawing.SystemColors.Control;
            this.PlayButton.Location = new System.Drawing.Point(638, 0);
            this.PlayButton.Name = "PlayButton";
            this.PlayButton.Size = new System.Drawing.Size(40, 40);
            this.PlayButton.TabIndex = 5;
            this.PlayButton.Tag = PlayButtonState.Playing;
            this.PlayButton.UseVisualStyleBackColor = false;
            this.PlayButton.Click += new System.EventHandler(this.PlayButton_Click);
            // 
            // NinePlayerButton
            // 
            this.NinePlayerButton.BackColor = System.Drawing.SystemColors.Control;
            this.NinePlayerButton.BackgroundImage = global::VideoClient.Properties.Resources._9;
            this.NinePlayerButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.NinePlayerButton.FlatAppearance.BorderSize = 0;
            this.NinePlayerButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.NinePlayerButton.Location = new System.Drawing.Point(1057, 0);
            this.NinePlayerButton.Name = "NinePlayerButton";
            this.NinePlayerButton.Size = new System.Drawing.Size(38, 43);
            this.NinePlayerButton.TabIndex = 4;
            this.NinePlayerButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.NinePlayerButton.UseVisualStyleBackColor = false;
            this.NinePlayerButton.Click += new System.EventHandler(this.NumChannelButtons_Click);
            // 
            // FourPlayerButton
            // 
            this.FourPlayerButton.BackColor = System.Drawing.SystemColors.Control;
            this.FourPlayerButton.BackgroundImage = global::VideoClient.Properties.Resources._4;
            this.FourPlayerButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.FourPlayerButton.FlatAppearance.BorderSize = 0;
            this.FourPlayerButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FourPlayerButton.Location = new System.Drawing.Point(1013, 0);
            this.FourPlayerButton.Name = "FourPlayerButton";
            this.FourPlayerButton.Size = new System.Drawing.Size(38, 43);
            this.FourPlayerButton.TabIndex = 3;
            this.FourPlayerButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.FourPlayerButton.UseVisualStyleBackColor = false;
            this.FourPlayerButton.Click += new System.EventHandler(this.NumChannelButtons_Click);
            // 
            // OnePlayerButton
            // 
            this.OnePlayerButton.BackColor = System.Drawing.SystemColors.Control;
            this.OnePlayerButton.BackgroundImage = global::VideoClient.Properties.Resources._1;
            this.OnePlayerButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.OnePlayerButton.FlatAppearance.BorderSize = 0;
            this.OnePlayerButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OnePlayerButton.Location = new System.Drawing.Point(969, 0);
            this.OnePlayerButton.Name = "OnePlayerButton";
            this.OnePlayerButton.Size = new System.Drawing.Size(40, 40);
            this.OnePlayerButton.TabIndex = 2;
            this.OnePlayerButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.OnePlayerButton.UseVisualStyleBackColor = false;
            this.OnePlayerButton.Click += new System.EventHandler(this.NumChannelButtons_Click);
            // 
            // ForceStopButton
            // 
            this.ForceStopButton.BackColor = System.Drawing.SystemColors.Control;
            this.ForceStopButton.BackgroundImage = global::VideoClient.Properties.Resources.force_stop;
            this.ForceStopButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ForceStopButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.ForceStopButton.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
            this.ForceStopButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            this.ForceStopButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ForceStopButton.ForeColor = System.Drawing.SystemColors.Control;
            this.ForceStopButton.Location = new System.Drawing.Point(684, 0);
            this.ForceStopButton.Name = "ForceStopButton";
            this.ForceStopButton.Size = new System.Drawing.Size(40, 40);
            this.ForceStopButton.TabIndex = 6;
            this.ForceStopButton.Tag = "";
            this.ForceStopButton.UseVisualStyleBackColor = false;
            this.ForceStopButton.Click += new System.EventHandler(this.ForceStopButton_Click);
            // 
            // AutoPlayButton
            // 
            this.AutoPlayButton.BackColor = System.Drawing.SystemColors.Control;
            this.AutoPlayButton.BackgroundImage = global::VideoClient.Properties.Resources.autoplay_inactivated;
            this.AutoPlayButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.AutoPlayButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.AutoPlayButton.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
            this.AutoPlayButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            this.AutoPlayButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AutoPlayButton.ForeColor = System.Drawing.SystemColors.Control;
            this.AutoPlayButton.Location = new System.Drawing.Point(592, 0);
            this.AutoPlayButton.Name = "AutoPlayButton";
            this.AutoPlayButton.Size = new System.Drawing.Size(40, 40);
            this.AutoPlayButton.TabIndex = 7;
            this.AutoPlayButton.Tag = AutoPlayButtonState.Inactivated;
            this.AutoPlayButton.UseVisualStyleBackColor = false;
            this.AutoPlayButton.Click += new System.EventHandler(this.AutoPlayButton_Click);
            // 
            // GUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1500, 800);
            this.Controls.Add(this.AutoPlayButton);
            this.Controls.Add(this.ForceStopButton);
            this.Controls.Add(this.PlayButton);
            this.Controls.Add(this.NinePlayerButton);
            this.Controls.Add(this.FourPlayerButton);
            this.Controls.Add(this.OnePlayerButton);
            this.Name = "GUI";
            this.Text = "Video Client";
            this.ResumeLayout(false);

        }

        #endregion
        //private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Button OnePlayerButton;
        private System.Windows.Forms.Button FourPlayerButton;
        private System.Windows.Forms.Button NinePlayerButton;
        private System.Windows.Forms.Button PlayButton;
        private System.Windows.Forms.Button ForceStopButton;
        private System.Windows.Forms.Button AutoPlayButton;
    }
}


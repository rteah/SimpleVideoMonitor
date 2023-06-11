using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisioForge.Controls.UI.Dialogs.OutputFormats;
using VisioForge.Controls.UI.WinForms;
using VisioForge.Controls.UI.WPF;
using VisioForge.MediaFramework.RTSPSource;
using VisioForge.Shared.DirectShowLib;
using VisioForge.Shared.MediaFoundation.Misc;
using VisioForge.Shared.MediaFoundation.OPM;
using VisioForge.Shared.SharpCompress.Common;
using VisioForge.Shared.WindowsMediaLib;
using VisioForge.Types;
using VisioForge.Types.OutputFormat;
using VisioForge.Types.Sources;

namespace VideoClient
{
    internal class MediaPlayer : Panel
    {
        // which is a painter to paint one of the media player to target
        private MediaContainer ui;// using On_DoubleClicked
        private VisioForge.Controls.UI.WinForms.VideoCapture _player;
        private VisioForge.Controls.UI.WinForms.MediaPlayer _localplayer;
        //private Button _FullScreenButton;

        private int x, y, width, height;
        private string PlayURL = "";

        public bool IsVisible;
        public bool IsRTSP;
        public bool IsPlaying;
        public bool IsHighlighted;
        public bool IsFullScreen;

        private Thread IdleThread;
        public MediaPlayer(MediaContainer ui)
        {
            this.ui = ui;
            VisioForge.Types.VideoRendererSettingsWinForms videoRendererSettingsWinForms 
                         = new VisioForge.Types.VideoRendererSettingsWinForms();
            _player      = new VisioForge.Controls.UI.WinForms.VideoCapture();
            _localplayer = new VisioForge.Controls.UI.WinForms.MediaPlayer();

            _player.DoubleClick += new EventHandler(On_DoubleClicked);
            _player.Click       += new EventHandler(On_Clicked);
            _localplayer.DoubleClick += new EventHandler(On_DoubleClicked);
            _localplayer.Click  += new EventHandler(On_Clicked);

            _localplayer.AutoSize = false;
            _localplayer.Visible  = false;
            IsVisible       = false;
            IsPlaying       = false;
            IsHighlighted   = false;
            IsFullScreen    = false;

            IdleThread = new Thread(() =>
            {
                while (true)
                {
                    IsPlaying = _player.Status == VFVideoCaptureStatus.Work || _localplayer.Status == VFMediaPlayerStatus.Play;
                    //Ultility.LogWrite("./log.txt", "IsPlaying: " + IsPlaying.ToString());
                    //Ultility.Debug(" RTSP: " + (_player.Status).ToString() + 
                    //              " Local: " + (_localplayer.Status).ToString());
                }
            });
        }

        /* visibility and location */

        public void setVisible(bool visible)
        
        {
            if (visible)
            {
                reveal();
            }
            else
            {
                hide();
            }
        }

        public void reveal()
        {
            IsVisible = true;
            //ui.Controls.Add(_localplayer);
            //ui.Controls.Add(_player);
            Controls.Add(_localplayer);
            Controls.Add(_player);
            _player.Visible      = true;
            _localplayer.Visible = false;
            this.Visible         = true;
        }

        public void hide()
        { 
            // hide 
            AsyncStopLocal();
            AsyncStopRTSP();
            IsVisible = false;

            //ui.Controls.Remove(_player);
            //ui.Controls.Remove(_localplayer);
            Controls.Remove(_player);
            Controls.Remove(_localplayer);
            this.Visible = false;
        }

        public void setLocation(int xx, int yy, int wid, int hei)
        {
            this.x      = xx;
            this.y      = yy;
            this.width  = wid;
            this.height = hei;

            _player.SetBounds(3, 3, width - 6, height - 6);
            _localplayer.SetBounds(3, 3, width - 6, height - 6);
        }

        /* play and stop */
        [Obsolete]
        public void Play(string url)
        {
            PlayURL = url;
#if Debug
            Ultility.Debug("play url: " + url);
#endif
            if (!Ultility.IsStarted(IdleThread))
            {
                IdleThread.Start();
            }
            else if (Ultility.IsInterrupted(IdleThread))
            {
                IdleThread.Resume();
            }

            Debug.Assert(IdleThread.ThreadState == System.Threading.ThreadState.Running);

            if (Ultility.IsRTSP(url))
            {
                playRTSP(url);
            }
            else
            {
                playLocal(url);
            }
            IsPlaying = true;
        }

        public void playRTSP(string URL)
        {
            AsyncStopLocal();
            _player.Stop();

            _player.IP_Camera_Source = new VisioForge.Types.Sources.IPCameraSourceSettings()
            {
                URL = URL,
            };
            _player.Audio_PlayAudio = _player.Audio_RecordAudio = false;
            _player.Mode = VisioForge.Types.VFVideoCaptureMode.IPPreview;

            _player.Start();
            IsRTSP = true;
        }

        public void playLocal(string URL)
        {
            AsyncStopRTSP();
            _player.Visible = false;

            _localplayer.Stop();
            _localplayer.Visible = true;

            _localplayer.FilenamesOrURL = new List<string>();
            _localplayer.FilenamesOrURL.Add(URL);

            _localplayer.Play();
            IsRTSP = false;
        }

        public void pause()
        {
            Ultility.Debug("Something paused: " + PlayURL);

            if (!PlayURL.Equals(""))
            {
                IdleThread.Interrupt();
                if (Ultility.IsRTSP(PlayURL))
                {
                    _player.Pause();
                }
                else
                {
                    _localplayer.Pause();
                }
                IsPlaying = false;
            }
        }

        [Obsolete]
        public void resume()
        {
            if (!PlayURL.Equals(""))
            {
                Ultility.Debug("Something resumed: " + PlayURL);
                IsPlaying = true;
                if (Ultility.IsRTSP(PlayURL))
                {
                    _player.Resume();
                }
                else
                {
                    _localplayer.Resume();
                }
                if (Ultility.IsInterrupted(IdleThread))
                {
                    IdleThread.Resume();
                }
            }
        }
        public void SeqStop()
        {
            _player.Stop();
            _localplayer.Stop();
            IdleThread.Interrupt();
            IsPlaying = false;
        }

        public void AsynStop()
        {
            AsyncStopLocal();
            AsyncStopRTSP();
        }

        private async void AsyncStopLocal()
        {
            await Task.Run(() => { _localplayer.Stop(); });
            IdleThread.Interrupt();
            IsPlaying = false;
        }

        private async void AsyncStopRTSP()
        {
            await Task.Run(() => { _player.Stop(); });
            IdleThread.Interrupt();
            IsPlaying = false;
        }

        public bool GetPlayingStatue()
        {
            // this can be optimizated
            Ultility.Debug(IsPlaying.ToString());
            return IsPlaying;
        }

        /* messaging */

        private void On_DoubleClicked(object sender, EventArgs e)
        {
#if DEBUG
            Ultility.Debug("triggered inner");
#endif
            ui.Container_OnClick(this);
        }

        private void On_Clicked(object sender, EventArgs e)
        {
            // highlight the player that is clicked once, using a 
            IsHighlighted = !IsHighlighted;
            Highlight();
        }

        private void Highlight()
        {
            // player's location is 5, 5, width, height

            if (IsHighlighted)
            {
                this.BackColor = Color.Orange;
            }
            else
            {
                this.BackColor = Color.White;
            }
        }

        /* delete */

        public void dispose()
        {
            IdleThread.Abort();
            IdleThread = null;

            //ui.Controls.Remove(_player);
            //ui.Controls.Remove(_localplayer);
            Controls.Remove(_player);
            Controls.Remove(_localplayer);
            _player.Stop();
            _player.Dispose();
            _localplayer.Stop();
            _localplayer.Dispose();
        }
        ~MediaPlayer()
        {
            IdleThread.Abort();
            IdleThread = null;

            //ui.Controls.Remove(_player);
            //ui.Controls.Remove(_localplayer);
            Controls.Remove(_player);
            Controls.Remove(_localplayer);
            _player.Stop();
            _player.Dispose();
            _localplayer.Stop();
            _localplayer.Dispose();
        }
    }
}

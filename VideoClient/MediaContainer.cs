using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisioForge.Shared.WindowsMediaLib;
using VisioForge.Types;

namespace VideoClient
{
    internal class MediaContainer : Panel
    {
        /* logical size and location */
        private int width;
        private int height;

        private Panel FullScreenContainer;

        private bool  IsFullScreenMode;
        private bool  IsFullScreen;

        /* defines the number of players in the form */
        private enum PLAYER_NUMBER
        {   ONE = 1,
            FOUR = 2,
            NINE = 3,
        };
        public int kNumContainers;
        
        private List<MediaPlayer> PlayerList;    // all media players
        private List<MediaPlayer> CurrentPlayer; // curr player in this form
        private MediaPlayer FullScreenPlayer;
        public MediaContainer(int width, int height) 
        {
            /* Set logical location and size */
            SetLogicalSize(width, height);

            /* generate 13 players, 
             * and put into the all playerlist */
            PlayerList = new List<MediaPlayer>();
            for (int i = 0; i < 13; i++)
            {
                MediaPlayer player = new MediaPlayer(this);
                PlayerList.Add(player); // creation
            };
            
            /* set default number of containers = 1 
             * and re-render the container */
            SetNumberContainer(1);
            CurrentPlayer = new List<MediaPlayer>();
            RetrievePlayers();
            Render();

            IsFullScreenMode = false;
            IsFullScreen     = false;
            FullScreenContainer = new Panel();
        }
        public void SetLogicalSize(int width, int height)
        {
            /* Set overall area, when the client is firstly initalized or the user trying to move the window.
             * if currently there are some container on GUI
             * reset the locations (not interfer anything) */

            this.width  = width;
            this.height = height;
        }
        public int SetNumberContainer(int num)
        {
            /* num can only be 1, 4, and 9.
             * this func only set value by this policy and do no more */
            switch (num)
            {
                case 1:
                    kNumContainers = (int)PLAYER_NUMBER.ONE;
                    break;
                case 4:
                    kNumContainers = (int)PLAYER_NUMBER.FOUR;
                    break;
                case 9:
                    kNumContainers = (int)PLAYER_NUMBER.NINE;
                    break;
            }
            return num;
        }
        public void RetrievePlayers()
        {
            int currCnt = CurrentPlayer.Count;
            int max = kNumContainers * kNumContainers;
            List<Task> tasks = new List<Task>();
            
            if (currCnt == max) 
            { 
                return; 
            }
            else if (max > currCnt) // add some player to the current list
            {
                for (int i = 0; i < max - currCnt; i++)
                {
                    MediaPlayer temp = PlayerList[i] as MediaPlayer;
                    CurrentPlayer.Add(temp);
                    PlayerList[i] = null;
                };
            }
            else // delete some  
            {
                for (int i = 0; i < currCnt - max; i++)
                {
                    MediaPlayer temp = CurrentPlayer[currCnt - i - 1] as MediaPlayer;
                    CurrentPlayer[currCnt - i - 1] = null;
                    PlayerList.Add(temp);
                };
            }

            tasks.Clear();

            PlayerList    =    PlayerList.Where(x => x != null).ToList();
            CurrentPlayer = CurrentPlayer.Where(x => x != null).ToList();

#if DEBUG
            int result = (PlayerList.Count + CurrentPlayer.Count);
            Ultility.Debug("+ count:" + result);
#endif
        }
        private void SetPlayerLocation(MediaPlayer player, int x, int y, int width, int height)
        {
            player.setLocation(x, y, width, height);
            player.SetBounds(  x, y, width, height); 
            Controls.Add(player);
            player.Visible = true;
            if (!player.IsVisible)
                player.setVisible(true);
        }
        public void Render()
        {
            if (IsFullScreen)
            {
#if DEBUG
                Ultility.Debug("Full Screen Mode");
#endif
                SetPlayerLocation(FullScreenPlayer, 0, 0, width, height);
            }
            else
            {
                int width  = this.width  / kNumContainers;
                int height = this.height / kNumContainers;
                for (int cnt = 0; cnt < kNumContainers * kNumContainers; cnt++)
                {
                    int row = cnt % kNumContainers;
                    int col = cnt / kNumContainers;
                    MediaPlayer curr = CurrentPlayer[cnt] as MediaPlayer;
                    int x =  width * row;
                    int y = height * col;
                    SetPlayerLocation(curr, x, y, width, height);
                }

                for (int cnt = 0; cnt < PlayerList.Count; cnt++)
                {
                    if (PlayerList[cnt].IsVisible == true)
                    {
                        PlayerList[cnt].setVisible(false);
                        Controls.Remove(PlayerList[cnt]);
                    }
                }
            }
        }
        public void Play(string url) 
        {

            //Ultility.Debug("playing "+url);

            MediaPlayer player = FindFreePlayer();
            if (player == null)
            {
                player = CurrentPlayer[0];
            }

            if (player.IsPlaying != false)
                player.AsynStop();
            player.Play(url);

            // to detect if is playing ??? 
        }

        public void Pause()
        {
            PauseAll();
        }

        public void PauseAll()
        {
            foreach(MediaPlayer player in CurrentPlayer)
            {
                if (player.IsPlaying)
                {
                    player.pause();
                }
            }
        }

        public void PauseSelected()
        {
            foreach (MediaPlayer player in CurrentPlayer)
            {
                if (player.IsHighlighted && player.IsPlaying)
                {
                    player.pause();
                }
            }
        }

        public void Resume()
        {
            ResumeAll();
        }

        public void ResumeAll()
        {
            foreach(MediaPlayer player in CurrentPlayer)
            {
                if (!player.IsPlaying)
                {
                    player.resume();
                }
            }
        }

        public void StopAll()
        {
            foreach (MediaPlayer player in CurrentPlayer)
            {
                player.AsynStop();
            }
        }

        private MediaPlayer FindFreePlayer()
        {
            foreach (MediaPlayer player in CurrentPlayer) {
                if (player.IsPlaying == false) { return player; }
            }
            return null;
        }

        public void Container_OnClick(object sender)
        {
            // the sender wanna be full-screened
            FullScreenPlayer = sender as MediaPlayer;
            IsFullScreen = !IsFullScreen;
            bool OtherVisible = !IsFullScreen;
            foreach (MediaPlayer player in CurrentPlayer)
            {
                player.Visible = OtherVisible;
            }

            Render();
            //string msg = clicked.X.ToString() + clicked.Y.ToString();
            //Ultility.Debug("outer triggered");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            foreach(MediaPlayer player in PlayerList)
            {
                player.dispose();
            }

            foreach(MediaPlayer player in CurrentPlayer)
            {
                player.dispose();
            }
        }
    }
}

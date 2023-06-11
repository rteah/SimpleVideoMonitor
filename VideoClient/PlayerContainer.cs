using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisioForge.Shared.WindowsMediaLib;

namespace VideoClient
{
    internal class PlayerContainer : Panel
    {
        private int x;
        private int y;
        private int width;
        private int height;

        private int MaxPlayingVideo;



        public PlayerContainer(int x, int y, int width, int height)
        {
            SetAreaLocation(x, y, width, height);
            //IsFullScreen = false;

            // initialize 9 media players in advance
            // once finished, add 1 media into the current avaiable media and render this to the area
            if (IsAnyPlaying())
            {

            }

        }

        public void SetAreaLocation(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            
            // if there are current-displayed media player entity, reset their location, too.
            
        }

        public void SetPlayerNumber(int k)
        {
            //this.kNumberEachLine = k;
        }

        public ArrayList RenderPlayers(ref ArrayList players)
        {
            // render current avaible players to the area
            // 

            return {};
        }

        private bool IsAnyPlaying() { return false; }

    }
}

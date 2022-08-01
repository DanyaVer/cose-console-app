using Game1.Assets;
using Game1.GameLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Game1.Timer
{
    public class GameTimer
    {       
        public ulong time;

        #region Constructor
        public GameTimer(ulong time = 0)
        {
            this.time = time;
        }
        #endregion

        public void MakeTimer()
        {
            time = 0;
            while (true)
            {
                Thread.Sleep(TimeSpan.FromMilliseconds(1));
                time++;
                OutPutTheTime(ref time);
            }
        }

        private void OutPutTheTime(ref ulong time)
        {
            ulong minutes = 0, seconds = 0, miliseconds = 0;
            miliseconds = time % 100;
            seconds = time / 100 % 100;
            minutes = time / 10000;
            if (miliseconds == 60)
            {
                seconds++;
                miliseconds = 0;
                time = minutes * 10000 + seconds * 100;
            }
            if (seconds == 60)
            {
                minutes++;
                seconds = 0;
                time = minutes * 10000 + miliseconds;
            }
            lock (GameMaze.locker)
            {
                Console.SetCursorPosition(0, Constants.SCENE_HEIGHT + 2);
                if (minutes < 10)
                    Console.Write("0" + Convert.ToString(minutes) + ".");
                else
                    Console.Write(Convert.ToString(minutes) + ".");
                
                if (seconds < 10)
                    Console.Write("0" + Convert.ToString(seconds) + ".");
                else
                    Console.Write(Convert.ToString(seconds) + ".");

                if (miliseconds < 10)
                    Console.Write("0" + Convert.ToString(miliseconds));
                else
                    Console.Write(Convert.ToString(miliseconds));
            }
        }
    }
}

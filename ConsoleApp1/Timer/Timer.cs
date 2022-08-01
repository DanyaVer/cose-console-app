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
        public static ulong time = 0;

        public void MakeTimer(GameMaze game)
        {
            time = 0;
            while (game.gameRunning && !game.isTheLevelPassed)
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
            if (miliseconds == 99)
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
                WriteTime((int)miliseconds, (int)seconds, (int)minutes);
            }

        }
        public static void WriteTime(int miliseconds, int seconds, int minutes)
        {
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
            Console.WriteLine("\n");
        }
    }
}

using System;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using Game1.GameLogic;
using Game1.GameLogic.MazeCreation;

namespace Game1
{
    class Program
    {
        public static object locker = new();
        public static void MyPrintMethod1(string name)
        {
            lock (locker)
            {
                for (int i = 0; i < 15; i++)
                {
                    Console.WriteLine("LOl 1 " + name);

                }
            } 
        }
        public static void MyPrintMethod2(string name)
        {
            lock (locker)
            {
                for (int i = 0; i < 15; i++)
                {
                    Console.WriteLine("LOl 2 " + name);
                }
            }          
        }
        public static void MyPrintMethod3(string name)
        {
            lock (locker)
            {
                for (int i = 0; i < 15; i++)
                {

                    Console.WriteLine("LOl 3 " + name);
                }
            }           
        }
        public static void Main(string[] args)
        {
            //String name = "Danya";
            //Thread t1 = new Thread(() => MyPrintMethod1(name));
            //Thread t2 = new Thread(() => MyPrintMethod2(name));
            //Thread t3 = new Thread(() => MyPrintMethod3(name));

            //t1.Start();
            //t2.Start();
            //t3.Start();

            //t1.Join();
            //t2.Join();
            //t3.Join();

            //Console.WriteLine(Thread.CurrentThread.CurrentCulture);
            //return;
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.CursorVisible = false;

            GameMaze game = new GameMaze();
            GameLoop(game);

            Clear();


        }

        private static void Clear()
        {
            Console.Clear();
            Console.WriteLine("Game was closed.");
        }

        private static void GameLoop(GameMaze game)
        {
            game.LaunchScreen();
            while (game.keepPlaying)
            {
                game.InitializeScene();
                Console.Clear();
                game.Render();

                while (game.gameRunning && !game.isTheLevelPassed)
                {
                    game.HandleInput();
                    game.Render();
                    if (game.gameRunning)
                    {
                        Thread.Sleep(TimeSpan.FromMilliseconds(33));
                    }
                }

                if (game.isTheLevelPassed)
                {
                    ConsoleKey key = Console.ReadKey(true).Key;
                    Console.Clear();

                    game.WinOrNot = "You passed The Maze!\n";
                    game.GameOverScreen();
                }
                else
                {
                    ConsoleKey key = Console.ReadKey(true).Key;
                    Console.Clear();
                    game.WinOrNot = "You haven't passed The Maze(\n";
                    game.GameOverScreen();

                }
            }
        }
    }
}



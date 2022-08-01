using System;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using Game1.GameLogic;
using Game1.GameLogic.MazeCreation;
using Game1.Assets;
using Game1.Timer;

namespace Game1
{
    class Program
    {        
        public static void Main(string[] args)
        {            
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
            GameTimer t = new GameTimer();
            Thread threadTimer = new Thread(() => t.MakeTimer());

            game.LaunchScreen();
            while (game.keepPlaying)
            {
                game.InitializeScene();
                Console.Clear();
                game.Render();

                threadTimer.Start();
                while (game.gameRunning && !game.isTheLevelPassed)
                {
                    game.HandleInput();
                    game.Render();
                    if (game.gameRunning)
                    {
                        Thread.Sleep(TimeSpan.FromMilliseconds(33));
                    }
                }
                threadTimer.Join();

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



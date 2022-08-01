using Game1.Assets;
using Game1.GameLogic.MazeCreation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Game1.GameLogic
{
    public class GameMaze : Maze
    {

        #region Variables
        public bool gameRunning;
        public bool keepPlaying;
        public bool isTheLevelPassed;
        public int chosenOption;
        public int chosenGameOverOption;
        public int gameLevel;

        public int score;
        public string WinOrNot;

        public int walkerPosX;
        public int walkerPosY;

        private readonly Random random;

        public static object locker = new();
        #endregion
        #region Constructors

        public GameMaze(
            char walkerCharacter = '■',
            int walkerPosX = 1, int walkerPosY = 1,
            bool gameRunning = true, bool keepPlaying = true,
            bool isTheLevelPassed = false, int chosenOption = 1,
            int chosenGameOverOption = 1,
            int gameLevel = 3, int score = 0,
            string winOrNot = "") : base(walkerCharacter: walkerCharacter)
        {

            random = new Random();
 
            this.walkerPosX = walkerPosX;
            this.walkerPosY = walkerPosY;
            this.gameRunning = gameRunning;
            this.keepPlaying = keepPlaying;
            this.isTheLevelPassed = isTheLevelPassed;
            this.chosenOption = chosenOption;
            this.chosenGameOverOption = chosenGameOverOption;
            this.gameLevel = gameLevel;
            this.score = score;
            this.WinOrNot = winOrNot;
        }
        #endregion

        #region Private Methods
        private void ClearEverything()
        {
            walkerCharacter = '■';
            walkerPosX = 1;
            walkerPosY = 1;
            score = 0;
            isTheLevelPassed = false;
        }
        private void PressEnterToContinue()
        {
        GetInput:
            ConsoleKey key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.Enter:
                    EnterTheOption();
                    break;
                case ConsoleKey.UpArrow:
                case ConsoleKey.W:
                    if (chosenOption != 1)
                    {
                        chosenOption--;
                        LaunchScreen();
                    }
                    else
                        goto GetInput;
                    break;
                case ConsoleKey.DownArrow:
                case ConsoleKey.S:
                    if (chosenOption != 4)
                    {
                        chosenOption++;
                        LaunchScreen();
                    }
                    else
                        goto GetInput;
                    break;
                case ConsoleKey.Escape:
                    keepPlaying = false;
                    break;
                default: goto GetInput;
            }
        }
        private void EnterTheOption()
        {
            if (chosenOption == 1)
                return;
            if (chosenOption == 2)
            {
                LevelChoice();
                LaunchScreen();
            }
            if (chosenOption == 3)
                PlayerHelp();
            if (chosenOption == 4)
                keepPlaying = false;
        }
        private void EnterTheGameOverOption()
        {
            if (chosenGameOverOption == 1)
                return;
            if (chosenGameOverOption == 2)
            {
                LevelChoice();
                GameOverScreen();
            }
            if (chosenOption == 3)
                keepPlaying = false;
        }
        private void PlayerHelp()
        {
            Console.Clear();
            string[] fileLines = System.IO.File.ReadAllLines(@"c:\Users\Danya\source\repos\C Sharp\1 task\Help.txt");

            Console.WriteLine(" > Exit");
            Console.WriteLine();
            if (fileLines != null)
            {
                //Console.WriteLine($"The file has {fileLines.Length} lines.");
                for (int i = 0; i < fileLines.Length; i++)
                {
                    Console.WriteLine(fileLines[i]);
                }
                //Thread.Sleep(TimeSpan.FromSeconds(10));
            }
        GetInput:
            ConsoleKey key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.Enter:
                case ConsoleKey.Escape:
                    LaunchScreen();
                    break;
                default: goto GetInput;
            }
        }
        private void LevelChoice()
        {
            Console.Clear();
            Console.WriteLine("Current level of game is 3 (medium)\n");
            Console.WriteLine("Type from 1 (the easiest) to 5 (the most difficult) level\n");
            Console.Write(" > ");
            Console.CursorVisible = true;
            string a = Console.ReadLine();
            while (a.Length != 1 || (int)a[0] < (int)'1' || (int)a[0] > (int)'5')
            {
                Console.WriteLine("\nSomething has gone wrong\n\n Try again\n");
                Console.Write(" > ");
                a = Console.ReadLine();
            }
            Console.CursorVisible = false;
            gameLevel = Convert.ToInt32(a);
            Console.Clear();
        }
        #endregion

        #region Public Methods

        public void LaunchScreen()
        {
            StringBuilder stringBuilder = new StringBuilder();
            string[] beginnings = { "   ", "   ", "   ", "   " };

            beginnings[chosenOption - 1] = " > ";

            stringBuilder.AppendLine("This is a Maze game.");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(beginnings[0] + "Start the game");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(beginnings[1] + "Choose difficulty level");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(beginnings[2] + "Help");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(beginnings[3] + "Exit");
            Console.SetCursorPosition(0, 0);
            Console.Write(stringBuilder);

            PressEnterToContinue();
        }        
        public void InitializeScene()
        {
            ClearEverything();
            int minSize = gameLevel * 5;
            gameRunning = true;
            Constants.SCENE_HEIGHT = minSize + random.Next(5) + 2;
            Constants.SCENE_WIDTH = minSize + random.Next(5) + 2;
            scene = new char[Constants.SCENE_HEIGHT, Constants.SCENE_WIDTH];
            sceneIsUsed = new bool[Constants.SCENE_HEIGHT, Constants.SCENE_WIDTH];

            Build();
        }
        public void Render()
        {
            StringBuilder stringBuilder = new StringBuilder(Constants.SCENE_WIDTH * Constants.SCENE_HEIGHT);
            for (int i = Constants.SCENE_HEIGHT - 1; i >= 0; i--)
            {
                for (int j = 0; j < Constants.SCENE_WIDTH; j++)
                {
                    stringBuilder.Append(scene[i, j]);
                }
                if (i > 0)
                {
                    stringBuilder.AppendLine();
                }
            }

            //matrix of colours
            lock (locker)
            {
                Console.SetCursorPosition(0, 0);
                Console.Write(stringBuilder);
            }
        }
        public void HandleInput()
        {            
            while (Console.KeyAvailable)
            {
                ConsoleKey key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.A:
                    case ConsoleKey.LeftArrow:
                        walkerCharacter = '←';
                        if (scene[walkerPosY, walkerPosX - 1] == ' ' || scene[walkerPosY, walkerPosX - 1] == '+')
                        {
                            scene[walkerPosY, walkerPosX] = ' ';
                            walkerPosX--;
                            scene[walkerPosY, walkerPosX] = walkerCharacter;
                        }
                        else
                            scene[walkerPosY, walkerPosX] = walkerCharacter;
                        break;
                    case ConsoleKey.D:
                    case ConsoleKey.RightArrow:
                        walkerCharacter = '→';
                        if (scene[walkerPosY, walkerPosX + 1] == ' ' || scene[walkerPosY, walkerPosX + 1] == '+')
                        {
                            scene[walkerPosY, walkerPosX] = ' ';
                            walkerPosX++;
                            scene[walkerPosY, walkerPosX] = walkerCharacter;
                        }
                        else
                            scene[walkerPosY, walkerPosX] = walkerCharacter;
                        break;
                    case ConsoleKey.W:
                    case ConsoleKey.UpArrow:
                        walkerCharacter = '↑';
                        if (scene[walkerPosY + 1, walkerPosX] == ' ' || scene[walkerPosY + 1, walkerPosX] == '+')
                        {
                            scene[walkerPosY, walkerPosX] = ' ';
                            walkerPosY++;
                            scene[walkerPosY, walkerPosX] = walkerCharacter;
                        }
                        else
                            scene[walkerPosY, walkerPosX] = walkerCharacter;
                        break;
                    case ConsoleKey.S:
                    case ConsoleKey.DownArrow:
                        walkerCharacter = '↓';
                        if (scene[walkerPosY - 1, walkerPosX] == ' ' || scene[walkerPosY - 1, walkerPosX] == '+')
                        {
                            scene[walkerPosY, walkerPosX] = ' ';
                            walkerPosY--;
                            scene[walkerPosY, walkerPosX] = walkerCharacter;
                        }
                        else
                            scene[walkerPosY, walkerPosX] = walkerCharacter;
                        break;
                    case ConsoleKey.Escape:
                        gameRunning = false;
                        keepPlaying = false;
                        break;
                }
            }
            if (walkerPosX == Constants.SCENE_WIDTH - 2 && walkerPosY == Constants.SCENE_HEIGHT - 2)
            {
                walkerCharacter = '●';
                scene[walkerPosY, walkerPosX] = walkerCharacter;
                isTheLevelPassed = true;
            }
        }
        public void GameOverScreen()
        {
            Console.SetCursorPosition(0, 0);
            string[] beginnings = { "   ", "   ", "   " };
            beginnings[chosenGameOverOption - 1] = " > ";
            Console.WriteLine(WinOrNot);
            Console.WriteLine($"Count of steps: {score}\n");
            Console.WriteLine(beginnings[0] + "Play Again\n");
            Console.WriteLine(beginnings[1] + "Choose difficulty level\n");
            Console.WriteLine(beginnings[2] + "Exit\n");
        GetInput:
            ConsoleKey key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.Enter:
                    EnterTheGameOverOption();
                    break;
                case ConsoleKey.UpArrow:
                case ConsoleKey.W:
                    if (chosenGameOverOption != 1)
                    {
                        chosenGameOverOption--;
                        GameOverScreen();
                    }
                    else
                        goto GetInput;
                    break;
                case ConsoleKey.DownArrow:
                case ConsoleKey.S:
                    if (chosenGameOverOption != 3)
                    {
                        chosenGameOverOption++;
                        GameOverScreen();
                    }
                    else
                        goto GetInput;
                    break;
                case ConsoleKey.Escape:
                    keepPlaying = false;
                    break;
                default: goto GetInput;
            }
        }
        
        #endregion
    }

}

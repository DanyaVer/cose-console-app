using System;
using System.Text;
using System.Threading;
using System.Collections.Generic;


namespace Game1
{
    class Program
    {
        public static int sceneWidth = 10;
        public static int sceneHeight = 10;

        public Random rand = new Random();
        public static char[,] scene;
        public static bool[,] sceneIsUsed;

        public static char walkerCharacter = '■';
        public static int walkerPosX = 1;
        public static int walkerPosY = 1;

        public static bool gameRunning;
        public static bool keepPlaying = true;
        public static bool isTheLevelPassed = false;
        public static int chosenOption = 1;
        public static int chosenGameOverOption = 1;
        public static int gameLevel = 3;

        public static int score = 0;
        public static string WinOrNot = "";
        
        public static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Program P = new Program();
            Console.CursorVisible = false;
            try
            {
                //P.Initialize();
                P.LaunchScreen();
                while (keepPlaying)
                {
                    P.InitializeScene();
                    Console.Clear();
                    P.Render();

                    while (gameRunning && !isTheLevelPassed)
                    {
                        P.HandleInput();
                        P.Render();
                        if (gameRunning)
                        {
                            Thread.Sleep(TimeSpan.FromMilliseconds(33));
                        }
                    }
                    if (isTheLevelPassed)
                    {
                        ConsoleKey key = Console.ReadKey(true).Key;
                        Console.Clear();
                        WinOrNot = "You passed The Maze!\n";
                        P.GameOverScreen();
                    }
                    else
                    {
                        ConsoleKey key = Console.ReadKey(true).Key;
                        Console.Clear();
                        WinOrNot = "You haven't passed The Maze(\n";
                        P.GameOverScreen();

                    }
                }
                Console.Clear();
                /*
                if (consoleSizeError)
                {
                    Console.WriteLine("Console/Terminal window is too small.");
                    Console.WriteLine($"Minimum size is {width} width x {height} height.");
                    Console.WriteLine("Increase the size of the console window.");
                }
                */
                Console.WriteLine("Game was closed.");
            }
            finally
            {
                Console.CursorVisible = true;
            }
        }

        void LaunchScreen()
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

        void ClearEverything()
        {
            walkerCharacter = '■';
            walkerPosX = 1;
            walkerPosY = 1;
            score = 0;
        }

        void InitializeScene()
        {
            ClearEverything();
            int minSize = gameLevel * 5;
            gameRunning = true;
            sceneHeight = minSize + rand.Next(5) + 2;
            sceneWidth = minSize + rand.Next(5) + 2;
            scene = new char[sceneHeight, sceneWidth];
            sceneIsUsed = new bool[sceneHeight, sceneWidth];
            for (int i = 0; i < sceneHeight; i++)
            {
                for (int j = 0; j < sceneWidth; j++)
                {
                    sceneIsUsed[i, j] = false;
                }
            }

            MakeBorders();
            MakeMaze();

        }

        void MakeBorders()
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < sceneWidth; j++)
                {
                    scene[(i + sceneHeight - 1) % sceneHeight, (j + sceneWidth - 1) % sceneWidth] = '—';
                    sceneIsUsed[(i + sceneHeight - 1) % sceneHeight, (j + sceneWidth - 1) % sceneWidth] = true;
                }
            }
            for (int i = 0; i < sceneHeight; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    scene[i, (j + sceneWidth - 1) % sceneWidth] = '|';
                    sceneIsUsed[i, (j + sceneWidth - 1) % sceneWidth] = true;
                }
            }
            for (int i = 1; i < sceneHeight - 1; i++)
            {
                for (int j = 1; j < sceneWidth - 1; j++)
                {
                    if (scene[i, j] == '\0')
                    {
                        scene[i, j] = ' ';
                    }

                }
            }
        }

        void MakeMaze()
        {
            List<Coordinate> queue = new List<Coordinate>();
            queue.Add(new Coordinate() { x = 1, y = 1 });
            //int randNum;
            int randX, randY;

            MakeAWinWay();

            for (int i = 0; i < sceneHeight * sceneWidth + rand.Next(15); i++)
            {
                randX = rand.Next(sceneHeight - 2) + 1;
                randY = rand.Next(sceneWidth - 2) + 1;
                if (sceneIsUsed[randX, randY])
                    continue;
                if (rand.Next(2) == 0)
                    scene[randX, randY] = '|';
                else
                    scene[randX, randY] = '—';
                sceneIsUsed[randX, randY] = true;
            }

            /*
            while (queue.Count != 0)
            {
                if (!sceneIsUsed[queue[0].x, queue[0].y])
                {
                    randNum = rand.Next(4);
                    if (randNum == 0 && sceneIsUsed[queue[0].x + 1, queue[0].y])
                    {
                        scene[queue[0].x, queue[0].y] = ' ';
                    }
                }
                queue.RemoveAt(0);
            }

            Console.Read();
            */
        }

        void MakeAWinWay()
        {

            scene[1, 1] = walkerCharacter;
            sceneIsUsed[1, 1] = true;

            bool actualChange = false;

            int countOfIteretions = 0, tempArg = 0;

            int currentRandNum, previousRandNum = -1;
            int wayX = 1, wayY = 1;
            while ((wayX != sceneWidth - 2 || wayY != sceneHeight - 2) && countOfIteretions != 100000)
            {
                countOfIteretions++;
                currentRandNum = rand.Next(4);
                if ((currentRandNum + 2) % 4 != previousRandNum)
                {
                    actualChange = false;
                    //moves up once
                    if (currentRandNum == 0 && !sceneIsUsed[wayY + 1, wayX] && (!sceneIsUsed[wayY + 1, wayX + 1] || wayX == sceneWidth - 2) && (!sceneIsUsed[wayY + 1, wayX - 1] || scene[wayY + 1, wayX - 1] == '|') && (!sceneIsUsed[wayY + 2, wayX] || scene[wayY + 2, wayX] == '—'))
                    {
                        wayY++;
                        sceneIsUsed[wayY, wayX] = true;
                        PutHereA0(wayY, wayX);
                        previousRandNum = currentRandNum;
                        actualChange = true;
                    }
                    //moves right once
                    if (currentRandNum == 1 && !sceneIsUsed[wayY, wayX + 1] && (!sceneIsUsed[wayY + 1, wayX + 1] || wayY == sceneHeight - 2) && (!sceneIsUsed[wayY - 1, wayX + 1] || scene[wayY - 1, wayX + 1] == '—') && (!sceneIsUsed[wayY, wayX + 2] || scene[wayY, wayX + 2] == '|'))
                    {
                        wayX++;
                        sceneIsUsed[wayY, wayX] = true;
                        PutHereA0(wayY, wayX);
                        previousRandNum = currentRandNum;
                        actualChange = true;
                    }
                    //moves down once
                    if (currentRandNum == 2 && !sceneIsUsed[wayY - 1, wayX] && !sceneIsUsed[wayY - 1, wayX - 1] && !sceneIsUsed[wayY - 2, wayX])
                    {
                        tempArg = wayX;
                        while (tempArg != sceneHeight - 1 && !sceneIsUsed[wayY - 1, tempArg])
                            tempArg++;
                        if (tempArg != sceneHeight - 1 || sceneWidth - wayX < 4)
                            continue;
                        wayY--;
                        sceneIsUsed[wayY, wayX] = true;
                        PutHereA0(wayY, wayX);
                        previousRandNum = currentRandNum;
                        actualChange = true;
                    }
                    //moves left once
                    if (currentRandNum == 3 && !sceneIsUsed[wayY, wayX - 1] && !sceneIsUsed[wayY - 1, wayX - 1] && !sceneIsUsed[wayY, wayX - 2])
                    {
                        tempArg = wayY;
                        while (tempArg != sceneHeight - 1 && !sceneIsUsed[tempArg, wayX - 1])
                            tempArg++;
                        if (tempArg != sceneHeight - 1 || sceneHeight - wayY < 4)
                            continue;
                        wayX--;
                        sceneIsUsed[wayY, wayX] = true;
                        PutHereA0(wayY, wayX);
                        previousRandNum = currentRandNum;
                        actualChange = true;
                    }
                    if (actualChange)
                        MakeAnObstacle(ref wayX, ref wayY);
                }
            }
            scene[sceneHeight - 2, sceneWidth - 2] = '+';
            sceneIsUsed[sceneHeight - 2, sceneWidth - 2] = true;
            Console.WriteLine(countOfIteretions);
            //Console.Read();
        }

        void MakeAnObstacle(ref int wayX, ref int wayY)
        {
            if (wayX >= sceneWidth - 3 && wayY >= sceneHeight - 3)
            {
                if (wayX == sceneWidth - 3 && wayY == sceneHeight - 3)
                    if (rand.Next(2) == 1)
                    {
                        sceneIsUsed[wayY + 1, wayX] = true;
                        PutHereA0(wayY + 1, wayX);
                    }
                    else
                    {
                        sceneIsUsed[wayY, wayX + 1] = true;
                        PutHereA0(wayY, wayX + 1);
                    }
                wayY = sceneHeight - 2;
                wayX = sceneWidth - 2;
            }
            //obstacle to the right
            if (wayX < sceneWidth - 2 && sceneIsUsed[wayY, wayX + 2] && !sceneIsUsed[wayY, wayX + 1] && wayX < sceneWidth - 3)
            {
                sceneIsUsed[wayY, wayX + 1] = true;
                scene[wayY, wayX + 1] = '|';
            }
            //obstacle to the left
            if (wayX > 1 && sceneIsUsed[wayY, wayX - 2] && !sceneIsUsed[wayY, wayX - 1])
            {
                sceneIsUsed[wayY, wayX - 1] = true;
                scene[wayY, wayX - 1] = '|';
            }
            //obstacle above
            if (wayY < sceneHeight - 2 && sceneIsUsed[wayY + 2, wayX] && !sceneIsUsed[wayY + 1, wayX] && wayY < sceneHeight - 3)
            {
                sceneIsUsed[wayY + 1, wayX] = true;
                scene[wayY + 1, wayX] = '—';
            }
            //obstacle below
            if (wayY > 1 && sceneIsUsed[wayY - 2, wayX] && !sceneIsUsed[wayY - 1, wayX])
            {
                sceneIsUsed[wayY - 1, wayX] = true;
                scene[wayY - 1, wayX] = '—';
            }

        }

        void PutHereA0(int wayY, int wayX)
        {
            //scene[wayY, wayX] = '0';
        }

        void Render()
        {
            StringBuilder stringBuilder = new StringBuilder(sceneWidth * sceneHeight);
            for (int i = sceneHeight - 1; i >= 0; i--)
            {
                for (int j = 0; j < sceneWidth; j++)
                {
                    stringBuilder.Append(scene[i, j]);
                }
                if (i > 0)
                {
                    stringBuilder.AppendLine();
                }
            }
            Console.SetCursorPosition(0, 0);
            Console.Write(stringBuilder);
        }

        void HandleInput()
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
            if (walkerPosX == sceneWidth - 2 && walkerPosY == sceneHeight - 2)
            {
                walkerCharacter = '●';
                scene[walkerPosY, walkerPosX] = walkerCharacter;
                isTheLevelPassed = true;
            }
        }

        void GameOverScreen()
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

        //in Main menu choose the option you want.
        void PressEnterToContinue()
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

        void EnterTheOption()
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

        void EnterTheGameOverOption()
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

        void PlayerHelp()
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

        void LevelChoice()
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

    }
}



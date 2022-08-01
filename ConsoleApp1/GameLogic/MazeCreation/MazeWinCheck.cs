using Game1.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1.GameLogic.MazeCreation
{
    public class MazeWinCheck
    {
        private readonly Random random;
        public MazeWinCheck()
        {
            random = new Random();
        }
        public void MakeAWinWay(ref char[,] scene, ref bool[,] sceneIsUsed)
        {
            bool actualChange = false;

            int countOfIteretions = 0, tempArg = 0;

            int currentRandNum, previousRandNum = -1;
            int wayX = 1, wayY = 1;
            while ((wayX != Constants.SCENE_WIDTH - 2 || wayY != Constants.SCENE_HEIGHT - 2) && countOfIteretions != 100000)
            {
                countOfIteretions++;
                currentRandNum = random.Next(4);
                if ((currentRandNum + 2) % 4 != previousRandNum)
                {
                    actualChange = false;
                    //moves up once
                    if (currentRandNum == 0 && !sceneIsUsed[wayY + 1, wayX] && (!sceneIsUsed[wayY + 1, wayX + 1] || wayX == Constants.SCENE_WIDTH - 2) && (!sceneIsUsed[wayY + 1, wayX - 1] || scene[wayY + 1, wayX - 1] == '|') && (!sceneIsUsed[wayY + 2, wayX] || scene[wayY + 2, wayX] == '—'))
                    {
                        wayY++;
                        sceneIsUsed[wayY, wayX] = true;
                        PutHereA0(ref scene, wayY, wayX);
                        previousRandNum = currentRandNum;
                        actualChange = true;
                    }
                    //moves right once
                    if (currentRandNum == 1 && !sceneIsUsed[wayY, wayX + 1] && (!sceneIsUsed[wayY + 1, wayX + 1] || wayY == Constants.SCENE_HEIGHT - 2) && (!sceneIsUsed[wayY - 1, wayX + 1] || scene[wayY - 1, wayX + 1] == '—') && (!sceneIsUsed[wayY, wayX + 2] || scene[wayY, wayX + 2] == '|'))
                    {
                        wayX++;
                        sceneIsUsed[wayY, wayX] = true;
                        PutHereA0(ref scene, wayY, wayX);
                        previousRandNum = currentRandNum;
                        actualChange = true;
                    }
                    //moves down once
                    if (currentRandNum == 2 && !sceneIsUsed[wayY - 1, wayX] && !sceneIsUsed[wayY - 1, wayX - 1] && !sceneIsUsed[wayY - 2, wayX])
                    {
                        tempArg = wayX;
                        while (tempArg != Constants.SCENE_HEIGHT - 1 && !sceneIsUsed[wayY - 1, tempArg])
                            tempArg++;
                        if (tempArg != Constants.SCENE_HEIGHT - 1 || Constants.SCENE_WIDTH - wayX < 4)
                            continue;
                        wayY--;
                        sceneIsUsed[wayY, wayX] = true;
                        PutHereA0(ref scene, wayY, wayX);
                        previousRandNum = currentRandNum;
                        actualChange = true;
                    }
                    //moves left once
                    if (currentRandNum == 3 && !sceneIsUsed[wayY, wayX - 1] && !sceneIsUsed[wayY - 1, wayX - 1] && !sceneIsUsed[wayY, wayX - 2])
                    {
                        tempArg = wayY;
                        while (tempArg != Constants.SCENE_HEIGHT - 1 && !sceneIsUsed[tempArg, wayX - 1])
                            tempArg++;
                        if (tempArg != Constants.SCENE_HEIGHT - 1 || Constants.SCENE_HEIGHT - wayY < 4)
                            continue;
                        wayX--;
                        sceneIsUsed[wayY, wayX] = true;
                        PutHereA0(ref scene, wayY, wayX);
                        previousRandNum = currentRandNum;
                        actualChange = true;
                    }
                    if (actualChange)
                        MakeAnObstacle(ref scene, ref sceneIsUsed, ref wayX, ref wayY);
                }
            }
            if (countOfIteretions == 10000)
            {
                GameMaze temp = new GameMaze();
                temp.IsWinWayPossible = false;

            }
            scene[Constants.SCENE_HEIGHT - 2, Constants.SCENE_WIDTH - 2] = '+';
            sceneIsUsed[Constants.SCENE_HEIGHT - 2, Constants.SCENE_WIDTH - 2] = true;
            //Console.WriteLine(countOfIteretions);
            //Console.Read();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="sceneIsUsed"></param>
        /// <param name="wayX"></param>
        /// <param name="wayY"></param>
        private void MakeAnObstacle(ref char[,] scene, ref bool[,] sceneIsUsed, ref int wayX, ref int wayY)
        {
            if (wayX >= Constants.SCENE_WIDTH - 3 && wayY >= Constants.SCENE_HEIGHT - 3)
            {
                if (wayX == Constants.SCENE_WIDTH - 3 && wayY == Constants.SCENE_HEIGHT - 3)
                    if (random.Next(2) == 1)
                    {
                        sceneIsUsed[wayY + 1, wayX] = true;
                        PutHereA0(ref scene, wayY + 1, wayX);
                    }
                    else
                    {
                        sceneIsUsed[wayY, wayX + 1] = true;
                        PutHereA0(ref scene, wayY, wayX + 1);
                    }
                wayY = Constants.SCENE_HEIGHT - 2;
                wayX = Constants.SCENE_WIDTH - 2;
            }
            //obstacle to the right
            if (wayX < Constants.SCENE_WIDTH - 2 && sceneIsUsed[wayY, wayX + 2] && !sceneIsUsed[wayY, wayX + 1] && wayX < Constants.SCENE_WIDTH - 3)
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
            if (wayY < Constants.SCENE_HEIGHT - 2 && sceneIsUsed[wayY + 2, wayX] && !sceneIsUsed[wayY + 1, wayX] && wayY < Constants.SCENE_HEIGHT - 3)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wayY"></param>
        /// <param name="wayX"></param>
        private void PutHereA0(ref char[,] scene, int wayY, int wayX)
        {
            //scene[wayY, wayX] = '0';
        }
    }
}

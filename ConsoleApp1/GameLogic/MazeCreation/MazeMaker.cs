using Game1.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1.GameLogic.MazeCreation
{
    public class MazeMaker
    {
        private readonly Random random;
        public MazeMaker()
        {
            random = new Random();
        }
        public void MakeMaze(ref char[,] scene, ref bool[,] sceneIsUsed, char walkerCharacter)
        {
            List<Coordinate<int, int>> queue = new List<Coordinate<int, int>>();
            queue.Add(new Coordinate<int, int>() { x = 1, y = 1 });

            int randX, randY;


            scene[1, 1] = walkerCharacter;
            sceneIsUsed[1, 1] = true;

            for (int i = 0; i < Constants.SCENE_HEIGHT * Constants.SCENE_WIDTH + random.Next(15); i++)
            {
                randX = random.Next(Constants.SCENE_HEIGHT - 2) + 1;
                randY = random.Next(Constants.SCENE_WIDTH - 2) + 1;
                if (sceneIsUsed[randX, randY])
                    continue;
                if (random.Next(2) == 0)
                    scene[randX, randY] = '|';
                else
                    scene[randX, randY] = '—';
                sceneIsUsed[randX, randY] = true;
            }
        }
    }
}

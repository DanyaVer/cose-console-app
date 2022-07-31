using Game1.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1.GameLogic.MazeCreation
{
    public class MazeBorders
    {
        public MazeBorders()
        {

        }
        public void MakeBorders(ref char[,] scene, ref bool[,] sceneIsUsed)
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < Constants.SCENE_WIDTH; j++)
                {
                    scene[(i + Constants.SCENE_HEIGHT - 1) % Constants.SCENE_HEIGHT, (j + Constants.SCENE_WIDTH - 1) % Constants.SCENE_WIDTH] = '—';
                    sceneIsUsed[(i + Constants.SCENE_HEIGHT - 1) % Constants.SCENE_HEIGHT, (j + Constants.SCENE_WIDTH - 1) % Constants.SCENE_WIDTH] = true;
                }
            }
            for (int i = 0; i < Constants.SCENE_HEIGHT; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    scene[i, (j + Constants.SCENE_WIDTH - 1) % Constants.SCENE_WIDTH] = '|';
                    sceneIsUsed[i, (j + Constants.SCENE_WIDTH - 1) % Constants.SCENE_WIDTH] = true;
                }
            }
            for (int i = 1; i < Constants.SCENE_HEIGHT - 1; i++)
            {
                for (int j = 1; j < Constants.SCENE_WIDTH - 1; j++)
                {
                    if (scene[i, j] == '\0')
                    {
                        scene[i, j] = ' ';
                    }

                }
            }
        }
    }
}

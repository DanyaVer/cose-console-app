﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1.GameLogic.MazeCreation
{
    public class Maze
    {
        #region Variables

        public char[,] scene;
        public bool[,] sceneIsUsed;

        public char walkerCharacter;
        public int walkerPosX;
        public int walkerPosY;     
        #endregion

        public Maze()
        {
            MazeMaker = new MazeMaker();
            MazeBorders = new MazeBorders();
            MazeWinCheck = new MazeWinCheck();
        }

        public Maze(char walkerCharacter, int walkerPosX, int walkerPosY) : this()
        {
            this.walkerCharacter = walkerCharacter;
            this.walkerPosX = walkerPosX;
            this.walkerPosY = walkerPosY;
        }

        public MazeMaker MazeMaker { get; set; }
        public MazeBorders MazeBorders { get; set; }
        public MazeWinCheck MazeWinCheck { get; set; }      
        
        public void Build()
        {
            MazeBorders.MakeBorders(ref this.scene, ref this.sceneIsUsed);
            MazeWinCheck.MakeAWinWay(ref this.scene, ref this.sceneIsUsed);
            MazeMaker.MakeMaze(ref this.scene, ref this.sceneIsUsed, this.walkerCharacter);
        }
    }
}

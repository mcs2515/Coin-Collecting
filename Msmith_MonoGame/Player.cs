using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Msmith_MonoGame
{
    class Player: GameObject
    {
        private int levelScore;
        private int totalScore;

        //PROPERTIES
        public int LevelScore
        {
            get { return levelScore; }
            set { levelScore = value; }
        }

        public int TotalScore
        {
            get { return totalScore; }
            set { totalScore = value; }
        }

        //CONSTRUCTOR
        //send the x,y,width,height parameters to base class
        public Player(int x, int y, int width, int height)
            :base(x,y, width, height)
        {
            //set scores to 0 at start of game
            levelScore = 0;
            totalScore = 0;
        }


    }
}

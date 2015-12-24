using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Msmith_MonoGame
{
    class Collectible:GameObject
    {

        bool active;

        //PROPERTIES
        public bool IsActive
        {
            get { return active; }
            set { active = value; }
        }

        //CONSTRUCTOR
        //send the x,y,width,height parameters to base class
        public Collectible(int x, int y, int width, int height)
            : base(x, y, width, height)
        {
            //set collectibles to be active at start of game
            active = true;
        }

        //CHECK COLLISION METHOD
        public bool CheckCollision(GameObject obj)
        {
            //is collectible is active....
            if (this.active)
            {
                //and it's rectangle is colliding with another's
                if (obj.ImageRectangle.Intersects(this.ImageRectangle))
                {
                    return true;
                }
                else
                {
                    return false;
                }                
            }
            else
            {
                return false;
            }
        }

        //DRAW METHOD
        public override void Draw(SpriteBatch sprite)
        {
            if (this.active)
            {
                base.Draw(sprite);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Msmith_MonoGame
{
    public class GameObject
    {
        //objects
        private Rectangle imageRectangle;
        private Texture2D imageTexture;

        //CONSTRUCTOR
        public GameObject(int x, int y, int width, int height)
        {
            imageRectangle = new Rectangle(x,y,width,height);
        }

        //PROPERTIES
        public Rectangle ImageRectangle
        {
            get { return imageRectangle; }
            set { imageRectangle = value; }
        }

        public Texture2D Image
        {
            get { return imageTexture; }
            set { imageTexture = value; }
        }

        public int X
        {
            get { return imageRectangle.X; }
            set { imageRectangle.X = value; }
        }

        public int Y
        {
            get { return imageRectangle.Y; }
            set { imageRectangle.Y = value; }
        }


        //DRAW MEHOD
        //child classes can overwrite method
        public virtual void Draw(SpriteBatch sprite)
        {
            //call the draw method
            sprite.Draw(imageTexture, imageRectangle, Color.White);
        }
    }
}

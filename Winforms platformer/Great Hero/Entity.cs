using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winforms_platformer
{
    class Entity
    {

        public Bitmap idleSheet { get; protected set; }
        public Bitmap fullSizeSprite { get; protected set; }
        public int spritesWidth { get; protected set; }
        public int spritesHeight { get; protected set; }
        public int X { get; protected set; }
        public int Y { get; protected set; }
        public int currentFrame { get; protected set; }
        protected int currentMaxFrames;
        protected Func<int, int, int, int> GetYSpeed;
        protected int ySpeed;

        protected void MoveY()
        {
            ySpeed = GetYSpeed(X, Y - spritesHeight, ySpeed);
            Y += ySpeed;
        }

        public void StepFrame()
        {
            currentFrame++;
            if (currentFrame >= currentMaxFrames)
                currentFrame = 0;
        }
    }
}

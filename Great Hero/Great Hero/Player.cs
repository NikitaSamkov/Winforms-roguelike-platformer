using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Great_Hero
{
    class Player : Creature
    {
        public readonly Bitmap idleSheet;
        public readonly Bitmap moveSheet;
        public readonly Bitmap fullSizeSprite;
        public readonly int spritesWidth;
        public readonly int spritesHeight;

        public Player(int x, int y)
        {
            X = x;
            Y = y;
            currentDirection = Direction.Right;
            currentAnimation = AnimationType.Idle;
            speed = 5;
            idleSheet = new Bitmap(@"..\..\..\..\Sprites\PlayerIdle.png");
            moveSheet = new Bitmap(@"..\..\..\..\Sprites\PlayerMove.png");
            fullSizeSprite = new Bitmap(@"..\..\..\..\Sprites\PlayerFullSize.png");
            spritesWidth = fullSizeSprite.Width;
            spritesHeight = fullSizeSprite.Height;
            idleMaxFrames = idleSheet.Width / fullSizeSprite.Width;
            moveMaxFrames = moveSheet.Width / fullSizeSprite.Width;
        }

        public Bitmap GetSheet()
        {
            switch (currentAnimation)
            {
                case AnimationType.Idle:
                    return idleSheet;
                case AnimationType.Move:
                    return moveSheet;
            }
            return idleSheet;
        }
    }
}

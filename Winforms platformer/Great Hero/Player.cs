using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Winforms_platformer
{
    class Player : Creature
    {
        
        public Player(int x, int y, Func<int, int, int, int> moveY)
        {
            X = x;
            Y = y;
            this.GetYSpeed = moveY;
            currentDirection = Direction.Right;
            currentStatus = Status.Idle;
            xSpeed = 5;
            idleSheet = new Bitmap(@"..\..\..\..\Sprites\Player\PlayerIdle.png");
            moveSheet = new Bitmap(@"..\..\..\..\Sprites\Player\PlayerMove.png");
            fullSizeSprite = new Bitmap(@"..\..\..\..\Sprites\Player\PlayerFullSize.png");
            spritesWidth = fullSizeSprite.Width;
            spritesHeight = fullSizeSprite.Height;
            idleMaxFrames = idleSheet.Width / fullSizeSprite.Width;
            moveMaxFrames = moveSheet.Width / fullSizeSprite.Width;
        }

        public Bitmap GetSheet()
        {
            switch (currentStatus)
            {
                case Status.Idle:
                    return idleSheet;
                case Status.Move:
                    return moveSheet;
            }
            return idleSheet;
        }
    }
}

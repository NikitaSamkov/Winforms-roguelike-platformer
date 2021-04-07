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

        public Player()
        {
            X = 150;
            Y = 150;
            currentDirection = Direction.Right;
            currentAnimation = AnimationType.Idle;
            speed = 5;
            idleSheet = new Bitmap(@"..\..\..\..\Sprites\PlayerIdle.png");
            moveSheet = new Bitmap(@"..\..\..\..\Sprites\PlayerMove.png");
            ySpeedup = 10;
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

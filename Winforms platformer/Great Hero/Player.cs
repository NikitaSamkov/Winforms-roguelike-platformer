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
        public Player(int x, int y, int width, Func<int, int, int, int> moveY)
        {
            base.x = x;
            base.y = y;
            this.GetYSpeed = moveY;
            currentDirection = Direction.Right;
            xSpeed = 5;
            this.width = width;
        }
    }
}

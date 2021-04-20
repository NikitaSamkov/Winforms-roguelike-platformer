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
        public Player(int x, int y, int width, Func<int, int, int, int, int> moveY, Func<int, int, int, bool> canJump)
        {
            this.x = x;
            this.y = y;
            getYSpeed = moveY;
            this.canJump = canJump;
            currentDirection = Direction.Right;
            xSpeed = 5;
            jumpStrength = 50;
            this.width = width;
        }
    }
}

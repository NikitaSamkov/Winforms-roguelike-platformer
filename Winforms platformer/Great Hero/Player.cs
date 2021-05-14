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
    public class Player : Creature
    {
        public List<ITreasure> treasures { get; set; }
        public Player(int x, int y, int playerWidth, Func<int, int, int, int, int> moveY, 
            Func<int, int, int, bool> canJump) 
            : base(x, y, playerWidth, moveY, canJump)
        {
            currentDirection = Direction.Right;
            xSpeed = 20;
            treasures = new List<ITreasure>();
        }
    }
}

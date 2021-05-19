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
        public Player(int x, int y, Collider collider, Room room) 
            : base(x, y, collider, room)
        {
            currentDirection = Direction.Right;
            xSpeed = 20;
            treasures = new List<ITreasure>();
            hp = 100;
            damage = 10;
        }
    }
}

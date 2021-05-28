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
        public Player(int x, int y, Collider collider, Func<Room> room) 
            : base(x, y, collider, room)
        {
            direction = Direction.Right;
            xSpeed = 20;
            treasures = new List<ITreasure>();
            hp = 100;
            damage = 10;
        }

        public override void Shoot()
        {
            if (Ammo > 0)
            {
                var arrow = new Arrow(x, y + collider.field.Height / 2,
                        new Collider(Resources.Arrow.IdleSize), CurrentRoom, 15, bowStrenght, ProjectileType.Ally);
                arrow.MoveTo(direction);
                arrow.status = Status.Move;
                CurrentRoom().ProjectilesList.Add(arrow);
                Ammo--;
            }
        }
    }
}

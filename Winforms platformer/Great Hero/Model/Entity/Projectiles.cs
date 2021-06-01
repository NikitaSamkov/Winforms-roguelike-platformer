using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winforms_platformer
{
    public class Projectile : Entity
    {
        public ProjectileType type;
        public Creature Sender;
        public Projectile(int x, int y, Collider collider, Func<Room> room, int angle, int strenght, ProjectileType type, Creature sender) 
            : base(x, y, collider, room)
        {
            HP = 100;
            damage = 20;
            xSpeed = (int)Math.Round(Math.Cos(Math.PI / 180 * angle) * strenght);
            ySpeed = -(int)Math.Round(Math.Sin(Math.PI / 180 * angle) * strenght);
            this.type = type;
            Sender = sender;
        }
    }

    public class Arrow : Projectile
    {
        public Arrow(int x, int y, Collider collider, Func<Room> room, int angle, int strenght, ProjectileType type, Creature sender) 
            : base(x, y, collider, room, angle, strenght, type, sender)
        {
        }
    }
}

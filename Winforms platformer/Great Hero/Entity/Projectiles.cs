using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winforms_platformer
{
    public class Arrow : Entity
    {
        public Arrow(int x, int y, Collider collider, Room room, int angle, int strenght) : base(x, y, collider, room)
        {
            hp = 100;
            damage = 20;
            xSpeed = (int)Math.Round(Math.Cos(Math.PI / 180 * angle) * strenght);
            ySpeed = -(int)Math.Round(Math.Sin(Math.PI / 180 * angle) * strenght);
        }

        protected override void MoveY()
        {
            ySpeed = room.GetYSpeed(x, y + collider.field.Height, collider.field.Width, ySpeed);
            y += ySpeed;
        }
    }
}

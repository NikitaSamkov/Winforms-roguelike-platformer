using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winforms_platformer
{
    public class Entity
    {
        protected int ySpeed;
        protected int xSpeed = 5;
        protected int damageInvincibility;
        public int x { get; protected set; }
        public int y { get; protected set; }
        public Direction direction { get; protected set; }
        public Status status { get; set; }
        public Collider collider { get; protected set; }
        public int hp { get; set; }
        public int damage { get; protected set; }
        public int invincibility { get; protected set; }
        public Room room;

        public Entity(int x, int y, Collider collider, Room room)
        {
            this.x = x;
            this.y = y;
            this.collider = collider;
            this.room = room;
            damageInvincibility = 12;
        }

        protected virtual void MoveY()
        {
            ySpeed = room.GetYSpeed(x, y + collider.field.Height, collider.field.Width, ySpeed);
            y += ySpeed;
        }

        public virtual void Update()
        {
            if (status == Status.Move)
                switch (direction)
                {
                    case Direction.Left:
                        x -= xSpeed;
                        break;
                    case Direction.Right:
                        x += xSpeed;
                        break;
                }
            MoveY();
            if (invincibility > 0)
                invincibility--;
        }

        public void Hurt(int damage)
        {
            if (invincibility == 0)
            {
                invincibility = damageInvincibility;
                hp -= damage;
            }
        }

        public bool IntersectsWithBody(Entity target)
        {
            return new Rectangle(new Point(collider.Left + x, collider.Top + x - collider.field.Height), collider.field)
                .IntersectsWith(new Rectangle(new Point(target.collider.Left + target.x,
                target.collider.Top + x - collider.field.Height), target.collider.field));
        }

        public void MoveTo(Direction direction) => this.direction = direction;
        public void MoveDown(int distance) => y += distance;
        public void MoveDown() => y += xSpeed;
        public void MoveUp() => y -= xSpeed;
        public void TeleportTo(int x) => TeleportTo(x, y);
        public int GetDistanceTo(int x, int y) => (int)Math.Sqrt((this.x - x) * (this.x - x) + (this.y - y) * (this.y - y));
        public void TeleportTo(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}

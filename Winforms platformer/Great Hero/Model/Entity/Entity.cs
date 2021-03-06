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
        public int xSpeed = 5;
        public int damageInvincibility;
        public double hurtMultiplier = 1;
        public int x { get; protected set; }
        public int y { get; protected set; }
        public Direction direction { get; protected set; }
        public Status status { get; set; }
        public Collider collider { get; protected set; }
        public int invincibility { get; protected set; }
        public int HP { get; set; }
        public int MaxHP { get; set; }
        public int damage { get; set; }
        public bool flying { get; set; }
        public Func<Room> CurrentRoom;

        public Entity(int x, int y, Collider collider, Func<Room> CurrentRoom)
        {
            this.x = x;
            this.y = y;
            this.collider = collider;
            this.CurrentRoom = CurrentRoom;
            HP = 50;
            MaxHP = HP;
            damageInvincibility = 12;
        }

        protected virtual void MoveY()
        {
            ySpeed = CurrentRoom().GetYSpeed(x, y + collider.field.Height + collider.y, collider.field.Width, ySpeed);
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
                if (damage > 0)
                    invincibility = damageInvincibility;
                HP -= (int)(damage * hurtMultiplier);
            }
        }

        public bool IntersectsWithBody(Entity target)
        {
            return IntersectsWithBody(new Rectangle(new Point(target.x + target.collider.x, target.y + target.collider.y), target.collider.field));
        }

        public bool IntersectsWithBody(Rectangle target)
        {
            return new Rectangle(new Point(x + collider.x, y + collider.y), collider.field)
                .IntersectsWith(target);
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

        public void FreezeInAir() => ySpeed = 0;
    }
}

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
        public int x { get; protected set; }
        public int y { get; protected set; } // (x, y) - левый нижний угол
        public Direction currentDirection { get; protected set; }
        public Status status { get; set; }
        public Collider collider { get; protected set; }
        protected Func<int, int, int, int, int> getYSpeed;
        protected int ySpeed;
        protected int xSpeed = 5;
        protected int hp = 100;
        protected int damage = 0;

        public Entity(int x, int y, Collider collider, Func<int, int, int, int, int> moveY)
        {
            this.x = x;
            this.y = y;
            this.collider = collider;
            getYSpeed = moveY;
            this.status = status;
        }

        protected void MoveY()
        {
            ySpeed = getYSpeed(x, y, collider.field.Width, ySpeed);
            y += ySpeed;
        }

        public virtual void Move()
        {
            if (status == Status.Move)
                switch (currentDirection)
                {
                    case Direction.Left:
                        x -= xSpeed;
                        break;
                    case Direction.Right:
                        x += xSpeed;
                        break;
                }
            MoveY();
        }

        public void MoveTo(Direction direction) => currentDirection = direction;
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

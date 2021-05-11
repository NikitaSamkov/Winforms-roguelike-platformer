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
        public bool flying { get; set; }
        public int width { get; protected set; }
        protected Func<int, int, int, int, int> getYSpeed;
        protected int ySpeed;
        protected int xSpeed;
        protected int jumpStrength;
        protected int hp;
        protected Func<int, int, int, bool> canJump;

        public Entity(int x, int y, int entityWidth, Func<int, int, int, int, int> moveY, Func<int, int, int, bool> canJump)
        {
            this.x = x;
            this.y = y;
            width = entityWidth;
            getYSpeed = moveY;
            this.canJump = canJump;
            this.status = status;
        }

        protected void MoveY()
        {
            ySpeed = getYSpeed(x, y, width, ySpeed);
            y += ySpeed;
        }
        public void Move()
        {
            if (status == Status.Move || (flying && !(status == Status.Idle)))
                switch (currentDirection)
                {
                    case Direction.Left:
                        x -= xSpeed;
                        break;
                    case Direction.Right:
                        x += xSpeed;
                        break;
                }
            if (!flying)
                MoveY();
        }

        public void MoveTo(Direction direction) => currentDirection = direction;

        public void MoveTo(Entity target) => currentDirection = (x - target.x >= 0) ? Direction.Left : Direction.Right;

        public void Jump()
        {
            if (canJump(x, y, width) && ySpeed == 0)
            {
                ySpeed -= jumpStrength;
            }
        }

        public void MoveDown(int distance) => y += distance;
        public void MoveDown() => y += xSpeed;

        public void MoveUp(int distance) => y += distance;
        public void MoveUp() => y -= xSpeed;

        public void TeleportTo(int x) => TeleportTo(x, y);

        public void TeleportTo(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public IEnumerable<Point> GetJumpTrajectory()
        {
            var trajectoryXSpeed = (currentDirection == Direction.Right) ? xSpeed : -xSpeed;
            var trajectoryYSpeed = ySpeed - jumpStrength;
            var trajectoryX = x + trajectoryXSpeed;
            var trajectoryY = y + trajectoryYSpeed;
            trajectoryYSpeed = getYSpeed(trajectoryX, trajectoryY, width, trajectoryYSpeed);
            while (!canJump(trajectoryX, trajectoryY, width))
            {
                yield return new Point(trajectoryX, trajectoryY);
                trajectoryYSpeed = getYSpeed(trajectoryX, trajectoryY, width, trajectoryYSpeed);
                trajectoryX += trajectoryXSpeed;
                trajectoryY += trajectoryYSpeed;
            }
        }

        public void UpdateRoom(Func<int, int, int, bool> onTheSurface, Func<int, int, int, int, int> getYSpeed)
        {
            canJump = onTheSurface;
            this.getYSpeed = getYSpeed;
        }
    }
}

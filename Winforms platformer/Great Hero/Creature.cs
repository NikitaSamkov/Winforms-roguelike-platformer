using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winforms_platformer
{
    public class Creature : Entity
    {
        public Direction currentDirection { get; protected set; }
        protected int xSpeed;
        protected int jumpStrength;
        protected int hp;
        protected Func<int, int, int, bool> canJump;

        public Creature(int x, int y, int creatureWidth, Func<int, int, int, int, int> moveY, Func<int, int, int, bool> canJump)
            : base(x, y, creatureWidth, moveY)
        {
            this.canJump = canJump;
        }

        public void Move(Status currentStatus)
        {
            if (currentStatus == Status.Move)
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

        public void MoveTo(Direction direction)
        {
            currentDirection = direction;
        }

        public void MoveTo(Entity target)
        {
            currentDirection = (x - target.x >= 0) ? Direction.Left : Direction.Right;
        }

        public void Jump()
        {
            if (canJump(x, y, width) && ySpeed == 0)
                ySpeed -= jumpStrength;
        }

        public void GoDown()
        {
            y += 1;
        }

        public void TeleportTo(int x)
        {
            this.x = x;
        }

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
            while(!canJump(trajectoryX, trajectoryY, width))
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winforms_platformer
{
    public class Creature : Entity
    {
        public bool flying { get; set; }
        protected int jumpStrength = 50;
        protected Func<int, int, int, bool> canJump;

        public Creature(int x, int y, int entityWidth, Func<int, int, int, int, int> moveY, Func<int, int, int, bool> canJump) 
            : base(x, y, entityWidth, moveY)
        {
            this.canJump = canJump;
        }

        public override void Move()
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

        public void Jump()
        {
            if (canJump(x, y, width) && ySpeed == 0)
            {
                ySpeed -= jumpStrength;
            }
        }

        public void UpdateRoom(Func<int, int, int, bool> onTheSurface, Func<int, int, int, int, int> getYSpeed)
        {
            canJump = onTheSurface;
            this.getYSpeed = getYSpeed;
        }

        //public IEnumerable<Point> GetJumpTrajectory()
        //{
        //    var trajectoryXSpeed = (currentDirection == Direction.Right) ? xSpeed : -xSpeed;
        //    var trajectoryYSpeed = ySpeed - jumpStrength;
        //    var trajectoryX = x + trajectoryXSpeed;
        //    var trajectoryY = y + trajectoryYSpeed;
        //    trajectoryYSpeed = getYSpeed(trajectoryX, trajectoryY, width, trajectoryYSpeed);
        //    while (!canJump(trajectoryX, trajectoryY, width))
        //    {
        //        yield return new Point(trajectoryX, trajectoryY);
        //        trajectoryYSpeed = getYSpeed(trajectoryX, trajectoryY, width, trajectoryYSpeed);
        //        trajectoryX += trajectoryXSpeed;
        //        trajectoryY += trajectoryYSpeed;
        //    }
        //}
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winforms_platformer
{
    class Creature : Entity
    {
        public Bitmap moveSheet { get; protected set; }
        public Direction currentDirection { get; protected set; }
        protected int xSpeed;
        protected Status currentStatus;
        protected int idleMaxFrames;
        protected int moveMaxFrames;


        public void Move()
        {
            if (currentStatus == Status.Move)
                switch (currentDirection)
                {
                    case Direction.Left:
                        X -= xSpeed;
                        break;
                    case Direction.Right:
                        X += xSpeed;
                        break;
                }
            MoveY();
        }

        public void MoveTo(Direction direction)
        {
            currentStatus = Status.Move;
            currentDirection = direction;
            currentMaxFrames = moveMaxFrames;
            currentFrame = 0;
        }

        public void SetIdle()
        {
            currentStatus = Status.Idle;
            currentMaxFrames = idleMaxFrames;
            currentFrame = 0;
        }
    }
}

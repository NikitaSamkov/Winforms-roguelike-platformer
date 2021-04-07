using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Great_Hero
{
    class Creature : Entity
    {
        public Direction currentDirection { get; protected set; }
        protected int speed;
        protected AnimationType currentAnimation;
        protected int idleMaxFrames;
        protected int moveMaxFrames;

        public void Move(Direction direction)
        {
            currentAnimation = AnimationType.Move;
            currentMaxFrames = moveMaxFrames;
            currentDirection = direction;
            switch (direction)
            {
                case Direction.Left:
                    X -= speed;
                    break;
                case Direction.Right:
                    X += speed;
                    break;
            }
        }

        public void SetIdle()
        {
            currentAnimation = AnimationType.Idle;
            currentMaxFrames = idleMaxFrames;
            currentFrame = 0;
        }
    }
}

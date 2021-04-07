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

        public void Move(Direction direction)
        {
            currentAnimation = AnimationType.Move;
            maxFrames = 2;
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
            maxFrames = 1;
            currentFrame = 0;
        }
    }
}

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
        public Direction currentDirection { get; set; }
        protected int xSpeed;
        protected int jumpStrength;
        protected int hp;
        protected Func<int, int, int, bool> canJump;

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

        public void Jump()
        {
            if (canJump(x, y, width) && ySpeed == 0)
                ySpeed -= jumpStrength;
        }
    }
}

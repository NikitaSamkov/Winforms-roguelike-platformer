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
        public int width { get; protected set; }
        protected Func<int, int, int, int, int> getYSpeed;
        protected int ySpeed;

        public Entity(int x, int y, int entityWidth, Func<int, int, int, int, int> moveY)
        {
            this.x = x;
            this.y = y;
            width = entityWidth;
            getYSpeed = moveY;
        }

        protected void MoveY()
        {
            ySpeed = getYSpeed(x, y, width, ySpeed);
            y += ySpeed;
        }
    }
}

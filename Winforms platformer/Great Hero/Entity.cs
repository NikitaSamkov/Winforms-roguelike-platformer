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
        public int y { get; protected set; }
        public int width { get; protected set; }
        protected Func<int, int, int, int> GetYSpeed;
        protected int ySpeed;

        protected void MoveY()
        {
            ySpeed = GetYSpeed(x, y, ySpeed);
            y += ySpeed;
        }
    }
}

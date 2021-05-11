using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winforms_platformer
{
    public class Platform
    {
        public readonly int level;
        public readonly int leftBorder;
        public readonly int rightBorder;

        public Platform(int from, int to, int level)
        {
            this.leftBorder = from;
            this.rightBorder = to;
            this.level = level;
        }
    }
}

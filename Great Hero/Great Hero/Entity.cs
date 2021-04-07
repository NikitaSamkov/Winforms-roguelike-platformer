using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Great_Hero
{
    class Entity
    {
        public int X { get; protected set; }
        public int Y { get; protected set; }
        public int currentFrame { get; protected set; }
        protected int maxFrames;
        protected int ySpeedup;

        public void StepFrame()
        {
            currentFrame++;
            if (currentFrame >= maxFrames)
                currentFrame = 0;
        }
    }
}

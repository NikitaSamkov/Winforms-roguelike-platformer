using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winforms_platformer
{
    class Room
    {
        public int gravitation { get; private set; }
        private int groundLevel;
        private List<Platform> platforms;

        public Room(int groundLevel)
        {
            platforms = new List<Platform>();
            gravitation = 5;
            this.groundLevel = groundLevel;
        }

        public int GetYSpeed(int x, int y, int width, int speed)
        {
            var newY = y + speed + gravitation;
            foreach (var platform in platforms)
                if (platform.field.Top >= y && platform.field.Top < newY &&
                    platform.field.Left < x + width && platform.field.Right > x)
                    newY = platform.field.Top;
            if (newY > groundLevel)
                newY = groundLevel;
            return newY - y;
        }

        public bool OnTheSurface(int x, int y, int width)
        {
            if (y == groundLevel)
                return true;
            foreach (var platform in platforms)
                if (platform.field.Top == y &&
                    platform.field.Left < x + width && platform.field.Right > x)
                    return true;
            return false;
        }
    }
}

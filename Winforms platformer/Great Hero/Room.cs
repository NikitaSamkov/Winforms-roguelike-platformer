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
        public int gForce { get; private set; }
        public readonly int groundLevel;
        public readonly List<Platform> platforms;

        public Room(int groundLevel, List<Platform> platforms, int gravitationForce = 5)
        {
            this.platforms = platforms;
            gForce = gravitationForce;
            this.groundLevel = groundLevel;
        }

        public int GetYSpeed(int x, int y, int width, int speed)
        {
            var newY = y + speed + gForce;
            foreach (var platform in platforms)
                if (platform.level >= y && platform.level < newY &&
                    platform.leftBorder < x + width && platform.rightBorder > x)
                    newY = platform.level;
            if (newY > groundLevel)
                newY = groundLevel;
            return newY - y;
        }

        public bool OnTheSurface(int x, int y, int width)
        {
            if (y == groundLevel)
                return true;
            foreach (var platform in platforms)
                if (platform.level == y &&
                    platform.leftBorder < x + width && platform.rightBorder > x)
                    return true;
            return false;
        }
    }
}

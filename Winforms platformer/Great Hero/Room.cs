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
        public Bitmap groundSheet { get; private set; }
        private Bitmap platformSheet;
        public Bitmap wallSheet { get; private set; }
        public int gravitation { get; private set; }
        private int groundLevel;
        private List<Platform> platforms;
        private int roomWidth;
        private int roomHeight;

        public Room(int roomWidth, int roomHeight)
        {
            this.roomWidth = roomWidth;
            this.roomHeight = roomHeight;
            platforms = new List<Platform>();
            gravitation = 10;
            groundSheet = new Bitmap(@"..\..\..\..\Sprites\Room\Ground.png");
            wallSheet = new Bitmap(@"..\..\..\..\Sprites\Room\Wall.png");
            groundLevel = groundSheet.Height;
        }

        public int GetYSpeed(int x, int y, int speed)
        {
            var newY = y + speed + gravitation;
            foreach (var platform in platforms)
                if (platform.field.Top <= y && platform.field.Top > newY)
                    newY = platform.field.Top;
            if (newY > roomHeight - groundLevel)
                newY = roomHeight - groundLevel;
            return newY - y;
        }
    }
}

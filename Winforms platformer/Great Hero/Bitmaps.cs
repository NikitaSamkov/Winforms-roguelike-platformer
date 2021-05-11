using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winforms_platformer
{
    public static class PlayerBitmaps
    {
        public static Bitmap FullSize = new Bitmap(@"..\..\..\..\Sprites\Player\PlayerFullSize.png");
        public static Bitmap Idle = new Bitmap(@"..\..\..\..\Sprites\Player\PlayerIdle.png");
        public static Bitmap Move = new Bitmap(@"..\..\..\..\Sprites\Player\PlayerMove.png");
    }

    public static class DummyBitmaps
    {
        public static Bitmap FullSize = new Bitmap(@"..\..\..\..\Sprites\Enemy\DummyFullSize.png");
        public static Bitmap Move = new Bitmap(@"..\..\..\..\Sprites\Enemy\DummyWalk.png");
    }

    public static class RoomBitmaps
    {
        public static Bitmap Wall = new Bitmap(@"..\..\..\..\Sprites\Room\Wall.png");
        public static Bitmap Ground = new Bitmap(@"..\..\..\..\Sprites\Room\Ground.png");
    }
}

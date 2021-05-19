using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winforms_platformer
{
    public static class PlayerRes
    {
        public static Bitmap Idle = new Bitmap(@"..\..\..\..\Sprites\Player\PlayerIdle.png");
        public static Bitmap Move = new Bitmap(@"..\..\..\..\Sprites\Player\PlayerMove.png");
        public static Bitmap Attack = new Bitmap(@"..\..\..\..\Sprites\Player\PlayerAttack.png");
        public static Bitmap AttackMove = new Bitmap(@"..\..\..\..\Sprites\Player\PlayerAttack.png");
        public static Size IdleSize = new Size(40, 112);
        public static Size MoveSize = new Size(40, 112);
        public static Size AttackSize = new Size(88, 112);
        public static Size AttackMoveSize = new Size(88, 112);
        public static Size AttackRange = new Size(60, 56);
    }

    public static class DummyRes
    {
        public static Bitmap Idle = new Bitmap(@"..\..\..\..\Sprites\Enemy\DummyIdle.png");
        public static Bitmap Move = new Bitmap(@"..\..\..\..\Sprites\Enemy\DummyWalk.png");
        public static Size IdleSize = new Size(40, 96);
        public static Size MoveSize = new Size(40, 96);
    }

    public static class RoomRes
    {
        public static Bitmap Wall = new Bitmap(@"..\..\..\..\Sprites\Room\Wall.png");
        public static Bitmap Ground = new Bitmap(@"..\..\..\..\Sprites\Room\Ground.png");
        public static Bitmap Death = new Bitmap(@"..\..\..\..\Sprites\Room\Death.png");
    }

    public static class ProjectilesRes
    {
        public static Bitmap Arrow = new Bitmap(@"..\..\..\..\Sprites\Projectiles\Arrow.png");
        public static Size ArrowSize = new Size(50, 10);
    }
}

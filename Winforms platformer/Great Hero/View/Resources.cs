using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winforms_platformer
{
    public static class Resources
    {
        public static readonly PlayerRes Player;
        public static readonly DummyRes Dummy;
        public static readonly RoomRes Room;
        public static readonly ArrowRes Arrow;
        static Resources()
        {
            Player = new PlayerRes();
            Dummy = new DummyRes();
            Room = new RoomRes();
            Arrow = new ArrowRes();
        }
    }

    public class EntityResource
    {
        public Bitmap Idle { get; protected set; }
        public Size IdleSize { get; protected set; }
        public Bitmap Move { get; protected set; }
        public Size MoveSize { get; protected set; }
    }

    public class AttackingEntityResource : EntityResource
    {
        public Bitmap Attack { get; protected set; }
        public Bitmap AttackMove { get; protected set; }
        public Size AttackSize { get; protected set; }
        public Size AttackMoveSize { get; protected set; }
        public Size AttackRange { get; protected set; }
    }

    public class PlayerRes : AttackingEntityResource
    {
        public PlayerRes()
        {
            Idle = new Bitmap(@"..\..\..\..\Sprites\Player\PlayerIdle.png");
            Move = new Bitmap(@"..\..\..\..\Sprites\Player\PlayerMove.png");
            Attack = new Bitmap(@"..\..\..\..\Sprites\Player\PlayerAttack.png");
            AttackMove = new Bitmap(@"..\..\..\..\Sprites\Player\PlayerAttack.png");
            IdleSize = new Size(40, 112);
            MoveSize = new Size(40, 112);
            AttackSize = new Size(88, 112);
            AttackMoveSize = new Size(88, 112);
            AttackRange = new Size(60, 56);
        }
    }

    public class DummyRes : EntityResource
    {
        public DummyRes()
        {
            Idle = new Bitmap(@"..\..\..\..\Sprites\Enemy\DummyIdle.png");
            Move = new Bitmap(@"..\..\..\..\Sprites\Enemy\DummyWalk.png");
            IdleSize = new Size(40, 96);
            MoveSize = new Size(40, 96);
        }
    }

    public class RoomRes
    {
        public readonly Bitmap Wall = new Bitmap(@"..\..\..\..\Sprites\Room\Wall.png");
        public readonly Bitmap Ground = new Bitmap(@"..\..\..\..\Sprites\Room\Ground.png");
        public readonly Bitmap Death = new Bitmap(@"..\..\..\..\Sprites\Room\Death.png");
    }

    public class ArrowRes : EntityResource
    {
        public ArrowRes()
        {
            Idle = new Bitmap(@"..\..\..\..\Sprites\Projectiles\Arrow.png");
            Move = new Bitmap(@"..\..\..\..\Sprites\Projectiles\Arrow.png");
            IdleSize = new Size(50, 10);
            MoveSize = new Size(50, 10);
        }
    }
}

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
        public static readonly SystemRes System = new SystemRes();
        public static readonly PlayerRes Player = new PlayerRes();
        public static readonly DummyRes Dummy = new DummyRes();
        public static readonly RoomRes Room = new RoomRes();
        public static readonly TreasureRoomRes TreasureRoom = new TreasureRoomRes();
        public static readonly ArrowRes Arrow = new ArrowRes();
        public static readonly TreasuresRes Treasures = new TreasuresRes();
        public static readonly UIRes UI = new UIRes();
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

    public class SystemRes
    {
        public readonly Bitmap Death = new Bitmap(@"..\..\..\..\Sprites\Room\Death.png");
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
        public Bitmap Wall { get; protected set; }
        public Bitmap Ground { get; protected set; }
        public RoomRes()
        {
            Wall = new Bitmap(@"..\..\..\..\Sprites\Room\Wall.png");
            Ground = new Bitmap(@"..\..\..\..\Sprites\Room\Ground.png");
        }
    }

    public class TreasureRoomRes : RoomRes
    {
        public TreasureRoomRes()
        {
            Wall = new Bitmap(@"..\..\..\..\Sprites\Room\TreasureWall.png");
            Ground = new Bitmap(@"..\..\..\..\Sprites\Room\TreasureGround.png");
        }
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

    public class UIRes
    {
        public readonly Bitmap HPBar = new Bitmap(@"..\..\..\..\Sprites\UI\hp.png");
        public readonly Bitmap HPFrame = new Bitmap(@"..\..\..\..\Sprites\UI\frame.png");
        public readonly Size HPSize = new Size(275, 38);
        public readonly Bitmap Ammo = new Bitmap(@"..\..\..\..\Sprites\UI\ammo.png");
        public readonly Size AmmoSize = new Size(12, 52);
    }

    public class TreasuresRes
    {
        public readonly Bitmap idNOtFound = new Bitmap(@"..\..\..\..\Sprites\Treasures\id-1.png");
        public readonly Size Size = new Size(75, 70);

        //СТРОГО НАЗЫВАТЬ *ID*id !!!!
        public readonly Bitmap id0 = new Bitmap(@"..\..\..\..\Sprites\Treasures\id0.png");
    }
}

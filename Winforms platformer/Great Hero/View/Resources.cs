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
        #region ENEMIES
        public static readonly DummyRes Dummy = new DummyRes();
        public static readonly SlimeRes Slime = new SlimeRes();
        public static readonly RollerRes Roller = new RollerRes();
        public static readonly SwordsmanRes Swordsman = new SwordsmanRes();
        public static readonly ArcherRes Archer = new ArcherRes();
        public static readonly MagicianRes Magician = new MagicianRes();
        public static readonly SuperMagicianRes SuperMagician = new SuperMagicianRes();
        public static readonly BigCowRes BigCow = new BigCowRes();
        public static readonly GhostRes Ghost = new GhostRes();
        public static readonly InvisibleManRes InvisibleMan = new InvisibleManRes();
        public static readonly TurretRes Turret = new TurretRes();
        public static readonly StickerRes Sticker = new StickerRes();
        public static readonly StickerAttackRes StickerAttack = new StickerAttackRes();
        #endregion
        public static readonly RoomRes Room = new RoomRes();
        public static readonly TreasureRoomRes TreasureRoom = new TreasureRoomRes();
        public static readonly TreasuresRes Treasures = new TreasuresRes();
        public static readonly ArrowRes Arrow = new ArrowRes();
        public static readonly PlasmaRes Plasma = new PlasmaRes();
        public static readonly UIRes UI = new UIRes();
        public static readonly LootRes Loot = new LootRes();
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
            AttackRange = new Size(60, 112);
        }
    }

    #region ENEMIES
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

    public class SlimeRes : EntityResource
    {
        public SlimeRes()
        {
            Idle = new Bitmap(@"..\..\..\..\Sprites\Enemy\SlimeIdle.png");
            Move = new Bitmap(@"..\..\..\..\Sprites\Enemy\SlimeWalk.png");
            IdleSize = new Size(60, 39);
            MoveSize = new Size(60, 39);
        }
    }

    public class RollerRes : EntityResource
    {
        public RollerRes()
        {
            Idle = new Bitmap(@"..\..\..\..\Sprites\Enemy\RollerIdle.png");
            Move = new Bitmap(@"..\..\..\..\Sprites\Enemy\RollerWalk.png");
            IdleSize = new Size(60, 60);
            MoveSize = new Size(60, 60);
        }
    }

    public class SwordsmanRes : AttackingEntityResource
    {
        public SwordsmanRes()
        {
            Idle = new Bitmap(@"..\..\..\..\Sprites\Enemy\SwordsmanIdle.png");
            Move = new Bitmap(@"..\..\..\..\Sprites\Enemy\SwordsmanWalk.png");
            Attack = new Bitmap(@"..\..\..\..\Sprites\Enemy\SwordsmanAttack.png");
            AttackMove = new Bitmap(@"..\..\..\..\Sprites\Enemy\SwordsmanAttack.png");
            IdleSize = new Size(40, 112);
            MoveSize = new Size(40, 112);
            AttackSize = new Size(88, 112);
            AttackMoveSize = new Size(88, 112);
            AttackRange = new Size(40, 56);
        }
    }

    public class ArcherRes : EntityResource
    {
        public ArcherRes()
        {
            Idle = new Bitmap(@"..\..\..\..\Sprites\Enemy\ArcherIdle.png");
            Move = new Bitmap(@"..\..\..\..\Sprites\Enemy\ArcherWalk.png");
            IdleSize = new Size(112, 110);
            MoveSize = new Size(112, 110);
        }
    }

    public class MagicianRes : EntityResource
    {
        public MagicianRes()
        {
            Idle = new Bitmap(@"..\..\..\..\Sprites\Enemy\MagicianIdle.png");
            Move = new Bitmap(@"..\..\..\..\Sprites\Enemy\MagicianIdle.png");
            IdleSize = new Size(64, 112);
            MoveSize = new Size(64, 112);
        }
    }

    public class BigCowRes : EntityResource
    {
        public readonly Size ColliderSize;
        public BigCowRes()
        {
            Idle = new Bitmap(@"..\..\..\..\Sprites\Enemy\BigCowIdle.png");
            Move = new Bitmap(@"..\..\..\..\Sprites\Enemy\BigCowWalk.png");
            ColliderSize = new Size(90, 76);
            IdleSize = new Size(90, 82);
            MoveSize = new Size(90, 82);
        }
    }

    public class SuperMagicianRes : EntityResource
    {
        public SuperMagicianRes()
        {
            Idle = new Bitmap(@"..\..\..\..\Sprites\Enemy\SuperMagicianIdle.png");
            Move = new Bitmap(@"..\..\..\..\Sprites\Enemy\SuperMagicianIdle.png");
            IdleSize = new Size(64, 112);
            MoveSize = new Size(64, 112);
        }
    }

    public class GhostRes : EntityResource
    {
        public GhostRes()
        {
            Idle = new Bitmap(@"..\..\..\..\Sprites\Enemy\GhostIdle.png");
            Move = new Bitmap(@"..\..\..\..\Sprites\Enemy\GhostIdle.png");
            IdleSize = new Size(94, 92);
            MoveSize = new Size(94, 92);
        }
    }

    public class InvisibleManRes : EntityResource
    {
        public InvisibleManRes()
        {
            Idle = new Bitmap(@"..\..\..\..\Sprites\Enemy\InvisibleManIdle.png");
            Move = new Bitmap(@"..\..\..\..\Sprites\Enemy\InvisibleManWalk.png");
            IdleSize = new Size(40, 96);
            MoveSize = new Size(40, 96);
        }
    }

    public class TurretRes : EntityResource
    {
        public TurretRes()
        {
            Idle = new Bitmap(@"..\..\..\..\Sprites\Enemy\TurretIdle.png");
            Move = new Bitmap(@"..\..\..\..\Sprites\Enemy\TurretIdle.png");
            IdleSize = new Size(56, 96);
            MoveSize = new Size(56, 96);
        }
    }

    public class StickerRes : EntityResource
    {
        public StickerRes()
        {
            Idle = new Bitmap(@"..\..\..\..\Sprites\Enemy\StickerIdle.png");
            Move = new Bitmap(@"..\..\..\..\Sprites\Enemy\StickerIdle.png");
            IdleSize = new Size(40, 24);
            MoveSize = new Size(40, 24);
        }
    }

    public class StickerAttackRes : EntityResource
    {
        public StickerAttackRes()
        {
            Idle = new Bitmap(@"..\..\..\..\Sprites\Enemy\StickerAttackIdle.png");
            Move = new Bitmap(@"..\..\..\..\Sprites\Enemy\StickerAttackIdle.png");
            IdleSize = new Size(60, 70);
            MoveSize = new Size(60, 70);
        }
    }
    #endregion

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

    public class PlasmaRes : EntityResource
    {
        public PlasmaRes()
        {
            Idle = new Bitmap(@"..\..\..\..\Sprites\Projectiles\Plasma.png");
            Move = new Bitmap(@"..\..\..\..\Sprites\Projectiles\Plasma.png");
            IdleSize = new Size(25, 25);
            MoveSize = new Size(25, 25);
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
        public readonly Size HPSize100 = new Size(275, 38);
        public readonly Size HPSize = new Size(778, 38);
        public readonly Bitmap Ammo = new Bitmap(@"..\..\..\..\Sprites\UI\ammo.png");
        public readonly Bitmap EternalAmmo = new Bitmap(@"..\..\..\..\Sprites\UI\eternalAmmo.png");
        public readonly Bitmap EternalAmmoReloading = new Bitmap(@"..\..\..\..\Sprites\UI\eternalAmmoReloading.png");
        public readonly Size AmmoSize = new Size(12, 52);
    }

    public class LootResource
    {
        public Bitmap idNotFound;
    }

    public class TreasuresRes : LootResource
    {
        public TreasuresRes()
        {
            idNotFound = new Bitmap(@"..\..\..\..\Sprites\Treasures\id-1.png");
        }
        public readonly Size Size = new Size(75, 70);

        //СТРОГО НАЗЫВАТЬ id*ID* !!!!
        public readonly Bitmap id0 = new Bitmap(@"..\..\..\..\Sprites\Treasures\id0.png");
        public readonly Bitmap id1 = new Bitmap(@"..\..\..\..\Sprites\Treasures\id1.png");
        public readonly Bitmap id2 = new Bitmap(@"..\..\..\..\Sprites\Treasures\id2.png");
        public readonly Bitmap id3 = new Bitmap(@"..\..\..\..\Sprites\Treasures\id3.png");
        public readonly Bitmap id4 = new Bitmap(@"..\..\..\..\Sprites\Treasures\id4.png");
        public readonly Bitmap id5 = new Bitmap(@"..\..\..\..\Sprites\Treasures\id5.png");
        public readonly Bitmap id6 = new Bitmap(@"..\..\..\..\Sprites\Treasures\id6.png");
        public readonly Bitmap id7 = new Bitmap(@"..\..\..\..\Sprites\Treasures\id7.png");
        public readonly Bitmap id8 = new Bitmap(@"..\..\..\..\Sprites\Treasures\id8.png");
        public readonly Bitmap id9 = new Bitmap(@"..\..\..\..\Sprites\Treasures\id9.png");
        public readonly Bitmap id10 = new Bitmap(@"..\..\..\..\Sprites\Treasures\id10.png");
        public readonly Bitmap id11 = new Bitmap(@"..\..\..\..\Sprites\Treasures\id11.png");
        public readonly Bitmap id12 = new Bitmap(@"..\..\..\..\Sprites\Treasures\id12.png");
        public readonly Bitmap id13 = new Bitmap(@"..\..\..\..\Sprites\Treasures\id13.png");
        public readonly Bitmap id14 = new Bitmap(@"..\..\..\..\Sprites\Treasures\id14.png");
    }

    public class LootRes : LootResource
    {
        public LootRes()
        {
            idNotFound = new Bitmap(@"..\..\..\..\Sprites\Loot\NotFound.png");
        }
        public readonly Size Size = new Size(20, 20);

        //СТРОГО НАЗЫВАТЬ id*ID* !!!!
        public readonly Bitmap id0 = new Bitmap(@"..\..\..\..\Sprites\Loot\Heart.png");
        public readonly Bitmap id0alt = new Bitmap(@"..\..\..\..\Sprites\Loot\HeartAngry.png");
        public readonly Bitmap id1 = new Bitmap(@"..\..\..\..\Sprites\Loot\Ammo.png");
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winforms_platformer.Properties;

namespace Winforms_platformer
{
    public static class Res
    {
        public static SystemRes System;
        public static PlayerRes Player;
        #region ENEMIES
        public static DummyRes Dummy;
        public static SlimeRes Slime;
        public static RollerRes Roller;
        public static SuperRollerRes SuperRoller;
        public static SwordsmanRes Swordsman;
        public static ArcherRes Archer;
        public static MagicianRes Magician;
        public static SuperMagicianRes SuperMagician;
        public static BigCowRes BigCow;
        public static GhostRes Ghost;
        public static InvisibleManRes InvisibleMan;
        public static TurretRes Turret;
        public static StickerRes Sticker;
        public static StickerAttackRes StickerAttack;
        public static CloneRes Clone;
        public static ChameleonRes Chameleon;
        #endregion
        public static RoomRes Room;
        public static TreasureRoomRes TreasureRoom;
        public static TreasuresRes Treasures;
        public static ArrowRes Arrow;
        public static PlasmaRes Plasma;
        public static UIRes UI;
        public static LootRes Loot;
        public static BossRes Boss;

        public static void Create()
        {
            System = new SystemRes();
            Player = new PlayerRes();
            Dummy = new DummyRes();
            Slime = new SlimeRes();
            Roller = new RollerRes();
            SuperRoller = new SuperRollerRes();
            Swordsman = new SwordsmanRes();
            Archer = new ArcherRes();
            Magician = new MagicianRes();
            SuperMagician = new SuperMagicianRes();
            BigCow = new BigCowRes();
            Ghost = new GhostRes();
            InvisibleMan = new InvisibleManRes();
            Turret = new TurretRes();
            Sticker = new StickerRes();
            StickerAttack = new StickerAttackRes();
            Clone = new CloneRes();
            Chameleon = new ChameleonRes();
            Room = new RoomRes();
            TreasureRoom = new TreasureRoomRes();
            Treasures = new TreasuresRes();
            Arrow = new ArrowRes();
            Plasma = new PlasmaRes();
            UI = new UIRes();
            Loot = new LootRes();
            Boss = new BossRes();
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

    public class SystemRes
    {
        public readonly Bitmap Death = Resources.death;
        public readonly Bitmap Win = Resources.win;
    }

    public class PlayerRes : AttackingEntityResource
    {
        public PlayerRes()
        {
            Idle = Resources.PlayerIdle;
            Move = Resources.PlayerMove;
            Attack = Resources.PlayerAttack;
            AttackMove = Resources.PlayerAttack;
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
            Idle = Resources.DummyIdle;
            Move = Resources.DummyWalk;
            IdleSize = new Size(40, 96);
            MoveSize = new Size(40, 96);
        }
    }

    public class SlimeRes : EntityResource
    {
        public SlimeRes()
        {
            Idle = Resources.SlimeIdle;
            Move = Resources.SlimeWalk;
            IdleSize = new Size(60, 39);
            MoveSize = new Size(60, 39);
        }
    }

    public class RollerRes : EntityResource
    {
        public RollerRes()
        {
            Idle = Resources.RollerIdle;
            Move = Resources.RollerWalk;
            IdleSize = new Size(60, 60);
            MoveSize = new Size(60, 60);
        }
    }
    
    public class SuperRollerRes : EntityResource
    {
        public SuperRollerRes()
        {
            Idle = Resources.SuperRollerIdle;
            Move = Resources.SuperRollerWalk;
            IdleSize = new Size(60, 60);
            MoveSize = new Size(60, 60);
        }
    }

    public class SwordsmanRes : AttackingEntityResource
    {
        public SwordsmanRes()
        {
            Idle = Resources.SwordsmanIdle;
            Move = Resources.SwordsmanWalk;
            Attack = Resources.SwordsmanAttack;
            AttackMove = Resources.SwordsmanAttack;
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
            Idle = Resources.ArcherIdle;
            Move = Resources.ArcherWalk;
            IdleSize = new Size(112, 110);
            MoveSize = new Size(112, 110);
        }
    }

    public class MagicianRes : EntityResource
    {
        public MagicianRes()
        {
            Idle = Resources.MagicianIdle;
            Move = Resources.MagicianIdle;
            IdleSize = new Size(64, 112);
            MoveSize = new Size(64, 112);
        }
    }

    public class BigCowRes : EntityResource
    {
        public readonly Size ColliderSize;
        public BigCowRes()
        {
            Idle = Resources.BigCowIdle;
            Move = Resources.BigCowWalk;
            ColliderSize = new Size(90, 76);
            IdleSize = new Size(90, 82);
            MoveSize = new Size(90, 82);
        }
    }

    public class SuperMagicianRes : EntityResource
    {
        public SuperMagicianRes()
        {
            Idle = Resources.SuperMagicianIdle;
            Move = Resources.SuperMagicianIdle;
            IdleSize = new Size(64, 112);
            MoveSize = new Size(64, 112);
        }
    }

    public class GhostRes : EntityResource
    {
        public GhostRes()
        {
            Idle = Resources.GhostIdle;
            Move = Resources.GhostIdle;
            IdleSize = new Size(94, 92);
            MoveSize = new Size(94, 92);
        }
    }

    public class InvisibleManRes : EntityResource
    {
        public InvisibleManRes()
        {
            Idle = Resources.InvisibleManIdle;
            Move = Resources.InvisibleManWalk;
            IdleSize = new Size(40, 96);
            MoveSize = new Size(40, 96);
        }
    }

    public class TurretRes : EntityResource
    {
        public TurretRes()
        {
            Idle = Resources.TurretIdle;
            Move = Resources.TurretIdle;
            IdleSize = new Size(56, 96);
            MoveSize = new Size(56, 96);
        }
    }

    public class StickerRes : EntityResource
    {
        public StickerRes()
        {
            Idle = Resources.StickerIdle;
            Move = Resources.StickerIdle;
            IdleSize = new Size(40, 24);
            MoveSize = new Size(40, 24);
        }
    }

    public class StickerAttackRes : EntityResource
    {
        public StickerAttackRes()
        {
            Idle = Resources.StickerAttackIdle;
            Move = Resources.StickerAttackIdle;
            IdleSize = new Size(60, 70);
            MoveSize = new Size(60, 70);
        }
    }

    public class CloneRes : EntityResource
    {
        public CloneRes()
        {
            Idle = Resources.CloneIdle;
            Move = Resources.CloneWalk;
            IdleSize = new Size(27, 54);
            MoveSize = new Size(27, 54);
        }
    }

    public class ChameleonRes : EntityResource
    {
        public ChameleonRes()
        {
            Idle = Resources.DummyIdle;
            Move = Resources.DummyWalk;
            IdleSize = new Size(40, 96);
            MoveSize = new Size(40, 96);
        }
    }
    #endregion

    #region ROOMS
    public class RoomRes
    {
        public Bitmap Wall { get; protected set; }
        public Bitmap Ground { get; protected set; }
        public Bitmap Platform { get; protected set; }
        public readonly Size PlatformSize = new Size(800, 27);

        public RoomRes()
        {
            Wall = Resources.Wall;
            Ground = Resources.Ground;
            Platform = Resources.Platform;
        }
    }

    public class TreasureRoomRes : RoomRes
    {
        public TreasureRoomRes()
        {
            Wall = Resources.TreasureWall;
            Ground = Resources.TreasureGround;
            Platform = Resources.Platform;
        }
    }
    #endregion

    #region PROJECTILES
    public class PlasmaRes : EntityResource
    {
        public PlasmaRes()
        {
            Idle = Resources.Plasma;
            Move = Resources.Plasma;
            IdleSize = new Size(25, 25);
            MoveSize = new Size(25, 25);
        }
    }

    public class ArrowRes : EntityResource
    {
        public ArrowRes()
        {
            Idle = Resources.Arrow;
            Move = Resources.Arrow;
            IdleSize = new Size(50, 10);
            MoveSize = new Size(50, 10);
        }
    }
    #endregion

    #region LOOT
    public class LootResource
    {
        public Bitmap idNotFound;
    }

    public class TreasuresRes : LootResource
    {
        public TreasuresRes()
        {
            idNotFound = Resources.id_1;
        }
        public readonly Size Size = new Size(75, 70);

        //СТРОГО НАЗЫВАТЬ id*ID* !!!!
        public readonly Bitmap id0 = Resources.id0;
        public readonly Bitmap id1 = Resources.id1;
        public readonly Bitmap id2 = Resources.id2;
        public readonly Bitmap id3 = Resources.id3;
        public readonly Bitmap id4 = Resources.id4;
        public readonly Bitmap id5 = Resources.id5;
        public readonly Bitmap id6 = Resources.id6;
        public readonly Bitmap id7 = Resources.id7;
        public readonly Bitmap id8 = Resources.id8;
        public readonly Bitmap id9 = Resources.id9;
        public readonly Bitmap id10 = Resources.id10;
        public readonly Bitmap id11 = Resources.id11;
        public readonly Bitmap id12 = Resources.id12;
        public readonly Bitmap id13 = Resources.id13;
        public readonly Bitmap id14 = Resources.id14;
        public readonly Bitmap id15 = Resources.id15;
        public readonly Bitmap id16 = Resources.id16;
        public readonly Bitmap id17 = Resources.id17;
        public readonly Bitmap id18 = Resources.id18;
        public readonly Bitmap id19 = Resources.id19;
        public readonly Bitmap id20 = Resources.id20;
    }

    public class LootRes : LootResource
    {
        public LootRes()
        {
            idNotFound = Resources.NotFound;
        }
        public readonly Size Size = new Size(20, 20);

        //СТРОГО НАЗЫВАТЬ id*ID* !!!!
        public readonly Bitmap id0 = Resources.Heart;
        public readonly Bitmap id0alt = Resources.HeartAngry;
        public readonly Bitmap id1 = Resources.Ammo;
    }
    #endregion

    public class UIRes
    {
        public readonly Bitmap HPBar = Resources.hp;
        public readonly Bitmap HPFrame = Resources.frame;
        public readonly Bitmap BossHPBar = Resources.bossHP;
        public readonly Bitmap BossHPFrame = Resources.bossFrame;
        public readonly Size HPSize = new Size(778, 38);
        public readonly Size HPSize100 = new Size(275, 38);
        public readonly Size BossHPSize = new Size(392, 38);
        public readonly Bitmap Ammo = Resources.ammoUI;
        public readonly Bitmap EternalAmmo = Resources.eternalAmmo;
        public readonly Bitmap EternalAmmoReloading = Resources.eternalAmmoReloading;
        public readonly Bitmap PlasmaAmmo = Resources.plasmaAmmo;
        public readonly Bitmap PlasmaAmmoReloading = Resources.plasmaAmmoReloading;
        public readonly Bitmap GhostForm = Resources.ghostForm;
        public readonly Bitmap GhostFormReloading = Resources.ghostFormReloading;
        public readonly Size AmmoSize = new Size(12, 52);
        public readonly Size EternalAmmoSize = new Size(12, 52);
        public readonly Size PlasmaAmmoSize = new Size(25, 25);
        public readonly Size GhostFormSize = new Size(46, 47);
    }


    public class BossRes
    {
        public Size LeftZoneSize = new Size(200, 600);
        public Size CenterZoneSize = new Size(400, 600);
        public Size RightZoneSize = new Size(200, 600);
        public Size PalmSize = new Size(200, 159);
        public Size FistSize = new Size(155, 159);
        public Size BodySize = new Size(491, 482);
        public Bitmap LeftPalm = Resources.LeftPalm;
        public Bitmap LeftFist = Resources.LeftFist;
        public Bitmap RightPalm = Resources.RightPalm;
        public Bitmap RightFist = Resources.RightFist;
        public Bitmap Body = Resources.Body;
        public Bitmap Secret = Resources.SuperSecret;
        public Bitmap Summon = Resources.Summon;
    }
}

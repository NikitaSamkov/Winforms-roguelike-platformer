using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winforms_platformer
{
    public enum Direction
    {
        Right = 0,
        Left = 1
    }

    public enum Status
    {
        Idle = 0,
        Move = 1,
        Attack = 2,
        AttackMove = 3
    }

    public enum RoomType
    {
        StartingRoom,
        RegularRoom,
        TreasureRoom,
        Shop,
        BossRoom
    }

    public enum ProjectileType
    {
        Ally,
        Enemy
    }

    public enum LootType
    {
        Heart,
        Ammo,
        Treasure
    }

    public enum EnemyType
    {
        Dummy,
        Slime,
        Roller,
        Swordsman,
        Archer,
        Magician,
        SuperMagician,
        BigCow,
        Ghost,
        InvisibleMan,
        Turret,
        Sticker,
        Clone,
        SuperRoller
    }
}

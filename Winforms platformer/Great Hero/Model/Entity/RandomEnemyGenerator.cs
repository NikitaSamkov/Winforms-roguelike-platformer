using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winforms_platformer.Model
{
    public static class RandomEnemyGenerator
    {
        public static List<EnemyType> enemyTypes;
        private static Random random;

        public static void Start()
        {
            random = new Random(Game.Map.seed);
            enemyTypes = new List<EnemyType>();

            enemyTypes.Add(EnemyType.Slime);
            enemyTypes.Add(EnemyType.Roller);
            enemyTypes.Add(EnemyType.SuperRoller);
            enemyTypes.Add(EnemyType.Swordsman);
            enemyTypes.Add(EnemyType.Archer);
            enemyTypes.Add(EnemyType.Magician);
            enemyTypes.Add(EnemyType.SuperMagician);
            enemyTypes.Add(EnemyType.Ghost);
            enemyTypes.Add(EnemyType.InvisibleMan);
            enemyTypes.Add(EnemyType.Turret);
            enemyTypes.Add(EnemyType.Sticker);
            enemyTypes.Add(EnemyType.Clone);
            enemyTypes.Add(EnemyType.Chameleon);
        }

        public static void AddToGenerator(EnemyType type)
        {
            if (!enemyTypes.Contains(type))
                enemyTypes.Add(type);
        }

        public static void RemoveFromGenerator(EnemyType type)
        {
            enemyTypes.Remove(type);
        }

        public static Enemy GetRandomEnemy(int x, int y)
        {
            var type = enemyTypes[random.Next(enemyTypes.Count)];
            if (Game.Player.Treasures.Contains(TreasurePool.GetTreasureByID(10)) && type != EnemyType.BigCow)
                type = EnemyType.Roller;
            return GetEnemy(x, y, type);
        }

        public static Enemy GetEnemy(int x, int y, EnemyType type)
        {
            Func<Room> currentRoom = Game.Map.CurrentRoom;
            switch (type)
            {
                case EnemyType.Dummy:
                    return new Enemy(x, y, new Collider(Res.Dummy.IdleSize), currentRoom);
                case EnemyType.Slime:
                    return new Slime(x, y, new Collider(Res.Slime.IdleSize), currentRoom);
                case EnemyType.Roller:
                    return new Roller(x, y, new Collider(Res.Roller.IdleSize), currentRoom);
                case EnemyType.SuperRoller:
                    return new SuperRoller(x, y, new Collider(Res.SuperRoller.IdleSize), currentRoom);
                case EnemyType.Swordsman:
                    return new Swordsman(x, y, new Collider(Res.Swordsman.IdleSize, 0, 0,
                new Collider(Res.Swordsman.AttackRange, -10, Res.Swordsman.Idle.Height / 8)),
                currentRoom);
                case EnemyType.Archer:
                    return new Archer(x, y, new Collider(Res.Archer.IdleSize), currentRoom);
                case EnemyType.Magician:
                    return new Magician(x, y, new Collider(Res.Magician.IdleSize), currentRoom);
                case EnemyType.SuperMagician:
                    return new SuperMagician(x, y, new Collider(Res.SuperMagician.IdleSize), currentRoom);
                case EnemyType.BigCow:
                    return new BigCow(x, y, new Collider(Res.BigCow.ColliderSize, 0, 6), currentRoom);
                case EnemyType.Ghost:
                    return new Ghost(x, y, new Collider(Res.Ghost.IdleSize), currentRoom);
                case EnemyType.InvisibleMan:
                    return new InvisibleMan(x, y, new Collider(Res.InvisibleMan.IdleSize), currentRoom);
                case EnemyType.Turret:
                    return new Turret(x, y, new Collider(Res.Turret.IdleSize), currentRoom);
                case EnemyType.Sticker:
                    return new Sticker(x, y, new Collider(Res.Sticker.IdleSize), currentRoom);
                case EnemyType.Clone:
                    return new Clone(x, y, new Collider(Res.Clone.IdleSize), currentRoom);
                case EnemyType.Chameleon:
                    return new Chameleon(x, y, new Collider(Res.Dummy.IdleSize), currentRoom);
            }
            return new Enemy(x, y, new Collider(Res.Dummy.IdleSize), currentRoom);
        }

        public static EnemyType GetRandomEnemyType() => enemyTypes[random.Next(enemyTypes.Count)];
    }
}

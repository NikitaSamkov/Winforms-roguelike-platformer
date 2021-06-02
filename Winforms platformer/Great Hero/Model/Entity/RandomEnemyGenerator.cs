using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winforms_platformer.Model
{
    public static class RandomEnemyGenerator
    {
        private static List<EnemyType> enemyTypes = new List<EnemyType>();
        private static Random random = new Random();

        public static void Start()
        {
            random = Game.Map.Random;

            enemyTypes.Add(EnemyType.Slime);
            enemyTypes.Add(EnemyType.Roller);
            enemyTypes.Add(EnemyType.Swordsman);
            enemyTypes.Add(EnemyType.Archer);
            enemyTypes.Add(EnemyType.Magician);
            enemyTypes.Add(EnemyType.SuperMagician);
            enemyTypes.Add(EnemyType.Ghost);
            enemyTypes.Add(EnemyType.InvisibleMan);
            enemyTypes.Add(EnemyType.Turret);
            enemyTypes.Add(EnemyType.Sticker);
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
            if (Game.Player.treasures.Contains(TreasurePool.GetTreasureByID(10)) && type != EnemyType.BigCow)
                type = EnemyType.Roller;
            Func<Room> currentRoom = Game.Map.CurrentRoom;
            var player = Game.Player;
            switch (type)
            {
                case EnemyType.Dummy:
                    return new Enemy(x, y, new Collider(Resources.Dummy.IdleSize), currentRoom, player);
                case EnemyType.Slime:
                    return new Slime(x, y, new Collider(Resources.Slime.IdleSize), currentRoom, player);
                case EnemyType.Roller:
                    return new Roller(x, y, new Collider(Resources.Roller.IdleSize), currentRoom, player);
                case EnemyType.Swordsman:
                    return new Swordsman(x, y, new Collider(Resources.Swordsman.IdleSize, 0, 0,
                new Collider(Resources.Swordsman.AttackRange, -10, Resources.Swordsman.Idle.Height / 8)), 
                currentRoom, player);
                case EnemyType.Archer:
                    return new Archer(x, y, new Collider(Resources.Archer.IdleSize), currentRoom, player);
                case EnemyType.Magician:
                    return new Magician(x, y, new Collider(Resources.Magician.IdleSize), currentRoom, player);
                case EnemyType.SuperMagician:
                    return new SuperMagician(x, y, new Collider(Resources.SuperMagician.IdleSize), currentRoom, player);
                case EnemyType.BigCow:
                    return new BigCow(x, y, new Collider(Resources.BigCow.ColliderSize, 0, 6), currentRoom, player);
                case EnemyType.Ghost:
                    return new Ghost(x, y, new Collider(Resources.Ghost.IdleSize), currentRoom, player);
                case EnemyType.InvisibleMan:
                    return new InvisibleMan(x, y, new Collider(Resources.InvisibleMan.IdleSize), currentRoom, player);
                case EnemyType.Turret:
                    return new Turret(x, y, new Collider(Resources.Turret.IdleSize), currentRoom, player);
                case EnemyType.Sticker:
                    return new Sticker(x, y, new Collider(Resources.Sticker.IdleSize), currentRoom, player);
            }
            return new Enemy(x, y, new Collider(Resources.Dummy.IdleSize),currentRoom, player);
        }
    }
}

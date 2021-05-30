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
        }

        public static void AddToGenerator(EnemyType type)
        {
            if (!enemyTypes.Contains(type))
                enemyTypes.Add(type);
        }

        public static Enemy GetRandomEnemy(int x, int y)
        {
            var type = enemyTypes[random.Next(enemyTypes.Count)];
            switch (type)
            {
                case EnemyType.Dummy:
                    return new Enemy(x, y, new Collider(Resources.Dummy.IdleSize), Game.Map.CurrentRoom, Game.Player);
                case EnemyType.Slime:
                    return new Slime(x, y, new Collider(Resources.Slime.IdleSize), Game.Map.CurrentRoom, Game.Player);
                case EnemyType.Roller:
                    return new Roller(x, y, new Collider(Resources.Roller.IdleSize), Game.Map.CurrentRoom, Game.Player);
                case EnemyType.Swordsman:
                    return new Swordsman(x, y, new Collider(Resources.Swordsman.IdleSize, 0, 0,
                new Collider(Resources.Swordsman.AttackRange, -10, Resources.Swordsman.Idle.Height / 8)), 
                Game.Map.CurrentRoom, Game.Player);
                case EnemyType.Archer:
                    return new Archer(x, y, new Collider(Resources.Archer.IdleSize), Game.Map.CurrentRoom, Game.Player);
                case EnemyType.Magician:
                    return new Magician(x, y, new Collider(Resources.Magician.IdleSize), Game.Map.CurrentRoom, Game.Player);
                case EnemyType.SuperMagician:
                    return new SuperMagician(x, y, new Collider(Resources.SuperMagician.IdleSize), Game.Map.CurrentRoom, Game.Player);
                case EnemyType.BigCow:
                    return new BigCow(x, y, new Collider(Resources.BigCow.ColliderSize, 0, 6), Game.Map.CurrentRoom, Game.Player);
            }
            return new Enemy(x, y, new Collider(Resources.Dummy.IdleSize), Game.Map.CurrentRoom, Game.Player);
        }
    }
}

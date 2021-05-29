﻿using System;
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
            }
            return new Enemy(x, y, new Collider(Resources.Dummy.IdleSize), Game.Map.CurrentRoom, Game.Player);
        }
    }
}

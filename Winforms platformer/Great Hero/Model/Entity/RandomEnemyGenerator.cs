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
            enemyTypes.Add(EnemyType.Dummy);
        }

        public static Enemy GetRandomEnemy(int x, int y)
        {
            var type = random.Next(1);
            switch ((EnemyType)type)
            {
                case EnemyType.Dummy:
                    return new Enemy(x, y, new Collider(Resources.Dummy.IdleSize), Game.Map.CurrentRoom, Game.Player);
            }
            return new Enemy(x, y, new Collider(Resources.Dummy.IdleSize), Game.Map.CurrentRoom, Game.Player);
        }
    }
}

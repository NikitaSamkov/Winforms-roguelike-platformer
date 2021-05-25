using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winforms_platformer
{
    public class Room
    {
        private Player player;
        public int gForce { get; private set; }
        public RoomType type { get; }
        public readonly int groundLevel;
        public readonly List<Platform> platforms;
        public List<Enemy> enemyList = new List<Enemy>();
        public List<Projectile> ProjectilesList = new List<Projectile>();
        public List<TreasureItem> TreasuresList = new List<TreasureItem>();

        public Room(RoomType type, List<Platform> platforms, Player player, int gravitationForce = 7, int groundLevel = 486)
        {
            this.platforms = platforms;
            gForce = gravitationForce;
            this.groundLevel = groundLevel;
            this.type = type;
            this.player = player;
        }

        public int GetYSpeed(int x, int y, int width, int speed)
        {
            var newY = y + speed + gForce;
            foreach (var platform in platforms)
                if (platform.level >= y && platform.level < newY &&
                    platform.leftBorder < x + width && platform.rightBorder > x)
                    newY = platform.level;
            if (newY > groundLevel)
                newY = groundLevel;
            return newY - y;
        }

        public bool OnTheSurface(int x, int y, int width)
        {
            if (y == groundLevel)
                return true;
            foreach (var platform in platforms)
                if (platform.level == y &&
                    platform.leftBorder < x + width && platform.rightBorder > x)
                    return true;
            return false;
        }

        public List<Enemy> GetIntersectedEnemies(Collider collider, int colliderX, int colliderY)
        {
            var result = new List<Enemy>();
            foreach (var enemy in enemyList)
                if (new Rectangle(new Point(colliderX, colliderY), collider.field)
                    .IntersectsWith(
                    new Rectangle(new Point(enemy.collider.x + enemy.x, enemy.collider.y + enemy.y),
                    enemy.collider.field)))
                    result.Add(enemy);
            return result;
        }
        public List<Entity> GetIntersectedEntities(Entity entity)
        {
            var result = new List<Entity>();
            foreach (var enemy in enemyList)
                if (entity.IntersectsWithBody(enemy))
                    result.Add(enemy);
            if (entity.IntersectsWithBody(player))
                result.Add(player);
            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winforms_platformer.Model;

namespace Winforms_platformer
{
    public class Room
    {
        private Player player;
        public int gForce { get; set; }
        public RoomType Type { get; }
        public readonly int GroundLevel;
        public readonly List<Platform> Platforms;
        public List<Enemy> EnemyList = new List<Enemy>();
        public List<Enemy> AdditionalEnemies = new List<Enemy>();
        public List<Projectile> ProjectilesList = new List<Projectile>();
        public List<Loot> LootList;
        public List<Point> EnemySpots;

        public Room(RoomType type, List<Platform> platforms = null, List<Loot> lootList = null,
            List<Point> enemies = null, int gravitationForce = 7, int groundLevel = 486)
        {
            Platforms = platforms;
            if (Platforms == null)
                Platforms = new List<Platform>();
            LootList = lootList;
            if (LootList == null)
                LootList = new List<Loot>();
            EnemySpots = enemies;
            if (EnemySpots == null)
                EnemySpots = new List<Point>();
            if (Type == RoomType.BossRoom)
                EnemyList.Add(Game.Boss);
            gForce = gravitationForce;
            GroundLevel = groundLevel;
            Type = type;
            player = Game.Player;
        }

        public int GetYSpeed(int x, int y, int width, int speed)
        {
            var newY = y + speed + gForce;
            foreach (var platform in Platforms)
                if (platform.level >= y && platform.level < newY &&
                    platform.leftBorder < x + width && platform.rightBorder > x)
                    newY = platform.level;
            if (newY > GroundLevel)
                newY = GroundLevel;
            return newY - y;
        }

        public bool OnTheSurface(int x, int y, int width)
        {
            if (y == GroundLevel)
                return true;
            foreach (var platform in Platforms)
                if (platform.level == y &&
                    platform.leftBorder < x + width && platform.rightBorder > x)
                    return true;
            return false;
        }

        public List<Entity> GetIntersectedEntities(Collider collider, int colliderX, int colliderY)
        {
            var result = new List<Entity>();
            foreach (var enemy in EnemyList)
                if (new Rectangle(new Point(colliderX, colliderY), collider.field)
                    .IntersectsWith(
                    new Rectangle(new Point(enemy.collider.x + enemy.x, enemy.collider.y + enemy.y),
                    enemy.collider.field)))
                    result.Add(enemy);
            if (new Rectangle(new Point(colliderX, colliderY), collider.field)
                    .IntersectsWith(
                    new Rectangle(new Point(player.collider.x + player.x, player.collider.y + player.y),
                    player.collider.field)))
                result.Add(player);
            if (Type == RoomType.BossRoom && new Rectangle(new Point(colliderX, colliderY), collider.field)
                    .IntersectsWith(
                    new Rectangle(new Point(Game.Boss.collider.x + Game.Boss.x, Game.Boss.collider.y + Game.Boss.y),
                    Game.Boss.collider.field)))
                result.Add(Game.Boss);
            return result;
        }

        public List<Entity> GetIntersectedEntities(Entity entity)
        {
            var result = new List<Entity>();
            foreach (var enemy in EnemyList)
                if (entity.IntersectsWithBody(enemy))
                    result.Add(enemy);
            if (entity.IntersectsWithBody(player))
                result.Add(player);
            return result;
        }
    }
}

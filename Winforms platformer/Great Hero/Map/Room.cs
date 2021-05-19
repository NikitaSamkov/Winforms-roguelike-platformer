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
        public int gForce { get; private set; }
        public readonly int groundLevel;
        public readonly List<Platform> platforms;
        public List<EntityRender> enemyList = new List<EntityRender>();
        public List<EntityRender> allyProjectilesList = new List<EntityRender>();
        public List<EntityRender> enemyProjectilesList = new List<EntityRender>();

        public Room(List<Platform> platforms, int gravitationForce = 7, int groundLevel = 486)
        {
            this.platforms = platforms;
            gForce = gravitationForce;
            this.groundLevel = groundLevel;
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

        public List<EntityRender> GetIntersectedEntities(Collider collider, int colliderX, int colliderY)
        {
            var result = new List<EntityRender>();
            foreach (var enemy in enemyList)
                if (new Rectangle(new Point(colliderX, colliderY), collider.field)
                    .IntersectsWith(
                    new Rectangle(new Point(enemy.entity.collider.x + enemy.entity.x, enemy.entity.collider.y + enemy.entity.y),
                    enemy.entity.collider.field)))
                    result.Add(enemy);
            return result;
        }
        public List<EntityRender> GetIntersectedEntities(Entity entity)
        {
            var result = new List<EntityRender>();
            foreach (var enemy in enemyList)
                if (entity.IntersectsWithBody(enemy.entity))
                    result.Add(enemy);
            return result;
        }
    }
}

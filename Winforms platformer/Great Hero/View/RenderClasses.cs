using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winforms_platformer.View
{
    public class EntityRender : IRenderable
    {
        private Entity entity;
        private EntityResource Resource;
        private Bitmap CurrentSheet;
        private int frame => (currentFrame / framePause) % maxFrames;
        private int maxFrames;
        private int currentFrame;
        private Size currentFrameSize;
        private int framePause;

        public EntityRender(Entity entity, EntityResource resource, int framePause = 1)
        {
            this.entity = entity;
            Resource = resource;
            this.framePause = framePause;
        }

        protected void GetCurrentSheet()
        {
            switch (entity.status)
            {
                case Status.Idle:
                    if (CurrentSheet != Resource.Idle)
                    {
                        CurrentSheet = Resource.Idle;
                        maxFrames = Resource.Idle.Width / Resource.IdleSize.Width;
                        currentFrameSize = Resource.IdleSize;
                        currentFrame = 0;
                    }
                    break;
                case Status.Move:
                    if (CurrentSheet != Resource.Move)
                    {
                        CurrentSheet = Resource.Move;
                        maxFrames = Resource.Move.Width / Resource.MoveSize.Width;
                        currentFrameSize = Resource.MoveSize;
                        currentFrame = 0;
                    }
                    break;
                case Status.Attack:
                    if (Resource is AttackingEntityResource attackingResource && CurrentSheet != attackingResource.Attack)
                    {
                        if (CurrentSheet != attackingResource.AttackMove)
                            currentFrame = 0;
                        CurrentSheet = attackingResource.Attack;
                        currentFrameSize = attackingResource.AttackSize;
                        maxFrames = attackingResource.Attack.Width / attackingResource.AttackSize.Width;
                    }
                    break;
                case Status.AttackMove:
                    if (Resource is AttackingEntityResource attackingMoveResource && CurrentSheet != attackingMoveResource.AttackMove)
                    {
                        if (CurrentSheet != attackingMoveResource.Attack)
                            currentFrame = 0;
                        CurrentSheet = attackingMoveResource.AttackMove;
                        currentFrameSize = attackingMoveResource.AttackMoveSize;
                        maxFrames = attackingMoveResource.AttackMove.Width / attackingMoveResource.AttackMoveSize.Width;
                    }
                    break;
            }
        }

        public void Paint(Graphics g)
        {
            GetCurrentSheet();
            currentFrame++;
            g.DrawImage(CurrentSheet, ((int)entity.direction == 0) ? entity.x :
                    entity.x + entity.collider.field.Width - currentFrameSize.Width,
                entity.y, new Rectangle(currentFrameSize.Width * frame,
                    currentFrameSize.Height * (int)entity.direction,
                    currentFrameSize.Width,
                    currentFrameSize.Height),
                    GraphicsUnit.Pixel);
        }
    }

    public class EnemysRender : IRenderable
    {
        private List<Enemy> enemies;

        public EnemysRender(List<Enemy> enemies)
        {
            this.enemies = enemies;
        }

        public void Paint(Graphics g)
        {
            foreach (var enemy in enemies)
            {
                var resources = Resources.Dummy;
                //вставить сюда if (enemy is CustomClass) resources = Resources.CustomClass
                var render = new EntityRender(enemy, resources);
                render.Paint(g);
            }
        }
    }

    public class ProjectilesRender : IRenderable
    {
        private List<Projectile> projectiles;
        public ProjectilesRender(List<Projectile> projectiles)
        {
            this.projectiles = projectiles;
        }

        public void Paint(Graphics g)
        {
            foreach (var projectile in projectiles)
            {
                var resources = Resources.Arrow;
                //вставить сюда if (projectile is CustomClass) resources = Resources.CustomClass
                var render = new EntityRender(projectile, resources);
                render.Paint(g);
            }
        }
    }

    public class RoomRender : IRenderable
    {
        private Room room;
        private RoomRes Resource;

        public RoomRender(Room room)
        {
            this.room = room;
            Resource = Resources.Room;
        }
        public void ChangeRoom(Room newRoom) => room = newRoom;

        public void Paint(Graphics g)
        {
            g.DrawImage(Resource.Wall, 0, 0, Resource.Wall.Width, Resource.Wall.Height);
            g.DrawImage(Resource.Ground,
                Resource.Wall.Width - Resource.Ground.Width,
                room.groundLevel,
                Resource.Ground.Width, Resource.Ground.Height);
            foreach (var platform in room.platforms)
                g.DrawLine(new Pen(Color.Red, 5), platform.leftBorder, platform.level, platform.rightBorder, platform.level);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winforms_platformer.Model;

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

        public void StopAttackingIfNeeded()
        {
            if (Resource is AttackingEntityResource attackingResource && ((currentFrame + 1) / framePause) == maxFrames)
            {
                if (CurrentSheet == attackingResource.Attack)
                {
                    Game.Player.status = Status.Idle;
                    CurrentSheet = Resource.Idle;
                    maxFrames = Resource.Idle.Width / Resource.IdleSize.Width;
                    currentFrameSize = Resource.IdleSize;
                    currentFrame = 0;
                }
                else if (CurrentSheet == attackingResource.AttackMove)
                {
                    Game.Player.status = Status.Move;
                    CurrentSheet = Resource.Move;
                    maxFrames = Resource.Move.Width / Resource.MoveSize.Width;
                    currentFrameSize = Resource.MoveSize;
                    currentFrame = 0;
                }
            }
        }

        public void Paint(Graphics g)
        {

            GetCurrentSheet();
            StopAttackingIfNeeded();
            currentFrame++;
            g.DrawImage(CurrentSheet, ((int)entity.direction == 0) ? entity.x :
                    entity.x + entity.collider.field.Width - currentFrameSize.Width,
                entity.y, new Rectangle(currentFrameSize.Width * frame,
                    currentFrameSize.Height * (int)entity.direction,
                    currentFrameSize.Width,
                    currentFrameSize.Height),
                    GraphicsUnit.Pixel);
            if (Game.DeveloperToolsON)
                g.DrawRectangle(new Pen(Color.Green), entity.x, entity.y, entity.collider.field.Width,
                        entity.collider.field.Height);
        }
    }

    public class EnemysRender : IRenderable
    {
        private Func<Room> CurrentRoom;

        public EnemysRender(Func<Room> CurrentRoom)
        {
            this.CurrentRoom = CurrentRoom;
        }

        public void Paint(Graphics g)
        {
            foreach (var enemy in CurrentRoom().enemyList)
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
        private Func<Room> CurrentRoom;

        public ProjectilesRender(Func<Room> CurrentRoom)
        {
            this.CurrentRoom = CurrentRoom;
        }

        public void Paint(Graphics g)
        {
            foreach (var projectile in CurrentRoom().ProjectilesList)
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
        private Func<Room> CurrentRoom;
        private RoomRes Resource;

        public RoomRender(Func<Room> CurrentRoom)
        {
            this.CurrentRoom = CurrentRoom;
            Resource = Resources.Room;
        }

        public void Paint(Graphics g)
        {
            g.DrawImage(Resource.Wall, 0, 0, Game.WindowWidth, Game.WindowHeight);
            g.DrawImage(Resource.Ground,
                Resource.Wall.Width - Resource.Ground.Width,
                CurrentRoom().groundLevel,
                Game.WindowWidth, Resource.Ground.Height);
            foreach (var platform in CurrentRoom().platforms)
                g.DrawLine(new Pen(Color.Red, 5), platform.leftBorder, platform.level, platform.rightBorder, platform.level);
        }
    }

    public class TreasuresRender : IRenderable
    {
        private Func<Room> CurrentRoom;

        public TreasuresRender(Func<Room> CurrentRoom)
        {
            this.CurrentRoom = CurrentRoom;
        }

        public void Paint(Graphics g)
        {
            foreach (var treasure in CurrentRoom().TreasuresList)
            {
                g.DrawImage(GetSprite(treasure.ID), treasure.x, treasure.y);
                if (Game.DeveloperToolsON)
                    g.DrawRectangle(new Pen(Color.Green), treasure.x, treasure.y, treasure.collider.field.Width,
                        treasure.collider.field.Height);
            }
        }

        public Bitmap GetSprite(int treasureID)
        {
            foreach (var field in Resources.Treasures.GetType().GetFields())
                if (field.Name == "id" + treasureID)
                    return (Bitmap)field.GetValue(Resources.Treasures);
            return Resources.Treasures.idNOtFound;
        }
    }
}

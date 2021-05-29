﻿using System;
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
        public Entity Entity;
        private EntityResource Resource;
        private Bitmap CurrentSheet;
        private int frame => (currentFrame / framePause) % maxFrames;
        private int maxFrames;
        private int currentFrame;
        private Size currentFrameSize;
        private int framePause;

        public EntityRender(Entity entity, EntityResource resource, int framePause = 1)
        {
            this.Entity = entity;
            Resource = resource;
            this.framePause = framePause;
        }

        protected void GetCurrentSheet()
        {
            switch (Entity.status)
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
            g.DrawImage(CurrentSheet, ((int)Entity.direction == 0) ? Entity.x :
                    Entity.x + Entity.collider.field.Width - currentFrameSize.Width,
                Entity.y, new Rectangle(currentFrameSize.Width * frame,
                    currentFrameSize.Height * (int)Entity.direction,
                    currentFrameSize.Width,
                    currentFrameSize.Height),
                    GraphicsUnit.Pixel);
            if (Game.DeveloperToolsON)
                g.DrawRectangle(new Pen(Color.Green), Entity.x, Entity.y, Entity.collider.field.Width,
                        Entity.collider.field.Height);
        }
    }

    public class EnemiesRender : IRenderable
    {
        private Func<Room> CurrentRoom;
        private List<EntityRender> enemies = new List<EntityRender>();

        public EnemiesRender(Func<Room> CurrentRoom)
        {
            this.CurrentRoom = CurrentRoom;
        }

        public void Paint(Graphics g)
        {
            foreach (var enemy in CurrentRoom().EnemyList)
            {
                var render = enemies.Where(e => e.Entity == (Entity)enemy).FirstOrDefault();
                if (render == null)
                {
                    EntityResource resources = Resources.Dummy;
                    //вставить сюда if (enemy is CustomClass) resources = Resources.CustomClass
                    if (enemy is Slime)
                        resources = Resources.Slime;
                    render = new EntityRender(enemy, resources, 3);
                    enemies.Add(render);
                }
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
        }

        public void GetResource()
        {
            switch (CurrentRoom().Type)
            {
                case RoomType.TreasureRoom:
                    Resource = Resources.TreasureRoom;
                    break;
                default:
                    Resource = Resources.Room;
                    break;
            }
        }

        public void Paint(Graphics g)
        {
            GetResource();
            g.DrawImage(Resource.Wall, 0, 0, Game.WindowSize.Width, Game.WindowSize.Height);
            foreach (var platform in CurrentRoom().Platforms)
                g.DrawLine(new Pen(Color.Red, 5), platform.leftBorder, platform.level, platform.rightBorder, platform.level);
            g.DrawImage(Resource.Ground,
                Resource.Wall.Width - Resource.Ground.Width,
                CurrentRoom().GroundLevel,
                Game.WindowSize.Width, Resource.Ground.Height);
        }
    }

    public class LootRender : IRenderable
    {
        private Func<Room> CurrentRoom;

        public LootRender(Func<Room> CurrentRoom)
        {
            this.CurrentRoom = CurrentRoom;
        }

        public void Paint(Graphics g)
        {
            foreach (var loot in CurrentRoom().LootList)
            {
                var resource = (loot is TreasureItem) ? (LootResource)Resources.Treasures : (LootResource)Resources.Loot;
                g.DrawImage(GetSprite(loot.ID, resource), loot.x, loot.y);
                if (Game.DeveloperToolsON)
                    g.DrawRectangle(new Pen(Color.Green), loot.x, loot.y, loot.collider.field.Width,
                        loot.collider.field.Height);
            }

        }

        public Bitmap GetSprite(int ID, LootResource resource)
        {
            foreach (var field in resource.GetType().GetFields())
                if (field.Name == "id" + ID)
                    return (Bitmap)field.GetValue(resource);
            return resource.idNotFound;
        }
    }

    public class UIRender : IRenderable
    {
        public void Paint(Graphics g)
        {
            #region HP
            g.DrawImage(Resources.UI.HPBar, 50, 50,
                new Rectangle(0, 0, Resources.UI.HPSize.Width * Game.Player.HP / 100, Resources.UI.HPSize.Width),
                GraphicsUnit.Pixel);
            g.DrawImage(Resources.UI.HPFrame, 50, 50, Resources.UI.HPSize.Width, Resources.UI.HPSize.Height);
            #endregion
            #region Ammo
            for (var i = 0; i < Game.Player.Ammo; i++)
            {
                g.DrawImage(Resources.UI.Ammo, i * 15 + 50, 100, Resources.UI.AmmoSize.Width, Resources.UI.AmmoSize.Height);
            }
            #endregion
            #region Treasures
            var column = 0;
            var row = 0;
            foreach (var treasure in Game.Player.treasures)
            {
                g.DrawImage(GetTreasureSprite(treasure.ID), Game.WindowSize.Width - 150 + 50 * column, 50 + 50 * row,
                    50, (float)(Resources.Treasures.Size.Height / ((double)Resources.Treasures.Size.Width / 50)));
                column++;
                if (column > 2)
                {
                    column = 0;
                    row++;
                }
            }
            #endregion
        }

        public Bitmap GetTreasureSprite(int treasureID)
        {
            foreach (var field in Resources.Treasures.GetType().GetFields())
                if (field.Name == "id" + treasureID)
                    return (Bitmap)field.GetValue(Resources.Treasures);
            return Resources.Treasures.idNotFound;
        }
    }
}

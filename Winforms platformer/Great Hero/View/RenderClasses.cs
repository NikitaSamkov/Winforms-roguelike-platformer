using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winforms_platformer.Model;

namespace Winforms_platformer.View
{
    public class EntityRender : IRenderable
    {
        public Entity Entity;
        public EntityResource Resource;
        private Bitmap CurrentSheet;
        private int maxFrames = 1;
        private int frame => (currentFrame / framePause) % maxFrames;
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
                    Entity.status = Status.Idle;
                    CurrentSheet = Resource.Idle;
                    maxFrames = Resource.Idle.Width / Resource.IdleSize.Width;
                    currentFrameSize = Resource.IdleSize;
                    currentFrame = 0;
                }
                else if (CurrentSheet == attackingResource.AttackMove)
                {
                    Entity.status = Status.Move;
                    CurrentSheet = Resource.Move;
                    maxFrames = Resource.Move.Width / Resource.MoveSize.Width;
                    currentFrameSize = Resource.MoveSize;
                    currentFrame = 0;
                }
            }
        }

        public void Paint(Graphics g)
        {
            ColorMatrix m = new ColorMatrix();
            if (!(Entity is Player) && Entity.invincibility > Entity.damageInvincibility - 5)
            {
                m.Matrix40 = 0.5f;
                m.Matrix41 = -0.3f;
                m.Matrix42 = -0.3f;
            }
            ImageAttributes a = new ImageAttributes();
            a.SetColorMatrix(m, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            GetCurrentSheet();
            StopAttackingIfNeeded();
            currentFrame++;
            if (Entity.invincibility % 2 != 1 || !(Entity is Player))
                g.DrawImage(CurrentSheet,
                    new Rectangle(((int)Entity.direction == 0) ? Entity.x : Entity.x + Entity.collider.field.Width - currentFrameSize.Width,
                    Entity.y, currentFrameSize.Width, currentFrameSize.Height),
                    currentFrameSize.Width * frame,
                        currentFrameSize.Height * (int)Entity.direction,
                        currentFrameSize.Width,
                        currentFrameSize.Height,
                        GraphicsUnit.Pixel, a);
            if (Game.DeveloperToolsON)
            {
                g.DrawRectangle(new Pen(Color.Green), Entity.x + Entity.collider.x, Entity.y + Entity.collider.y, Entity.collider.field.Width,
                        Entity.collider.field.Height);
                g.DrawEllipse(new Pen(Color.Red), Entity.x, Entity.y, 1, 1);
                if (Entity.status == Status.Attack || Entity.status == Status.AttackMove)
                {
                    var colliderX = (Entity.direction == 0) ?
                    Entity.collider.field.Width + Entity.collider.attackCollider.x + Entity.x + Entity.collider.x :
                    Entity.x - Entity.collider.attackCollider.x - Entity.collider.attackCollider.field.Width - Entity.collider.x;
                    var colliderY = Entity.y + Entity.collider.attackCollider.y + Entity.collider.y;
                    g.DrawRectangle(new Pen(Color.Purple), colliderX, colliderY, Entity.collider.attackCollider.field.Width, Entity.collider.attackCollider.field.Height);
                }
            }
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

        public EntityResource GetResources(Enemy enemy)
        {
            EntityResource resources = Res.Dummy;

            //вставить сюда if (enemy is CustomClass) resources = Resources.CustomClass
            if (enemy is Slime)
                resources = Res.Slime;
            if (enemy is Roller)
                resources = Res.Roller;
            if (enemy is SuperRoller)
                resources = Res.SuperRoller;
            if (enemy is Swordsman)
                resources = Res.Swordsman;
            if (enemy is Archer)
                resources = Res.Archer;
            if (enemy is Magician)
                resources = Res.Magician;
            if (enemy is SuperMagician)
                resources = Res.SuperMagician;
            if (enemy is BigCow)
                resources = Res.BigCow;
            if (enemy is Ghost)
                resources = Res.Ghost;
            if (enemy is InvisibleMan)
                resources = Res.InvisibleMan;
            if (enemy is Turret)
                resources = Res.Turret;
            if (enemy is Sticker)
                resources = Res.Sticker;
            if (enemy is Clone)
                resources = Res.Clone;
            if (enemy is Chameleon)
                resources = Res.Chameleon;

            return resources;
        }

        public void Paint(Graphics g)
        {
            foreach (var enemy in CurrentRoom().EnemyList)
            {
                var render = enemies.Where(e => e.Entity == (Entity)enemy).FirstOrDefault();
                if (render == null)
                {
                    var ticksPerFrame = 3;
                    if (enemy is BigCow)
                        ticksPerFrame = 2;
                    render = new EntityRender(enemy, GetResources(enemy), ticksPerFrame);
                    enemies.Add(render);
                }
                if (render.Entity is Sticker && (render.Entity as Sticker).stickerStatus == Status.Attack && render.Resource != Res.StickerAttack)
                    render.Resource = Res.StickerAttack;
                if (render.Entity is Chameleon)
                    render.Resource = GetResources((render.Entity as Chameleon).original);
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
                EntityResource resources = Res.Arrow;
                //вставить сюда if (projectile is CustomClass) resources = Resources.CustomClass
                if (projectile is Plasma)
                    resources = Res.Plasma;
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
                    Resource = Winforms_platformer.Res.TreasureRoom;
                    break;
                default:
                    Resource = Winforms_platformer.Res.Room;
                    break;
            }
        }

        public void Paint(Graphics g)
        {
            GetResource();
            g.DrawImage(Resource.Wall, 0, 0, Game.WindowSize.Width, Game.WindowSize.Height);
            foreach (var platform in CurrentRoom().Platforms)
            {
                g.DrawImage(Resource.Platform, platform.leftBorder, platform.level, new Rectangle(0, 0, (platform.rightBorder - platform.leftBorder) / 2, Resource.PlatformSize.Height), GraphicsUnit.Pixel);
                g.DrawImage(Resource.Platform, platform.leftBorder + (platform.rightBorder - platform.leftBorder) / 2, platform.level,
                    new Rectangle(Resource.PlatformSize.Width - (platform.rightBorder - platform.leftBorder) / 2, 0,
                    (platform.rightBorder - platform.leftBorder) / 2, Resource.PlatformSize.Height), GraphicsUnit.Pixel);
            }
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
                var resource = (loot is TreasureItem) ? (LootResource)Res.Treasures : (LootResource)Res.Loot;
                g.DrawImage(GetSprite(loot.ID, resource), loot.x, loot.y);
                if (Game.DeveloperToolsON)
                    g.DrawRectangle(new Pen(Color.Green), loot.x, loot.y, loot.collider.field.Width,
                        loot.collider.field.Height);
            }

        }

        public Bitmap GetSprite(int ID, LootResource resource)
        {
            if (ID == 0 && resource is LootRes && Game.Player.Treasures.Contains(TreasurePool.GetTreasureByID(3)))
                return (resource as LootRes).id0alt;
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
            var hpSize = new Size(Res.UI.HPSize100.Width * Game.Player.MaxHP / 100, Res.UI.HPSize.Height);
            g.DrawImage(Res.UI.HPBar, 50, 50,
                new Rectangle(0, 0, hpSize.Width * Game.Player.HP / Game.Player.MaxHP, hpSize.Height),
                GraphicsUnit.Pixel);
            if (hpSize.Width >= Res.UI.HPSize.Width)
                g.DrawImage(Res.UI.HPFrame, 50, 50, Res.UI.HPSize.Width, Res.UI.HPSize.Height);
            else
            {
                g.DrawImage(Res.UI.HPFrame, 50, 50,
                    new Rectangle(0, 0, hpSize.Width / 2 + 1, hpSize.Height), GraphicsUnit.Pixel);
                g.DrawImage(Res.UI.HPFrame, 50 + hpSize.Width / 2 + 1, 50,
                    new Rectangle(Res.UI.HPSize.Width - hpSize.Width / 2, 0, hpSize.Width / 2, hpSize.Height), GraphicsUnit.Pixel);
            }
            #endregion
            #region Ammo
            if (Game.Player.SecondaryWeapons[Game.Player.CurrentSecondaryWeapon] == SecondaryWeapon.Bow)
                for (var i = 0; i < Game.Player.Ammo; i++)
                {
                    g.DrawImage(Res.UI.Ammo, i * 15 + 50, 100, Res.UI.AmmoSize.Width, Res.UI.AmmoSize.Height);
                }
            else
            {
                EternalBow weapon = TreasurePool.GetTreasureByID(1) as EternalBow;
                Bitmap ammo = Res.UI.EternalAmmo;
                Bitmap ammoReloading = Res.UI.EternalAmmoReloading;
                Size ammoSize = Res.UI.EternalAmmoSize;
                if (Game.Player.SecondaryWeapons[Game.Player.CurrentSecondaryWeapon] == SecondaryWeapon.GhostForm)
                {
                    weapon = TreasurePool.GetTreasureByID(19) as GhostForm;
                    ammo = Res.UI.GhostForm;
                    ammoReloading = Res.UI.GhostFormReloading;
                    ammoSize = Res.UI.GhostFormSize;
                }
                if (Game.Player.SecondaryWeapons[Game.Player.CurrentSecondaryWeapon] == SecondaryWeapon.PlasmaBall)
                {
                    weapon = TreasurePool.GetTreasureByID(18) as PlasmaBall;
                    ammo = Res.UI.PlasmaAmmo;
                    ammoReloading = Res.UI.PlasmaAmmoReloading;
                    ammoSize = Res.UI.PlasmaAmmoSize;
                }
                if (weapon.timer == 0)
                    g.DrawImage(ammo, 50, 100, ammoSize.Width, ammoSize.Height);
                else
                    g.DrawImage(ammoReloading, 50, 100,
                        new Rectangle(0, 0, ammoSize.Width,
                        ammoSize.Height * (weapon.reloadTime - weapon.timer) / weapon.reloadTime), GraphicsUnit.Pixel);
            }
            #endregion
            #region Treasures
            var column = 0;
            var row = 0;
            foreach (var treasure in Game.Player.Treasures)
            {
                g.DrawImage(GetTreasureSprite(treasure.ID), Game.WindowSize.Width - 150 + 50 * column, 50 + 50 * row,
                    50, (float)(Res.Treasures.Size.Height / ((double)Res.Treasures.Size.Width / 50)));
                column++;
                if (column > 2)
                {
                    column = 0;
                    row++;
                }
            }
            #endregion
            #region BOSS HP
            if (Game.Map.CurrentRoom().Type == RoomType.BossRoom)
            {
                g.DrawImage(Res.UI.BossHPBar, Game.WindowSize.Width / 2 - Res.UI.BossHPSize.Width / 2,
                    Game.Map.CurrentRoom().GroundLevel + (Game.WindowSize.Height - Game.Map.CurrentRoom().GroundLevel) / 2 - Res.UI.BossHPSize.Height / 2,
                    new Rectangle(0, 0, Res.UI.BossHPSize.Width * Game.Boss.HP / Game.Boss.MaxHP, hpSize.Height),
                    GraphicsUnit.Pixel);
                g.DrawImage(Res.UI.BossHPFrame, Game.WindowSize.Width / 2 - Res.UI.BossHPSize.Width / 2,
                    Game.Map.CurrentRoom().GroundLevel + (Game.WindowSize.Height - Game.Map.CurrentRoom().GroundLevel) / 2 - Res.UI.BossHPSize.Height / 2);
            }
            #endregion
        }

        public Bitmap GetTreasureSprite(int treasureID)
        {
            foreach (var field in Res.Treasures.GetType().GetFields())
                if (field.Name == "id" + treasureID)
                    return (Bitmap)field.GetValue(Res.Treasures);
            return Res.Treasures.idNotFound;
        }
    }

    public class BossRender : IRenderable
    {
        private Func<Room> CurrentRoom;

        public BossRender(Func<Room> CurrentRoom)
        {
            this.CurrentRoom = CurrentRoom;
        }

        public void Paint(Graphics g)
        {
            if (CurrentRoom().Type == RoomType.BossRoom && Game.Boss.HP > 0)
            {
                if (Game.Boss.Status == BossStatus.SummonEnemies)
                {
                    g.DrawImage(Res.Boss.Summon, Game.Boss.x, Game.Boss.y,
                        new Rectangle(((5 - Game.Boss.summonTimer / 3 == 5) ? 4 : (5 - Game.Boss.summonTimer / 3)) * Res.Boss.BodySize.Width, 0, Res.Boss.BodySize.Width,
                        Res.Boss.BodySize.Height), GraphicsUnit.Pixel);
                }
                else
                    g.DrawImage(Res.Boss.Body, Game.Boss.x, Game.Boss.y);
                foreach (var hand in Game.Boss.Hands)
                {
                    Bitmap sprite;
                    if (hand.HandStatus == BossHandStatus.Palm)
                        sprite = (hand.CurrentZone == Zone.Left || hand.CurrentZone == Zone.CenterLeft) ? Res.Boss.LeftPalm : Res.Boss.RightPalm;
                    else
                        sprite = (hand.CurrentZone == Zone.Left || hand.CurrentZone == Zone.CenterLeft) ? Res.Boss.LeftFist : Res.Boss.RightFist;
                    g.DrawImage(sprite, hand.x, hand.y);
                }
                if (Game.DeveloperToolsON)
                {
                    g.DrawRectangle(new Pen(Color.Green), new Rectangle(new Point(Game.Boss.x, Game.Boss.y), Game.Boss.collider.field));
                    g.DrawRectangle(new Pen(Color.Green), new Rectangle(new Point(Game.Boss.Hands[0].x, Game.Boss.Hands[0].y), Game.Boss.Hands[0].collider.field));
                    g.DrawRectangle(new Pen(Color.Green), new Rectangle(new Point(Game.Boss.Hands[1].x, Game.Boss.Hands[1].y), Game.Boss.Hands[1].collider.field));
                    g.FillRectangle(new SolidBrush(Color.FromArgb(50, Color.White)), BossZones.GetRectangle(Zone.Left));
                    g.FillRectangle(new SolidBrush(Color.FromArgb(50, Color.Blue)), BossZones.GetRectangle(Zone.CenterLeft));
                    g.FillRectangle(new SolidBrush(Color.FromArgb(50, Color.Red)), BossZones.GetRectangle(Zone.Right));
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Winforms_platformer
{
    public partial class Form1 : Form
    {
        EntityRender playerRender;
        RoomRender roomRender;
        Map map;
        bool developerToolsON = true;

        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
            //генерация карты
            map = new Map();
            map.GenerateRooms();
            //сортировка сокровищницы
            TreasurePool.SortPool();
            //создание комнаты
            roomRender = new RoomRender(map.Current());

            //создание игрока
            playerRender = new EntityRender(new Player(150, 150, new Collider(PlayerRes.IdleSize, 0, 0, 
                new Collider(PlayerRes.AttackRange, -10, PlayerRes.Idle.Height / 8)),
                roomRender.room),
                new Sprite(PlayerRes.Idle, PlayerRes.IdleSize,
                PlayerRes.Move, PlayerRes.MoveSize,
                PlayerRes.Attack, PlayerRes.AttackSize,
                PlayerRes.Attack, PlayerRes.AttackSize,
                3));

            var mainTimer = new Timer();
            mainTimer.Interval = 60;
            mainTimer.Tick += (sender, args) =>
            {
                //механика, не позволяющая игроку перейти в след./пред. комнату, если текущая комната конечная/начальная или есть враги
                if (((map.IsCurrentRoomLast() || roomRender.room.enemyList.Count != 0) &&
                    playerRender.entity.x + playerRender.entity.collider.field.Width >= ClientSize.Width &&
                    playerRender.entity.currentDirection == Direction.Right) ||
                    ((map.IsCurrentRoomFirst() || roomRender.room.enemyList.Count != 0) &&
                    playerRender.entity.x <= 0 &&
                    playerRender.entity.currentDirection == Direction.Left))
                {
                    if (playerRender.entity.x < 0)
                        playerRender.entity.TeleportTo(0);
                    if (playerRender.entity.x + playerRender.entity.collider.field.Width > ClientSize.Width)
                        playerRender.entity.TeleportTo(ClientSize.Width - playerRender.entity.collider.field.Width);
                    playerRender.SetIdle();
                }
                //смена комнаты
                if ((playerRender.entity.x > ClientSize.Width || 
                playerRender.entity.x + playerRender.entity.collider.field.Width < 0))
                {
                    ChangeRoom();
                    playerRender.entity.room = roomRender.room;
                }
                //обновление противников
                foreach (var enemyRender in roomRender.room.enemyList)
                {
                    if (enemyRender.entity.hp <= 0)
                    {
                        roomRender.room.enemyList.Remove(enemyRender);
                        break;
                    }
                    if (enemyRender.entity.status == Status.Move && playerRender.entity.x == enemyRender.entity.x
                    && playerRender.entity.y == enemyRender.entity.y)
                        enemyRender.SetIdle();
                    else if (enemyRender.entity.status == Status.Idle && (playerRender.entity.x != enemyRender.entity.x ||
                    playerRender.entity.y != enemyRender.entity.y))
                        enemyRender.SetMoving();
                    else
                        (enemyRender.entity as Enemy).MoveToPlayer();
                    if (enemyRender.entity.IntersectsWithBody(playerRender.entity))
                        playerRender.Hurt(enemyRender.entity.damage);
                    enemyRender.Update();
                }
                //обновление игрока
                playerRender.Update();
                //обновление снарядов
                foreach (var projectile in roomRender.room.allyProjectilesList)
                {
                    if (projectile.entity.y + projectile.entity.collider.field.Height >= roomRender.room.groundLevel)
                    {
                        roomRender.room.allyProjectilesList.Remove(projectile);
                        break;
                    }
                    var targets = roomRender.room.GetIntersectedEntities(projectile.entity);
                    if (targets.Count != 0)
                    {
                        targets
                        .OrderBy(target => target.entity.GetDistanceTo(projectile.entity.x, projectile.entity.y))
                        .First()
                        .entity.hp -= projectile.entity.damage;
                        roomRender.room.allyProjectilesList.Remove(projectile);
                        break;
                    }
                    projectile.Update();
                }

                Invalidate();
            };
            mainTimer.Start();
        }

        private void ChangeRoom()
        {
            var heightAboveFloor = roomRender.room.groundLevel - playerRender.entity.y;
            if (playerRender.entity.x > ClientSize.Width && !map.IsCurrentRoomLast())
            {
                roomRender.ChangeRoom(map.GoToNext());
                playerRender.entity.TeleportTo(0, roomRender.room.groundLevel - heightAboveFloor);
            }
            if (playerRender.entity.x < 0 && !map.IsCurrentRoomFirst())
            {
                roomRender.ChangeRoom(map.GoToPrevious());
                playerRender.entity.TeleportTo(ClientSize.Width - playerRender.entity.collider.field.Width,
                    roomRender.room.groundLevel - heightAboveFloor);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            //отрисовка комнаты
            g.DrawImage(roomRender.wallSprite.idleSheet, 0, 0, roomRender.wallSprite.idleSize.Width,
                roomRender.wallSprite.idleSize.Height);
            g.DrawImage(roomRender.groundSprite.idleSheet, 
                roomRender.wallSprite.idleSize.Width - roomRender.groundSprite.idleSize.Width,
                roomRender.room.groundLevel,
                roomRender.groundSprite.idleSize.Width, roomRender.groundSprite.idleSize.Height);
            foreach (var platform in roomRender.room.platforms)
                g.DrawLine(new Pen(Color.Red, 5), platform.leftBorder, platform.level, platform.rightBorder, platform.level);
            //отрисовка врагов
            foreach (var enemy in roomRender.room.enemyList)
            {
                var size = enemy.sprite.GetSize();
                g.DrawImage(enemy.sprite.GetSheet(), enemy.entity.x, enemy.entity.y,
                    new Rectangle(size.Width * enemy.sprite.currentFrame,
                    size.Height * (int)enemy.entity.currentDirection,
                    size.Width, size.Height),
                    GraphicsUnit.Pixel);
                if (developerToolsON)
                {
                    g.DrawRectangle(new Pen(Color.Blue), 
                        new Rectangle(new Point(enemy.entity.x + enemy.entity.collider.x,
                        enemy.entity.y + enemy.entity.collider.y), enemy.entity.collider.field));
                }
            }
            //отрисовка игрока
            var playerSize = playerRender.sprite.GetSize();
            g.DrawImage(playerRender.sprite.GetSheet(), 
                ((int)playerRender.entity.currentDirection == 0) ? playerRender.entity.x :
                    playerRender.entity.x + playerRender.entity.collider.field.Width - playerSize.Width,
                playerRender.entity.y,
                new Rectangle(playerSize.Width * playerRender.sprite.currentFrame,
                    playerSize.Height * (int)playerRender.entity.currentDirection,
                    playerSize.Width, playerSize.Height),
                GraphicsUnit.Pixel);
            if (developerToolsON)
            {
                g.DrawRectangle(new Pen(Color.Green), 
                    new Rectangle(new Point(playerRender.entity.x + playerRender.entity.collider.x,
                    playerRender.entity.y + playerRender.entity.collider.y), playerRender.entity.collider.field));
                var colliderPoint = (playerRender.entity.currentDirection == 0) ?
                    new Point(PlayerRes.IdleSize.Width + playerRender.entity.collider.attackCollider.x + playerRender.entity.x,
                    playerRender.entity.y + playerRender.entity.collider.attackCollider.y) :
                    new Point(-playerRender.entity.collider.attackCollider.x + playerRender.entity.x - 
                    playerRender.entity.collider.attackCollider.field.Width,
                    playerRender.entity.y + playerRender.entity.collider.attackCollider.y);
                g.DrawRectangle(new Pen(Color.IndianRed),
                    new Rectangle(colliderPoint,
                        playerRender.entity.collider.attackCollider.field));
            }
            //отрисовка снарядов
            foreach (var projectile in roomRender.room.allyProjectilesList)
            {
                var size = projectile.sprite.GetSize();
                g.DrawImage(projectile.sprite.GetSheet(), projectile.entity.x, projectile.entity.y,
                    new Rectangle(size.Width * projectile.sprite.currentFrame,
                    size.Height * (int)projectile.entity.currentDirection,
                    size.Width, size.Height),
                    GraphicsUnit.Pixel);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                //влево
                case Keys.Left:
                case Keys.A:
                    playerRender.entity.MoveTo(Direction.Left);
                    playerRender.SetMoving();
                    break;
                //вправо
                case Keys.Right:
                case Keys.D:
                    playerRender.entity.MoveTo(Direction.Right);
                    playerRender.SetMoving();
                    break;
                //вверх
                case Keys.Up:
                case Keys.W:
                    if ((playerRender.entity as Player).flying)
                        playerRender.entity.MoveUp();
                    else
                        (playerRender.entity as Player).Jump();
                    break;
                //вниз
                case Keys.Down:
                case Keys.S:
                    if ((playerRender.entity as Player).flying)
                        playerRender.entity.MoveDown();
                    else
                        playerRender.entity.MoveDown(1);
                    break;
                //вывод в консоль seed
                case Keys.M:
                    Console.WriteLine(map.seed);
                    break;
                //спавн простого врага
                case Keys.D0:
                    roomRender.room.enemyList.Add(new EntityRender(new Enemy(playerRender.entity.x, playerRender.entity.y, 
                        new Collider(DummyRes.IdleSize), roomRender.room, 
                        (Player)playerRender.entity), 
                        new Sprite(DummyRes.Idle, DummyRes.IdleSize,
                        DummyRes.Move, DummyRes.MoveSize, 
                        null, new Size(),
                        null, new Size(), 3)));
                    break;
                //уничтожение последнего врага в списке
                case Keys.D9:
                    if (roomRender.room.enemyList.Count > 0)
                        roomRender.room.enemyList.RemoveAt(0);
                    break;
                //ближнебойная атака
                case Keys.E:
                    playerRender.SetAttacking();
                    break;
                //дальнобойная атака
                case Keys.Q:
                    var arrow =
                        new EntityRender(
                        new Arrow(
                        playerRender.entity.x,
                        playerRender.entity.y + playerRender.entity.collider.field.Height / 2,
                        new Collider(ProjectilesRes.ArrowSize), roomRender.room, 15, (playerRender.entity as Player).bowStrenght),
                        new Sprite(ProjectilesRes.Arrow, ProjectilesRes.ArrowSize,
                        ProjectilesRes.Arrow, ProjectilesRes.ArrowSize));
                    arrow.entity.MoveTo(playerRender.entity.currentDirection);
                    arrow.SetMoving();
                    roomRender.room.allyProjectilesList.Add(arrow);
                    break;
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A || e.KeyCode == Keys.Left || e.KeyCode == Keys.D || e.KeyCode == Keys.Right)
                playerRender.SetIdle();
        }
    }
}

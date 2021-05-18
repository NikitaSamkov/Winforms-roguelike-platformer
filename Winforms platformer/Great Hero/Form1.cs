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
        List<EntityRender> enemyList;
        Map map;
        Sprite dummySprite;

        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
            //генерация карты
            map = new Map();
            map.GenerateRooms();
            //мортировка сокровищницы
            TreasurePool.SortPool();
            //создание комнаты
            roomRender = new RoomRender(map.Current());

            //создание игрока
            playerRender = new EntityRender(new Player(150, 150, new Collider(PlayerBitmaps.IdleSize),
                roomRender.room.GetYSpeed, roomRender.room.OnTheSurface),
                new Sprite(PlayerBitmaps.Idle, PlayerBitmaps.IdleSize,
                PlayerBitmaps.Move, PlayerBitmaps.MoveSize,
                null, new Size(), 3));
            //создание списка врагов
            enemyList = new List<EntityRender>();

            var timer = new Timer();
            timer.Interval = 60;
            timer.Tick += (sender, args) =>
            {
                //механика, не позволяющая игроку перейти в след./пред. комнату, если текущая комната конечная/начальная или есть враги
                if (((map.IsCurrentRoomLast() || enemyList.Count != 0) &&
                    playerRender.entity.x + playerRender.entity.collider.field.Width >= ClientSize.Width &&
                    playerRender.entity.currentDirection == Direction.Right) ||
                    ((map.IsCurrentRoomFirst() || enemyList.Count != 0) &&
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
                    (playerRender.entity as Player).UpdateRoom(roomRender.room.OnTheSurface, roomRender.room.GetYSpeed);
                }
                //обновление противников
                foreach (var enemyRender in enemyList)
                {
                    if (enemyRender.entity.status == Status.Move && playerRender.entity.x == enemyRender.entity.x)
                        enemyRender.SetIdle();
                    else if (enemyRender.entity.status == Status.Idle && playerRender.entity.x != enemyRender.entity.x)
                        enemyRender.SetMoving();
                    else
                        (enemyRender.entity as Enemy).MoveToPlayer();
                    enemyRender.Update();
                }
                //обновление игрока
                playerRender.Update();

                Invalidate();
            };
            timer.Start();
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
            foreach (var enemy in enemyList)
            {
                var size = enemy.sprite.GetSize();
                g.DrawImage(enemy.sprite.GetSheet(), enemy.entity.x, enemy.entity.y - size.Height,
                    new Rectangle(size.Width * enemy.sprite.currentFrame,
                    size.Height * (int)enemy.entity.currentDirection,
                    size.Width, size.Height),
                    GraphicsUnit.Pixel);
            }
            //отрисовка игрока
            var playerSize = playerRender.sprite.GetSize();
            g.DrawImage(playerRender.sprite.GetSheet(), playerRender.entity.x,
                playerRender.entity.y - playerSize.Height,
                new Rectangle(playerSize.Width * playerRender.sprite.currentFrame,
                    playerSize.Height * (int)playerRender.entity.currentDirection,
                    playerSize.Width, playerSize.Height),
                GraphicsUnit.Pixel);
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
                    enemyList.Add(new EntityRender(new Enemy(playerRender.entity.x, playerRender.entity.y, 
                        new Collider(DummyBitmaps.IdleSize), roomRender.room.GetYSpeed, roomRender.room.OnTheSurface, 
                        (Player)playerRender.entity), 
                        new Sprite(DummyBitmaps.Idle, DummyBitmaps.IdleSize,
                        DummyBitmaps.Move, DummyBitmaps.MoveSize, null, new Size(), 3)));
                    break;
                //уничтожение последнего врага в списке
                case Keys.D9:
                    if (enemyList.Count > 0)
                        enemyList.RemoveAt(0);
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

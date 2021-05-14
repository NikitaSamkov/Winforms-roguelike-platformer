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
        Sprite playerSprite;
        Sprite dummySprite;

        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;

            map = new Map();
            map.GenerateRooms();

            TreasurePool.SortPool();

            roomRender = new RoomRender(map.Current());


            playerSprite = new Sprite(PlayerBitmaps.FullSize, PlayerBitmaps.Idle, PlayerBitmaps.Move, 3);
            playerRender = new EntityRender(new Player(150, 150, playerSprite.spriteWidth,
                roomRender.room.GetYSpeed, roomRender.room.OnTheSurface),
                playerSprite);

            dummySprite = new Sprite(DummyBitmaps.FullSize, DummyBitmaps.Idle, DummyBitmaps.Move);
            dummySprite.SetMoving();

            enemyList = new List<EntityRender>();

            var timer = new Timer();
            timer.Interval = 60;
            timer.Tick += (sender, args) =>
            {
                if (((map.IsCurrentRoomLast() || enemyList.Count != 0) &&
                    playerRender.entity.x + playerRender.entity.width >= ClientSize.Width &&
                    playerRender.entity.currentDirection == Direction.Right) ||
                    ((map.IsCurrentRoomFirst() || enemyList.Count != 0) &&
                    playerRender.entity.x <= 0 &&
                    playerRender.entity.currentDirection == Direction.Left))
                {
                    if (playerRender.entity.x < 0)
                        playerRender.entity.TeleportTo(0);
                    if (playerRender.entity.x + playerRender.entity.width > ClientSize.Width)
                        playerRender.entity.TeleportTo(ClientSize.Width - playerRender.entity.width);
                    playerRender.SetIdle();
                }

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
                
                if ((playerRender.entity.x > ClientSize.Width || playerRender.entity.x + playerRender.entity.width < 0))
                {
                    ChangeRoom();
                    (playerRender.entity as Player).UpdateRoom(roomRender.room.OnTheSurface, roomRender.room.GetYSpeed);
                }
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
                playerRender.entity.TeleportTo(ClientSize.Width - playerRender.entity.width,
                    roomRender.room.groundLevel - heightAboveFloor);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.DrawImage(roomRender.wallSprite.idleSheet, 0, 0, roomRender.wallSprite.spriteWidth,
                roomRender.wallSprite.spriteHeight);
            g.DrawImage(roomRender.groundSprite.idleSheet, roomRender.wallSprite.spriteWidth - roomRender.groundSprite.spriteWidth,
                roomRender.room.groundLevel,
                roomRender.groundSprite.spriteWidth, roomRender.groundSprite.spriteHeight);
            foreach (var platform in roomRender.room.platforms)
                g.DrawLine(new Pen(Color.Red, 5), platform.leftBorder, platform.level, platform.rightBorder, platform.level);
            foreach (var enemy in enemyList)
                g.DrawImage(enemy.sprite.GetSheet(), enemy.entity.x, enemy.entity.y - enemy.sprite.spriteHeight,
                    new Rectangle(enemy.sprite.spriteWidth * enemy.sprite.currentFrame,
                    enemy.sprite.spriteHeight * (int)enemy.entity.currentDirection,
                    enemy.sprite.spriteWidth,
                    enemy.sprite.spriteHeight),
                    GraphicsUnit.Pixel);
            g.DrawImage(playerRender.sprite.GetSheet(), playerRender.entity.x,
                playerRender.entity.y - playerRender.sprite.spriteHeight,
                new Rectangle(playerRender.sprite.spriteWidth * playerRender.sprite.currentFrame,
                    playerRender.sprite.spriteHeight * (int)playerRender.entity.currentDirection,
                    playerRender.sprite.spriteWidth,
                    playerRender.sprite.spriteHeight),
                GraphicsUnit.Pixel);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                case Keys.A:
                    playerRender.entity.MoveTo(Direction.Left);
                        playerRender.SetMoving();
                    break;
                case Keys.Right:
                case Keys.D:
                    playerRender.entity.MoveTo(Direction.Right);
                        playerRender.SetMoving();
                    break;
                case Keys.Up:
                case Keys.W:
                    if ((playerRender.entity as Player).flying)
                        playerRender.entity.MoveUp();
                    else
                        (playerRender.entity as Player).Jump();
                    break;
                case Keys.Down:
                case Keys.S:
                    if ((playerRender.entity as Player).flying)
                        playerRender.entity.MoveDown();
                    else
                        playerRender.entity.MoveDown(1);
                    break;
                case Keys.M:
                    Console.WriteLine(map.seed);
                    break;
                case Keys.D0:
                    enemyList.Add(new EntityRender(new Enemy(playerRender.entity.x, playerRender.entity.y,
                        dummySprite.spriteWidth, roomRender.room.GetYSpeed, roomRender.room.OnTheSurface, 
                        (Player)playerRender.entity), dummySprite));
                    break;
                case Keys.D9:
                    if (enemyList.Count > 0)
                        enemyList.RemoveAt(0);
                    break;
                case Keys.D8:
                    TreasurePool.GiveToPlayer(playerRender, 0);
                    break;
                case Keys.D7:
                    (playerRender.entity as Player).treasures = new List<ITreasure>();
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

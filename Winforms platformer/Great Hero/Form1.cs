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

            roomRender = new RoomRender(map.Current());

            
            playerSprite = new Sprite(PlayerBitmaps.FullSize, PlayerBitmaps.Idle, PlayerBitmaps.Move, 3);
            playerRender = new EntityRender(new Player(150, 150, playerSprite.spriteWidth,
                roomRender.room.GetYSpeed, roomRender.room.OnTheSurface),
                playerSprite);

            dummySprite = new Sprite(DummyBitmaps.FullSize, null, DummyBitmaps.Move);
            dummySprite.SetMoving();

            enemyList = new List<EntityRender>();

            var timer = new Timer();
            timer.Interval = 60;
            timer.Tick += (sender, args) =>
            {
                foreach (var enemy in enemyList)
                {
                    if (enemy.sprite.currentStatus == Status.Move && playerRender.entity.x == enemy.entity.x)
                        enemy.sprite.SetIdle();
                    else if (enemy.sprite.currentStatus == Status.Idle && playerRender.entity.x != enemy.entity.x)
                        enemy.sprite.SetMoving();
                    else
                        enemy.entity.MoveTo(playerRender.entity);
                    enemy.entity.Move(enemy.sprite.currentStatus);
                    enemy.sprite.StepFrame();
                }
                if ((map.IsCurrentRoomLast() &&
                    playerRender.entity.x + playerRender.entity.width >= ClientSize.Width &&
                    playerRender.entity.currentDirection == Direction.Right) ||
                    (map.IsCurrentRoomFirst() &&
                    playerRender.entity.x == 0 &&
                    playerRender.entity.currentDirection == Direction.Left))
                {
                    if (playerRender.entity.x < 0)
                        playerRender.entity.TeleportTo(0);
                    if (playerRender.entity.x + playerRender.entity.width > ClientSize.Width)
                        playerRender.entity.TeleportTo(ClientSize.Width - playerRender.entity.width);
                    playerRender.sprite.SetIdle();
                }
                playerRender.entity.Move(playerRender.sprite.currentStatus);
                playerRender.sprite.StepFrame();
                if ((playerRender.entity.x > ClientSize.Width || playerRender.entity.x + playerRender.entity.width < 0) &&
                enemyList.Count == 0)
                {
                    ChangeRoom();
                    playerRender.entity.UpdateRoom(roomRender.room.OnTheSurface, roomRender.room.GetYSpeed);
                }
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
                    playerRender.sprite.SetMoving();
                    break;
                case Keys.Right:
                case Keys.D:
                    playerRender.entity.MoveTo(Direction.Right);
                    playerRender.sprite.SetMoving();
                    break;
                case Keys.Up:
                case Keys.W:
                    playerRender.entity.Jump();
                    break;
                case Keys.Down:
                case Keys.S:
                    playerRender.entity.GoDown();
                    break;
                case Keys.M:
                    Console.WriteLine(map.seed);
                    break;
                case Keys.D0:
                    enemyList.Add(new EntityRender(new Dummy(playerRender.entity.x, playerRender.entity.y,
                        dummySprite.spriteWidth, roomRender.room.GetYSpeed, roomRender.room.OnTheSurface, 5), dummySprite));
                    break;
                case Keys.D9:
                    if (enemyList.Count > 0)
                        enemyList.RemoveAt(0);
                    break;
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A || e.KeyCode == Keys.Left || e.KeyCode == Keys.D || e.KeyCode == Keys.Right)
                playerRender.sprite.SetIdle();
        }
    }
}

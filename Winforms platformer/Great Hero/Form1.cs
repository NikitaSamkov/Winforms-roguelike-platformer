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
        CreatureRender playerRender;
        RoomRender roomRender;
        List<CreatureRender> enemyList;
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

            playerSprite = new Sprite(new Bitmap(@"..\..\..\..\Sprites\Player\PlayerFullSize.png"),
                new Bitmap(@"..\..\..\..\Sprites\Player\PlayerIdle.png"),
                new Bitmap(@"..\..\..\..\Sprites\Player\PlayerMove.png"));
            playerRender = new CreatureRender(new Player(150, 150, playerSprite.spriteWidth,
                roomRender.room.GetYSpeed, roomRender.room.OnTheSurface),
                playerSprite);

            dummySprite = new Sprite(new Bitmap(@"..\..\..\..\Sprites\Enemy\DummyFullSize.png"), null,
                        new Bitmap(@"..\..\..\..\Sprites\Enemy\DummyWalk.png"));
            dummySprite.SetMoving();

            enemyList = new List<CreatureRender>();

            var timer = new Timer();
            timer.Interval = 60;
            timer.Tick += (sender, args) =>
            {
                foreach (var enemy in enemyList)
                {
                    if (enemy.sprite.currentStatus == Status.Move && playerRender.creature.x == enemy.creature.x)
                        enemy.sprite.SetIdle();
                    else if (enemy.sprite.currentStatus == Status.Idle && playerRender.creature.x != enemy.creature.x)
                        enemy.sprite.SetMoving();
                    else
                        enemy.creature.MoveTo(playerRender.creature);
                    enemy.creature.Move(enemy.sprite.currentStatus);
                    enemy.sprite.StepFrame();
                }
                playerRender.creature.Move(playerRender.sprite.currentStatus);
                playerRender.sprite.StepFrame();
                if ((playerRender.creature.x > ClientSize.Width || playerRender.creature.x < 0) && enemyList.Count == 0)
                {
                    ChangeRoom();
                    playerRender.creature.UpdateRoom(roomRender.room.OnTheSurface, roomRender.room.GetYSpeed);
                }
                Invalidate();
            };
            timer.Start();
        }

        private void ChangeRoom()
        {
            if (playerRender.creature.x > ClientSize.Width && !map.IsCurrentRoomLast())
            {
                roomRender.ChangeRoom(map.GoToNext());
                playerRender.creature.TeleportTo(0);
            }
            if (playerRender.creature.x < 0 && !map.IsCurrentRoomFirst())
            {
                roomRender.ChangeRoom(map.GoToPrevious());
                playerRender.creature.TeleportTo(ClientSize.Width);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            var groundSheet = roomRender.groundSprite.idleSheet;
            g.DrawImage(roomRender.wallSprite.idleSheet, 0, 0, roomRender.wallSprite.spriteWidth,
                roomRender.wallSprite.spriteHeight);
            g.DrawImage(roomRender.groundSprite.idleSheet, roomRender.wallSprite.spriteWidth - groundSheet.Width,
                roomRender.room.groundLevel,
                groundSheet.Width, groundSheet.Height);
            foreach (var platform in roomRender.room.platforms)
                g.DrawLine(new Pen(Color.Red, 5), platform.leftBorder, platform.level, platform.rightBorder, platform.level);
            foreach (var enemy in enemyList)
                g.DrawImage(enemy.sprite.GetSheet(), enemy.creature.x, enemy.creature.y - enemy.sprite.spriteHeight,
                    new Rectangle(enemy.sprite.spriteWidth * enemy.sprite.currentFrame,
                    enemy.sprite.spriteHeight * (int)enemy.creature.currentDirection,
                    enemy.sprite.spriteWidth,
                    enemy.sprite.spriteHeight),
                    GraphicsUnit.Pixel);
            g.DrawImage(playerRender.sprite.GetSheet(), playerRender.creature.x,
                playerRender.creature.y - playerRender.sprite.spriteHeight,
                new Rectangle(playerRender.sprite.spriteWidth * playerRender.sprite.currentFrame,
                    playerRender.sprite.spriteHeight * (int)playerRender.creature.currentDirection,
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
                    playerRender.creature.MoveTo(Direction.Left);
                    playerRender.sprite.SetMoving();
                    break;
                case Keys.Right:
                case Keys.D:
                    playerRender.creature.MoveTo(Direction.Right);
                    playerRender.sprite.SetMoving();
                    break;
                case Keys.Up:
                case Keys.W:
                    playerRender.creature.Jump();
                    break;
                case Keys.Down:
                case Keys.S:
                    playerRender.creature.GoDown();
                    break;
                case Keys.D0:
                    enemyList.Add(new CreatureRender(new Dummy(playerRender.creature.x, playerRender.creature.y,
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

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

        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;

            roomRender = new RoomRender(new Bitmap(@"..\..\..\..\Sprites\Room\Wall.png"), 
                new Bitmap(@"..\..\..\..\Sprites\Room\Ground.png"));
            var playerSprite = new Sprite(new Bitmap(@"..\..\..\..\Sprites\Player\PlayerFullSize.png"),
                new Bitmap(@"..\..\..\..\Sprites\Player\PlayerIdle.png"),
                new Bitmap(@"..\..\..\..\Sprites\Player\PlayerMove.png"));
            playerRender = new CreatureRender(new Player(150, 150, playerSprite.spriteWidth, roomRender.room.GetYSpeed), 
                playerSprite);
            var timer = new Timer();
            timer.Interval = 60;
            timer.Tick += (sender, args) =>
            {
                playerRender.sprite.StepFrame();
                playerRender.creature.Move(playerRender.sprite.currentStatus);
                Invalidate();
            };
            timer.Start();
        }



        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            var groundSheet = roomRender.groundSprite.idleSheet;
            g.DrawImage(roomRender.wallSprite.idleSheet, 0, 0, roomRender.wallSprite.spriteWidth, 
                roomRender.wallSprite.spriteHeight);
            g.DrawImage(roomRender.groundSprite.idleSheet, roomRender.wallSprite.spriteWidth - groundSheet.Width, 
                roomRender.wallSprite.spriteHeight - groundSheet.Height,
                groundSheet.Width, groundSheet.Height);
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
                    playerRender.creature.currentDirection = Direction.Left;
                    playerRender.sprite.SetMoving();
                    break;
                case Keys.Right:
                case Keys.D:
                    playerRender.creature.currentDirection = Direction.Right;
                    playerRender.sprite.SetMoving();
                    break;
                case Keys.Up:
                case Keys.W:

            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            playerRender.sprite.SetIdle();
        }
    }
}

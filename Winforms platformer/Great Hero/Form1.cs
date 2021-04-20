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
        Room room;

        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;

            room = new Room(800, 600);
            var playerSprite = new Sprite(new Bitmap(@"..\..\..\..\Sprites\Player\PlayerFullSize.png"),
                new Bitmap(@"..\..\..\..\Sprites\Player\PlayerIdle.png"),
                new Bitmap(@"..\..\..\..\Sprites\Player\PlayerMove.png"));
            playerRender = new CreatureRender(new Player(150, 150, playerSprite.spriteWidth, room.GetYSpeed), playerSprite);
            var timer = new Timer();
            timer.Interval = 100;
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
            var groundSheet = room.groundSheet;
            g.DrawImage(room.wallSheet, 0, 0, 800, 600);
            g.DrawImage(room.groundSheet, 800 - groundSheet.Width, 600 - groundSheet.Height,
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
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            playerRender.sprite.SetIdle();
        }
    }
}

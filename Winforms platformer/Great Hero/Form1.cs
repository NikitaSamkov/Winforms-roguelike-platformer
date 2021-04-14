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
        Player player;
        Room room;

        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
            Load += (sender, args) => OnSizeChanged(EventArgs.Empty);

            room = new Room(800, 600);
            player = new Player(150, 150, room.GetYSpeed);
            var timer = new Timer();
            timer.Interval = 100;
            timer.Tick += (sender, args) =>
            {
                player.StepFrame();
                player.Move();
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
            g.DrawImage(player.GetSheet(), player.X, player.Y,
                new Rectangle(player.spritesWidth * player.currentFrame,
                    player.spritesHeight * (int)player.currentDirection,
                    player.spritesWidth,
                    player.spritesHeight),
                GraphicsUnit.Pixel);
            g.DrawEllipse(new Pen(Color.Red), player.X, player.Y, 1, 1);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.A:
                    player.MoveTo(Direction.Left);
                    break;
                case Keys.D:
                    player.MoveTo(Direction.Right);
                    break;
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            player.SetIdle();
        }
    }
}

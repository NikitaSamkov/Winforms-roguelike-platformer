using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Great_Hero
{
    public partial class Form1 : Form
    {
        Player player;
        
        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;

            player = new Player(150, 150);
            var timer = new Timer();
            timer.Interval = 100;
            timer.Tick += (sender, args) =>
            {
                player.StepFrame();
                Invalidate();
            };
            timer.Start();
        }

        

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            var sheet = player.GetSheet();
            g.DrawImage(sheet, player.X, player.Y,
                new Rectangle(player.spritesWidth * player.currentFrame, 
                    player.spritesHeight * (int)player.currentDirection, 
                    player.spritesWidth - 1, 
                    player.spritesHeight - 1),
                GraphicsUnit.Pixel);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.A:
                    player.Move(Direction.Left);
                    break;
                case Keys.D:
                    player.Move(Direction.Right);
                    break;
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            player.SetIdle();
        }
    }
}

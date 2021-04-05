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
        Bitmap playerIdleSheet = new Bitmap(@"..\..\..\..\Sprites\PlayerIdle.png");
        Bitmap playerWalkSheet = new Bitmap(@"..\..\..\..\Sprites\PlayerWalk.png");
        Point playerCoords;
        int currentFrame;
        int currentDirection;
        int currentAnimation;
        enum Direction
        {
            Right = 0,
            Left = 1
        }
        enum AnimationType
        {
            Idle = 0,
            Walk = 1
        }

        public Form1()
        {
            InitializeComponent();
            playerCoords = new Point(150, 150);
            currentDirection = (int)Direction.Right;
            currentAnimation = (int)AnimationType.Walk;
            var timer = new Timer();
            timer.Interval = 100;
            timer.Tick += (sender, args) =>
            {
                StepFrame();
                Invalidate();
            };
            timer.Start();
        }

        void StepFrame()
        {
            currentFrame++;
            if (currentFrame > 1)
                currentFrame = 0;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.DrawImage(playerWalkSheet, playerCoords.X, playerCoords.Y,
                new Rectangle(50 * currentFrame, 140 * currentDirection, 50, 140),
                GraphicsUnit.Pixel);
        }
    }
}

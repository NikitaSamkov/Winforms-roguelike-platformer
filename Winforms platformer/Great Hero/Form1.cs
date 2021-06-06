using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Winforms_platformer.Model;
using Winforms_platformer.View;

namespace Winforms_platformer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.FixedToolWindow;

            var mainTimer = new Timer();
            mainTimer.Interval = 60;
            mainTimer.Tick += (sender, args) =>
            {
                Game.SetWindowSize(ClientSize.Width, ClientSize.Height);
                Game.Update();
                Invalidate();
            };
            mainTimer.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            GameRender.RenderAll(g);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {if (Game.KeyBindings.ContainsKey(e.KeyCode))
                Game.KeyBindings[e.KeyCode]();
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {if (e.Button == MouseButtons.Right)
                Game.TryThrowTreasure(e.X, e.Y);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.A || e.KeyCode == Keys.Left || e.KeyCode == Keys.D || e.KeyCode == Keys.Right) && e.KeyCode == Game.lastKey)
                if (Game.Player.status == Status.AttackMove)
                    Game.Player.status = Status.Attack;
                else
                    Game.Player.status = Status.Idle;
        }
    }
}

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
    public class Player : Creature
    {
        public List<ITreasure> treasures { get; set; }
        public Player(int x, int y, Collider collider, Func<Room> room) 
            : base(x, y, collider, room)
        {
            direction = Direction.Right;
            xSpeed = 20;
            treasures = new List<ITreasure>();
            HP = 100;
            MaxHP = HP;
            damage = 10;
        }

        public override void Update()
        {
            if (treasures.Contains(TreasurePool.GetTreasureByID(19)))
                (TreasurePool.GetTreasureByID(19) as GhostForm).UpdateTimer();
            else if (treasures.Contains(TreasurePool.GetTreasureByID(18)))
                (TreasurePool.GetTreasureByID(18) as EternalBow).UpdateTimer();
            else if (treasures.Contains(TreasurePool.GetTreasureByID(1)))
                (TreasurePool.GetTreasureByID(1) as EternalBow).UpdateTimer();
            base.Update();
        }

        public override void Shoot(int angle = 15)
        {
            if (Ammo > 0)
            {
                var arrow = new Arrow(x, y + collider.field.Height / 2,
                        new Collider(Resources.Arrow.IdleSize), CurrentRoom, angle, ShootingPower, ProjectileType.Ally, this);
                arrow.MoveTo(direction);
                arrow.status = Status.Move;
                CurrentRoom().ProjectilesList.Add(arrow);
                Ammo--;
            }
            else if (treasures.Contains(TreasurePool.GetTreasureByID(19)))
            {
                var ghost = TreasurePool.GetTreasureByID(19) as GhostForm;
                if (ghost.timer == 0)
                {
                    invincibility = ghost.Power;
                    ghost.SetTimer();
                }
            }
            else if (treasures.Contains(TreasurePool.GetTreasureByID(18)))
            {
                var ball = TreasurePool.GetTreasureByID(18) as PlasmaBall;
                if (ball.timer == 0)
                {
                    var plasma = new Plasma(x, y + collider.field.Height / 2,
                        new Collider(Resources.Plasma.IdleSize), CurrentRoom, (direction == Direction.Right) ? 0 : 180, ball.PlasmaSpeed, ProjectileType.Ally, this);
                    plasma.status = Status.Move;
                    CurrentRoom().ProjectilesList.Add(plasma);
                    ball.SetTimer();
                }
            }
            else if (treasures.Contains(TreasurePool.GetTreasureByID(1)))
            {
                var bow = TreasurePool.GetTreasureByID(1) as EternalBow;
                if (bow.timer == 0)
                {
                    var arrow = new Arrow(x, y + collider.field.Height / 2,
                        new Collider(Resources.Arrow.IdleSize), CurrentRoom, angle, ShootingPower, ProjectileType.Ally, this);
                    arrow.MoveTo(direction);
                    arrow.status = Status.Move;
                    CurrentRoom().ProjectilesList.Add(arrow);
                    bow.SetTimer();
                }
            }
        }
    }
}

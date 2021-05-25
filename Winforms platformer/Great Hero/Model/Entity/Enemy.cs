using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winforms_platformer
{
    public class Enemy : Creature
    {
        private Player player;
        public Enemy(int x, int y, Collider collider, Func<Room> room,
            Player player) 
            : base(x, y, collider, room)
        {
            this.player = player;
            hp = 20;
            damage = 10;
        }

        public override void Update()
        {
            MoveToPlayer();
            base.Update();
        }

        public void MoveToPlayer()
        {
            status = Status.Move;
            direction = (x - player.x >= 0) ? Direction.Left : Direction.Right;
            var distance = GetDistanceTo(player.x, player.y);
            if (distance > 150 && xSpeed < 15 || distance <= 100 && xSpeed < 10)
                xSpeed++;
            else if (distance <= 100 && xSpeed > 10)
                xSpeed--;
            if (Math.Abs(x - player.x) < xSpeed)
                xSpeed = Math.Abs(x - player.x);
            if (player.y + player.collider.field.Height < y + collider.field.Height && distance < 150)
                Jump();
        }
    }
}

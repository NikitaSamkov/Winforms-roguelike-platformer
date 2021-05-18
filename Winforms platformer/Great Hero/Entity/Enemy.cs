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
        public Enemy(int x, int y, Collider collider, Func<int, int,int,int,int> moveY, Func<int,int,int,bool> canJump,
            Player player) 
            : base(x, y, collider, moveY, canJump)
        {
            this.player = player;
        }
        
        public void MoveToPlayer()
        {
            currentDirection = (x - player.x >= 0) ? Direction.Left : Direction.Right;
            var distance = GetDistanceTo(player.x, player.y);
            if (distance > 150 && xSpeed < 25)
                xSpeed++;
            else if (distance <= 100 && xSpeed > 10)
                xSpeed--;
            if (Math.Abs(x - player.x) < xSpeed)
                xSpeed = Math.Abs(x - player.x);
            if (player.y < y && distance < 150)
                Jump();
        }
    }
}

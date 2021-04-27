using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winforms_platformer
{
    class Dummy : Creature
    {
        public Dummy(int x, int y, int dummyWidth, Func<int, int,int,int,int> moveY, Func<int,int,int,bool> canJump, int dummySpeed) 
            : base(x, y, dummyWidth, moveY, canJump)
        {
            xSpeed = dummySpeed;
        }
    }
}

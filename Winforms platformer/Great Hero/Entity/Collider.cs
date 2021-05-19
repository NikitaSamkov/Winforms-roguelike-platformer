using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winforms_platformer
{
    public class Collider
    {
        public Size field { get; set; }
        public Collider attackCollider { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int Left => x;
        public int Right => x + field.Width;
        public int Bottom => y + field.Height;
        public int Top => y;

        public Collider(Size field, int x = 0, int y = 0, Collider attackCollider = null)
        {
            this.field = field;
            this.x = x;
            this.y = y;
            this.attackCollider = attackCollider;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winforms_platformer
{
    class Platform
    {
        public Rectangle field { get; private set; }
        public readonly Bitmap sheet;

        public Platform(Rectangle field, Bitmap sheet)
        {
            this.field = field;
            this.sheet = sheet;
        }
    }
}

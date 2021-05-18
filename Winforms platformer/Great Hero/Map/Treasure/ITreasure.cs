using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winforms_platformer
{
    public interface ITreasure
    {
        void Apply(EntityRender target);
        int GetID();
    }
}

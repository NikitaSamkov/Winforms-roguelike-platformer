using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winforms_platformer
{
    public interface ITreasure
    {
        void Enable(Player player);
        void Disable(Player player);
        int ID { get; }
    }

    public class TreasureItem : Entity
    {
        ITreasure Treasure { get; }

        public TreasureItem(int x, int y, Collider collider, Func<Room> CurrentRoom) : base(x, y, collider, CurrentRoom)
        {
        }
    }
}

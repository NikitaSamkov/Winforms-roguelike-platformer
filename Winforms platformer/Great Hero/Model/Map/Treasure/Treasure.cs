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
        public ITreasure Treasure { get; }
        public int ID => Treasure.ID;

        public TreasureItem(int x, int y, Collider collider, Func<Room> CurrentRoom, int treasureID) 
            : base(x, y, collider, CurrentRoom)
        {
            Treasure = TreasurePool.GetTreasureByID(0);
        }

        public void GiveToPlayer()
        {
            TreasurePool.GiveToPlayer(Treasure.ID);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winforms_platformer.Model;

namespace Winforms_platformer
{
    public interface ITreasure
    {
        int ID { get; }
        int Price { get; }
    }

    public class TreasureItem : Loot
    {
        public ITreasure Treasure { get; }

        public TreasureItem(int x, int y, Collider collider, Func<Room> CurrentRoom, int treasureID) 
            : base(x, y, collider, CurrentRoom)
        {
            Treasure = TreasurePool.GetTreasureByID(treasureID);
            ID = Treasure.ID;
        }

        public override void Pickup(Entity target)
        {
            TreasurePool.GiveToPlayer(Treasure.ID);
        }
    }
}

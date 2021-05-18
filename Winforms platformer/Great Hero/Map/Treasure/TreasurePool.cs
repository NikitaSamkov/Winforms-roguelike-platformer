using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winforms_platformer
{
    public static class TreasurePool
    {
        private static List<ITreasure> treasures = new List<ITreasure>
        {
            new AmuletOfFlying()
        };

        public static void GiveToPlayer(EntityRender target, int treasureID)
        {
            if (treasureID < treasures.Count)
            {
                var player = (Player)target.entity;
                player.treasures.Add(treasures[treasureID]);
                treasures[treasureID].Apply(target);
            }
        }

        public static ITreasure GetTreasureByID(int treasureID)
        {
            if (treasureID < treasures.Count)
                return treasures[treasureID];
            return null;
        }

        public static void SortPool() => treasures.OrderBy(treasure => treasure.GetID());
    }

    public class AmuletOfFlying : ITreasure
    {
        public int GetID()
        {
            return 0;
        }

        public void Apply(EntityRender target)
        {
            (target.entity as Player).flying = true;
        }
    }
}

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

        public static void GiveToPlayer(Player player, int treasureID)
        {
            if (treasureID < treasures.Count)
            {
                player.treasures.Add(treasures[treasureID]);
                treasures[treasureID].Enable(player);
            }
        }

        public static void RemoveFromPlayer(Player player, int treasureID)
        {
            for (var i = 0; i < player.treasures.Count; i++)
                if (player.treasures[i].ID == treasureID)
                {
                    player.treasures[i].Disable(player);
                    player.treasures.RemoveAt(i);
                    break;
                }
        }

        public static ITreasure GetTreasureByID(int treasureID)
        {
            if (treasureID < treasures.Count)
                return treasures[treasureID];
            return null;
        }

        public static void SortPool() => treasures.OrderBy(treasure => treasure.ID);
    }

    public class AmuletOfFlying : ITreasure
    {
        int ITreasure.ID { get => 0; }

        public void Enable(Player player)
        {
            player.flying = true;
        }

        public void Disable(Player player)
        {
            var count = 0;
            for (var i = 0; i < player.treasures.Count; i++)
                if (player.treasures[i].ID == 0)
                    count++;
            if (count < 2)
                player.flying = false;
        }
    }
}

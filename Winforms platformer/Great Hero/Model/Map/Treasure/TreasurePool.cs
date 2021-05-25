using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winforms_platformer.Model;

namespace Winforms_platformer
{
    public static class TreasurePool
    {
        private static List<ITreasure> treasures = new List<ITreasure>
        {
            new AmuletOfFlying()
        };

        public static void GiveToPlayer(int treasureID)
        {
            if (treasureID < treasures.Count)
            {
                Game.Player.treasures.Add(treasures[treasureID]);
                treasures[treasureID].Enable(Game.Player);
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

        public static void GetPrice()
        {
            var treasuresPrices = treasures.Select(e => e.Price).ToList();
            var sum = 0.00;
            var chances = new List<double>();
            var rand = new Random();
            for (var i = 0; i < treasuresPrices.Count; i++)
            {
                chances.Add(1 * (treasuresPrices.Max() - treasuresPrices[i] + 1) * 100 / treasuresPrices.Count);
                sum += chances[i];
            }
            for (var i = 0; i < treasuresPrices.Count; i++)
                if (rand.Next(1, 100) <= chances[i] * 100 / sum || i == treasuresPrices.Count - 1)
                {
                    Console.WriteLine(treasuresPrices[i]);
                    break;
                }
        }
    }

    public class AmuletOfFlying : ITreasure
    {
        int ITreasure.ID { get => 0; }

        int ITreasure.Price { get => 5; }

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

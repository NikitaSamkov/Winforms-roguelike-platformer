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
        private static Random Random = Game.Map.Random;

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

        private static int GetPrice()
        {
            var treasuresPrices = treasures.Select(e => e.Price).ToList();
            var sum = 0.00;
            var chances = new List<double>();
            for (var i = 0; i < treasuresPrices.Count; i++)
            {
                chances.Add(1 * (treasuresPrices.Max() - treasuresPrices[i] + 1) * 100 / treasuresPrices.Count);
                sum += chances[i];
            }
            for (var i = 0; i < treasuresPrices.Count; i++)
                if (Random.Next(100) + 1 <= chances[i] * 100 / sum || i == treasuresPrices.Count - 1)
                    return treasuresPrices[i];
            return 0;
        }

        private static ITreasure GetRandomItem(int price)
        {
            var items = treasures.Where(e => e.Price == price).ToList();
            if (items.Count == 0)
                return new NotFound();
            return items[Random.Next(items.Count)];
        }

        public static List<ITreasure> GenerateItems(int count)
        {
            var items = new List<ITreasure>();
            for (var i = 0; i < count; i++)
                items.Add(GetRandomItem(GetPrice()));
            return items;
        }
    }

    public class NotFound : ITreasure
    {
        int ITreasure.ID { get => -1; }

        int ITreasure.Price { get => -1; }

        public void Enable(Player player)
        {
            
        }

        public void Disable(Player player)
        {
            
        }
    }

    public class AmuletOfFlying : ITreasure
    {
        int ITreasure.ID { get => 0; }

        int ITreasure.Price { get => 10; }

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

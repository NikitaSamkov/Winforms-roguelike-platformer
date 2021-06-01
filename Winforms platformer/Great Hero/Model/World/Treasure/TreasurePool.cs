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
            new AmuletOfFlying(),
            new EternalBow(),
            new GravityFeather(),
            new AngryHearts(),
            new CowMan(),
            new AngryArrows()
        };

        public static void GiveToPlayer(int treasureID)
        {
            if (treasureID < treasures.Count && treasureID >= 0)
            {
                treasures[treasureID].Enable();
                Game.Player.treasures.Add(treasures[treasureID]);
            }
        }

        public static void RemoveFromPlayer(int treasureID)
        {
            for (var i = 0; i < Game.Player.treasures.Count; i++)
                if (Game.Player.treasures[i].ID == treasureID)
                {
                    Game.Player.treasures.RemoveAt(i);
                    treasures[treasureID].Disable();
                    break;
                }
        }

        public static ITreasure GetTreasureByID(int treasureID)
        {
            if (treasureID < treasures.Count)
                return treasures[treasureID];
            return new NotFoundedTreasure();
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
                return new NotFoundedTreasure();
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

    public class NotFoundedTreasure : ITreasure
    {
        int ITreasure.ID { get => -1; }

        int ITreasure.Price { get => -1; }

        public void Disable()
        {

        }

        public void Enable()
        {

        }
    }

    public class AmuletOfFlying : ITreasure
    {
        int ITreasure.ID { get => 0; }

        int ITreasure.Price { get => 10; }

        public void Disable()
        {

        }

        public void Enable()
        {

        }
    }

    public class EternalBow : ITreasure
    {
        private int playerAmmo;
        public int reloadTime { get => 25; }
        public int timer { get; protected set; }

        int ITreasure.ID { get => 1; }

        int ITreasure.Price { get => 9; }

        public void Disable()
        {
            if (Game.Player.treasures.Where(t => t == this).Count() == 0)
                Game.Player.Ammo = playerAmmo;
        }

        public void Enable()
        {
            if (Game.Player.Ammo != -1)
                playerAmmo = Game.Player.Ammo;
            Game.Player.Ammo = -1;
        }

        public void UpdateTimer()
        {
            if (timer > 0)
                timer--;
        }

        public void SetTimer() => timer = reloadTime;
    }

    public class GravityFeather : ITreasure
    {
        int ITreasure.ID { get => 2; }

        int ITreasure.Price { get => 1; }

        public void Disable()
        {
            Game.Map.ChangeGravitation(2);
        }

        public void Enable()
        {
            Game.Map.ChangeGravitation(0.5);
        }
    }

    public class AngryHearts : ITreasure
    {
        int ITreasure.ID { get => 3; }

        int ITreasure.Price { get => 3; }

        public void Disable()
        {
            if (Game.Player.treasures.Contains(this))
                HeartLoot.HealPower /= 2;
            else
                HeartLoot.HealPower /= -2;
        }

        public void Enable()
        {
            if (Game.Player.treasures.Contains(this))
                HeartLoot.HealPower *= 2;
            else
                HeartLoot.HealPower *= -2;
        }
    }

    public class CowMan : ITreasure
    {
        int ITreasure.ID { get => 4; }

        int ITreasure.Price { get => 3; }

        public void Disable()
        {
            RandomEnemyGenerator.RemoveFromGenerator(EnemyType.BigCow);
        }

        public void Enable()
        {
            RandomEnemyGenerator.AddToGenerator(EnemyType.BigCow);
        }
    }

    public class AngryArrows : ITreasure
    {
        int ITreasure.ID { get => 5; }

        int ITreasure.Price { get => 5; }

        public void Disable()
        {

        }

        public void Enable()
        {

        }
    }
}

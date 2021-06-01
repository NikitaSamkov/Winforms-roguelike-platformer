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
            new AngryArrows(),
            new Meat(),
            new Essentuki(),
            new EnergyDrink(),
            new HolyCross(),
            new RollerAmulet(),
            new Hammer()
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
        private double gravityMultiplier = 0.5;

        int ITreasure.ID { get => 2; }

        int ITreasure.Price { get => 1; }

        public void Disable()
        {
            Game.Map.ChangeGravitation(1 / gravityMultiplier);
        }

        public void Enable()
        {
            Game.Map.ChangeGravitation(gravityMultiplier);
        }
    }

    public class AngryHearts : ITreasure
    {
        private int multiplier = 2;

        int ITreasure.ID { get => 3; }

        int ITreasure.Price { get => 3; }

        public void Disable()
        {
            if (Game.Player.treasures.Contains(this))
                HeartLoot.HealPower /= multiplier;
            else
                HeartLoot.HealPower /= -multiplier;
        }

        public void Enable()
        {
            if (Game.Player.treasures.Contains(this))
                HeartLoot.HealPower *= multiplier;
            else
                HeartLoot.HealPower *= -multiplier;
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

    public class Meat : ITreasure
    {
        private int buff = 10;

        int ITreasure.ID { get => 6; }

        int ITreasure.Price { get => 8; }

        public void Disable()
        {
            Game.Player.damage -= buff;
        }

        public void Enable()
        {
            Game.Player.damage += buff;
        }
    }

    public class Essentuki : ITreasure
    {
        private int buff = 20;

        int ITreasure.ID { get => 7; }

        int ITreasure.Price { get => 6; }

        public void Disable()
        {
            Game.Player.MaxHP -= buff;
            if (Game.Player.HP > Game.Player.MaxHP)
                Game.Player.HP = Game.Player.MaxHP;
        }

        public void Enable()
        {
            Game.Player.MaxHP += buff;
            Game.Player.HP = Game.Player.MaxHP;
        }
    }

    public class EnergyDrink : ITreasure
    {
        private double multiplier = 1.5;

        int ITreasure.ID { get => 8; }

        int ITreasure.Price { get => 2; }

        public void Disable()
        {
            Game.Player.xSpeed = (int)(Game.Player.xSpeed / multiplier);
        }

        public void Enable()
        {
            Game.Player.xSpeed = (int)(Game.Player.xSpeed * multiplier);
        }
    }

    public class HolyCross : ITreasure
    {
        private int additionalTicks = 5;

        int ITreasure.ID { get => 9; }

        int ITreasure.Price { get => 4; }

        public void Disable()
        {
            Game.Player.damageInvincibility -= additionalTicks;
        }

        public void Enable()
        {
            Game.Player.damageInvincibility += additionalTicks;
        }
    }

    public class RollerAmulet : ITreasure
    {
        int ITreasure.ID { get => 10; }

        int ITreasure.Price { get => 2; }

        public void Disable()
        {

        }

        public void Enable()
        {

        }
    }

    public class Hammer : ITreasure
    {
        int ITreasure.ID { get => 11; }

        int ITreasure.Price { get => 1; }

        public void Disable()
        {

        }

        public void Enable()
        {

        }
    }

    public class BetterHearts : ITreasure
    {
        private double multiplier = 1.5;

        int ITreasure.ID { get => 12; }

        int ITreasure.Price { get => 7; }

        public void Disable()
        {
            HeartLoot.HealPower = (int)(HeartLoot.HealPower / multiplier);
        }

        public void Enable()
        {
            HeartLoot.HealPower = (int)(HeartLoot.HealPower * multiplier);
        }
    }
}

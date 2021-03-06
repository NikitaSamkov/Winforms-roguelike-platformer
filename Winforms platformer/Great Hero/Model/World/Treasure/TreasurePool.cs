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
        private static Random Random;

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
            new Hammer(),
            new BetterHearts(),
            new BetterAmmo(),
            new BestFriend(),
            new GiantRuby(),
            new StrongShield(),
            new MindPower(),
            new PlasmaBall(),
            new GhostForm(),
            new SlimeLayer()
        };

        public static void SetRandom(int seed)
        {
            Random = new Random(seed);
        }

        public static void SetRandom()
        {
            Random = Game.Map.Random;
        }

        public static void GiveToPlayer(int treasureID, Player player = null)
        {
            if (player == null)
                player = Game.Player;
            if (treasureID < treasures.Count && treasureID >= 0)
            {
                treasures[treasureID].Enable();
                player.Treasures.Add(treasures[treasureID]);
            }
        }

        public static void RemoveFromPlayer(int index, bool treasureID = true, Player player = null)
        {
            if (player == null)
                player = Game.Player;
            if (treasureID)
            {
                for (var i = 0; i < player.Treasures.Count; i++)
                    if (player.Treasures[i].ID == index)
                    {
                        player.Treasures.RemoveAt(i);
                        treasures[index].Disable();
                        break;
                    }
            }
            else
            {
                var id = player.Treasures[index].ID;
                player.Treasures.RemoveAt(index);
                treasures[id].Disable();
            }
        }

        public static ITreasure GetTreasureByID(int treasureID)
        {
            if (treasureID < treasures.Count)
                return treasures[treasureID];
            return new NotFoundedTreasure();
        }

        public static void SortPool() => treasures.OrderBy(treasure => treasure.ID);

        public static int GetPrice()
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

        public static ITreasure GetRandomItem(int price)
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
            {
                var item = GetRandomItem(GetPrice());
                if (item is GiantRuby)
                    item = TreasurePool.GetTreasureByID(0);
                items.Add(item);
            }
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
        public int reloadTime { get; protected set; }
        public int timer { get; protected set; }

        public int ID { get; protected set; }

        public int Price { get; protected set; }
        protected SecondaryWeapon type;

        public EternalBow()
        {
            reloadTime = 25;
            ID = 1;
            Price = 9;
            type = SecondaryWeapon.EternalBow;
        }

        public void Disable()
        {
            if (Game.Player.Treasures.Where(t => t == this).Count() == 0)
                Game.Player.SecondaryWeapons.Remove(type);
            Game.Player.CurrentSecondaryWeapon = Game.Player.SecondaryWeapons.Count - 1;
        }

        public void Enable()
        {
            if (Game.Player.Treasures.Where(t => t == this).Count() == 0)
                Game.Player.SecondaryWeapons.Add(type);
            Game.Player.CurrentSecondaryWeapon = Game.Player.SecondaryWeapons.Count - 1;
        }

        public void UpdateTimer()
        {
            if (timer > 0)
                timer--;
        }

        public virtual void SetTimer()
        {
            timer = reloadTime;
        }
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
            if (Game.Player.Treasures.Contains(this))
                HeartLoot.HealPower /= multiplier;
            else
                HeartLoot.HealPower /= -multiplier;
        }

        public void Enable()
        {
            if (Game.Player.Treasures.Contains(this))
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
        private int power = 10;

        int ITreasure.ID { get => 6; }

        int ITreasure.Price { get => 8; }

        public void Disable()
        {
            Game.Player.damage -= power;
        }

        public void Enable()
        {
            Game.Player.damage += power;
        }
    }

    public class Essentuki : ITreasure
    {
        private int power = 20;

        int ITreasure.ID { get => 7; }

        int ITreasure.Price { get => 7; }

        public void Disable()
        {
            Game.Player.MaxHP -= power;
            Game.Player.HP -= power;
        }

        public void Enable()
        {
            Game.Player.MaxHP += power;
            Game.Player.HP += power;
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

    public class BetterAmmo : ITreasure
    {
        private double multiplier = 2;

        int ITreasure.ID { get => 13; }

        int ITreasure.Price { get => 7; }

        public void Disable()
        {
            AmmoLoot.AmmoCount = (int)(AmmoLoot.AmmoCount / multiplier);
        }

        public void Enable()
        {
            AmmoLoot.AmmoCount = (int)(AmmoLoot.AmmoCount * multiplier);
        }
    }

    public class BestFriend : ITreasure
    {
        private int hpBuff = 10;
        private int damageBuff = 5;

        int ITreasure.ID { get => 14; }

        int ITreasure.Price { get => 6; }

        public void Disable()
        {
            Game.Player.MaxHP -= hpBuff;
            if (Game.Player.HP > Game.Player.MaxHP)
                Game.Player.HP = Game.Player.MaxHP;
            Game.Player.damage -= damageBuff;
        }

        public void Enable()
        {
            Game.Player.MaxHP += hpBuff;
            Game.Player.damage += damageBuff;
        }
    }

    public class GiantRuby : ITreasure
    {
        int ITreasure.ID { get => 15; }

        int ITreasure.Price { get => 5; }

        public void Disable()
        {
            Game.Death = true;
        }

        public void Enable()
        {
            Game.Win = true;
        }
    }

    public class StrongShield : ITreasure
    {
        private double power = 0.75;

        int ITreasure.ID { get => 16; }

        int ITreasure.Price { get => 7; }

        public void Disable()
        {
            Game.Player.hurtMultiplier /= power;
        }

        public void Enable()
        {
            Game.Player.hurtMultiplier *= power;
        }
    }

    public class MindPower : ITreasure
    {
        public int Range = 300;
        public int Power = 3;

        int ITreasure.ID { get => 17; }

        int ITreasure.Price { get => 4; }

        public void Disable()
        {

        }

        public void Enable()
        {

        }
    }

    public class PlasmaBall : EternalBow
    {
        public int PlasmaSpeed = 10;
        private int minReload = 5;
        private int maxReload = 50;
        private Room lastRoom = null;

        public PlasmaBall()
        {
            ID = 18;
            Price = 8;
            reloadTime = maxReload;
            type = SecondaryWeapon.PlasmaBall;
        }

        public override void SetTimer()
        {
            if (Game.Map.CurrentRoom() != lastRoom)
            {
                lastRoom = Game.Map.CurrentRoom();
                reloadTime = maxReload;
            }
            if (reloadTime > minReload)
                reloadTime -= 5;
            if (reloadTime < 0)
                reloadTime = 0;
            base.SetTimer();
        }
    }

    public class GhostForm : EternalBow
    {
        public int Power = 10;

        public GhostForm()
        {
            ID = 19;
            Price = 8;
            reloadTime = 45;
            type = SecondaryWeapon.GhostForm;
        }
    }

    public class SlimeLayer : ITreasure
    {
        private int power = 30;

        int ITreasure.ID { get => 20; }

        int ITreasure.Price { get => 7; }

        public void Disable()
        {
            Game.Player.MaxHP -= power;
            if (Game.Player.HP > Game.Player.MaxHP)
                Game.Player.HP = Game.Player.MaxHP;
        }

        public void Enable()
        {
            Game.Player.MaxHP += power;
        }
    }
}

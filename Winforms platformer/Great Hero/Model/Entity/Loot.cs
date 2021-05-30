using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winforms_platformer.Model
{
    public class Loot : Entity
    {
        public int ID { get; protected set; }
        public Loot(int x, int y, Collider collider, Func<Room> CurrentRoom) : base(x, y, collider, CurrentRoom)
        {
        }

        public virtual void Pickup(Entity target)
        {

        }
    }

    public class NotFoundedLoot : Loot
    {

        public NotFoundedLoot(int x, int y, Collider collider, Func<Room> CurrentRoom) : base(x, y, collider, CurrentRoom)
        {
            ID = -1;
        }

    }

    public class HeartLoot : Loot
    {
        public static int HealPower = 20;

        public HeartLoot(int x, int y, Collider collider, Func<Room> CurrentRoom) : base(x, y, collider, CurrentRoom)
        {
            ID = 0;
        }

        public override void Pickup(Entity target)
        {
            if (target.HP + HealPower <= target.MaxHP)
                target.HP += HealPower;
            else
                target.HP = target.MaxHP;
        }
    }

    public class AmmoLoot : Loot
    {
        public static int AmmoCount = 1;

        public AmmoLoot(int x, int y, Collider collider, Func<Room> CurrentRoom) : base(x, y, collider, CurrentRoom)
        {
            ID = 1;
        }

        public override void Pickup(Entity target)
        {
            if (target is Creature creature && (!(target is Player) && 
                !(target as Player).treasures.Contains(TreasurePool.GetTreasureByID(1))))
                creature.Ammo += AmmoCount;
        }
    }
}

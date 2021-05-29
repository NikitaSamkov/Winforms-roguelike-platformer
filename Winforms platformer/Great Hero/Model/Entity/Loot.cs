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
        public int HealPower;

        public HeartLoot(int x, int y, Collider collider, Func<Room> CurrentRoom) : base(x, y, collider, CurrentRoom)
        {
            HealPower = 20;
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
        public int AmmoCount;

        public AmmoLoot(int x, int y, Collider collider, Func<Room> CurrentRoom) : base(x, y, collider, CurrentRoom)
        {
            AmmoCount = 1;
            ID = 1;
        }

        public override void Pickup(Entity target)
        {
            if (target is Creature creature)
                creature.Ammo += AmmoCount;
        }
    }
}

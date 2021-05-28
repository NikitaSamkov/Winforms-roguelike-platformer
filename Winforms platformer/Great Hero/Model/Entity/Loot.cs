using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winforms_platformer.Model
{
    public class Loot : Entity
    {
        public int ID { get; }
        public Loot(int x, int y, Collider collider, Func<Room> CurrentRoom) : base(x, y, collider, CurrentRoom)
        {
        }

        public virtual void Pickup(Entity target)
        {

        }
    }

    public class HeartLoot : Loot
    {
        public int ID => 0;
        public int HealPower;

        public HeartLoot(int x, int y, Collider collider, Func<Room> CurrentRoom) : base(x, y, collider, CurrentRoom)
        {
            HealPower = 20;
        }

        public override void Pickup(Entity target)
        {
            target.hp += HealPower;
        }
    }

    public class AmmoLoot : Loot
    {
        public int ID => 0;
        public int AmmoCount;

        public AmmoLoot(int x, int y, Collider collider, Func<Room> CurrentRoom) : base(x, y, collider, CurrentRoom)
        {
            AmmoCount = 1;
        }

        public override void Pickup(Entity target)
        {
            if (target is Creature creature)
                creature.Ammo += AmmoCount;
        }
    }
}

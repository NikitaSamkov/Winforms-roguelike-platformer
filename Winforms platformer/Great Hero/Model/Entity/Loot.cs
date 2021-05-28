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

        public virtual void Pickup()
        {

        }
    }

    public class HeartLoot : Loot
    {
        public int ID => 0;
        public HeartLoot(int x, int y, Collider collider, Func<Room> CurrentRoom) : base(x, y, collider, CurrentRoom)
        {
        }
    }
}

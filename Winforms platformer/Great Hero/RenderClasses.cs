using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winforms_platformer
{
    class EntityRender
    {
        public Entity entity;
        public Sprite sprite;

        public EntityRender(Entity entity, Sprite entitySprite)
        {
            this.entity = entity;
            sprite = entitySprite;
        }
    }

    class CreatureRender
    {
        public Creature creature;
        public Sprite sprite;

        public CreatureRender(Creature creature, Sprite creatureSprite)
        {
            this.creature = creature;
            sprite = creatureSprite;
        }
    }

    class RoomRender
    {
        public Room room;
        public Sprite wallSprite;
        public Sprite groundSprite;
        public Sprite platformSprite;

        public RoomRender(Room room, Bitmap wall, Bitmap ground)
        {
            this.room = room;
            wallSprite = new Sprite(wall);
            groundSprite = new Sprite(ground);
        }
    }
}

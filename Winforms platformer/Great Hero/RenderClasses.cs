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
}

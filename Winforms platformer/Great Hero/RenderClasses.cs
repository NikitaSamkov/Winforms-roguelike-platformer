using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winforms_platformer
{
    public class EntityRender
    {
        public Entity entity;
        public Sprite sprite;

        public EntityRender(Entity entity, Sprite entitySprite)
        {
            this.entity = entity;
            sprite = entitySprite;
        }

        public void Update()
        {
            entity.status = sprite.currentStatus;
            entity.Move();
            sprite.StepFrame();
        }

        public void SetIdle()
        {
            if (sprite.currentStatus == Status.AttackMove)
                sprite.SetAttacking();
            if (sprite.currentStatus != Status.Attack)
                sprite.SetIdle();
        }

        public void SetMoving()
        {
            if (sprite.currentStatus == Status.Attack)
                sprite.SetAttackingMove();
            if (sprite.currentStatus != Status.AttackMove)
                sprite.SetMoving();
        }

        public void SetAttacking()
        {
            if (sprite.currentStatus == Status.Move)
                sprite.SetAttackingMove();
            if (sprite.currentStatus == Status.Idle)
                sprite.SetAttacking();
        }
    }

    public class RoomRender
    {
        public Room room;
        public Sprite wallSprite;
        public Sprite groundSprite;

        public RoomRender(Room room)
        {
            this.room = room;
            wallSprite = new Sprite(RoomBitmaps.Wall, new Size(800, 600));
            groundSprite = new Sprite(RoomBitmaps.Ground, new Size(800, 600));
        }

        public void ChangeRoom(Room room)
        {
            this.room = room;
        }
    }
}

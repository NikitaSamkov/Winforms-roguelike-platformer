﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winforms_platformer
{
    public class Creature : Entity
    {
        public bool flying { get; set; }
        public int bowStrenght { get; set; }
        protected int jumpStrength = 50;

        public Creature(int x, int y, Collider collider, Room room) 
            : base(x, y, collider, room)
        {
            bowStrenght = 75;
        }

        public override void Update()
        {
            if (status != Status.Idle && status != Status.Attack)
                switch (currentDirection)
                {
                    case Direction.Left:
                        x -= xSpeed;
                        break;
                    case Direction.Right:
                        x += xSpeed;
                        break;
                }
            if (!flying)
                MoveY();
            if (invincibility > 0)
                invincibility--;
            if (status == Status.Attack || status == Status.AttackMove)
            {
                var colliderX = ((int)currentDirection == 0) ?
                    collider.field.Width + collider.attackCollider.x + x :
                    x - collider.attackCollider.x - collider.attackCollider.field.Width;
                var colliderY = y + collider.attackCollider.y;
                foreach (var target in room.GetIntersectedEntities(collider, colliderX, colliderY))
                    target.Hurt(damage);
            }
        }

        public void Jump()
        {
            if (room.OnTheSurface(x, y + collider.field.Height, collider.field.Width) && ySpeed == 0)
            {
                ySpeed -= jumpStrength;
            }
        }

        //public IEnumerable<Point> GetJumpTrajectory()
        //{
        //    var trajectoryXSpeed = (currentDirection == Direction.Right) ? xSpeed : -xSpeed;
        //    var trajectoryYSpeed = ySpeed - jumpStrength;
        //    var trajectoryX = x + trajectoryXSpeed;
        //    var trajectoryY = y + trajectoryYSpeed;
        //    trajectoryYSpeed = getYSpeed(trajectoryX, trajectoryY, width, trajectoryYSpeed);
        //    while (!canJump(trajectoryX, trajectoryY, width))
        //    {
        //        yield return new Point(trajectoryX, trajectoryY);
        //        trajectoryYSpeed = getYSpeed(trajectoryX, trajectoryY, width, trajectoryYSpeed);
        //        trajectoryX += trajectoryXSpeed;
        //        trajectoryY += trajectoryYSpeed;
        //    }
        //}
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winforms_platformer.Model
{
    public class Boss : Enemy
    {
        private int attackCooldown;
        private int attackTimer;
        public List<BossHand> Hands;
        public BossStatus Status;

        public Boss(int x, int y, Collider collider, Func<Room> room, Player player) : base(x, y, collider, room, player)
        {
            HP = 500;
            MaxHP = HP;
            damage = 33;

            attackCooldown = 50;
            attackTimer = attackCooldown;

            Hands = new List<BossHand>()
            {
                new BossHand(BossZones.GetHandPosition(Zone.Left, BossHandStatus.Palm).X,
                BossZones.GetHandPosition(Zone.Left, BossHandStatus.Palm).Y, new Collider(Resources.Boss.PalmSize), CurrentRoom, player, damage, Zone.Left),

                new BossHand(BossZones.GetHandPosition(Zone.Right, BossHandStatus.Palm).X,
                BossZones.GetHandPosition(Zone.Right, BossHandStatus.Palm).Y, new Collider(Resources.Boss.PalmSize), CurrentRoom, player, damage, Zone.Right)
            };


            treasureDropID = -1;
            SetDropChances(0, 0, 0);
        }

        public override void Update()
        {
            if (attackTimer == attackCooldown / 2)
                RaiseHands();
            if (attackTimer == 0)
            {
                Hit();
                attackCooldown = 50 * HP / MaxHP;
                if (attackCooldown < 15)
                    attackCooldown = 15;
                attackTimer = attackCooldown + 1;
            }
            foreach (var hand in Hands)
                hand.Update();
            attackTimer--;
            if (invincibility > 0)
                invincibility--;
        }

        private void RaiseHands()
        {
            if (player.IntersectsWithBody(BossZones.GetRectangle(Zone.Left)))
                Hands[0].Rise(Zone.Left);
            else if (player.IntersectsWithBody(BossZones.GetRectangle(Zone.Right)))
                Hands[1].Rise(Zone.Right);
            else
            {
                Hands[0].Rise(Zone.CenterLeft);
                Hands[1].Rise(Zone.CenterRight);
            }

        }

        private void Hit()
        {
            if (Hands[0].HandStatus == BossHandStatus.Fist)
                Hands[0].Hit();
            if (Hands[1].HandStatus == BossHandStatus.Fist)
                Hands[1].Hit();
        }
    }

    public class BossHand : Entity
    {
        public BossHandStatus HandStatus;
        public Zone CurrentZone;
        private Point target;
        private int risingSpeed = 75;
        private int hittingSpeed = 100;
        private int speed;
        private Player player;

        public BossHand(int x, int y, Collider collider, Func<Room> CurrentRoom, Player player, int damage, Zone zone)
            : base(x, y, collider, CurrentRoom)
        {
            this.x = x;
            this.y = y;
            HandStatus = BossHandStatus.Palm;
            this.player = player;
            this.damage = damage;
            CurrentZone = zone;
            target = new Point(x, y);
            speed = risingSpeed;
        }

        public override void Update()
        {
            if (new Point(x, y) == BossZones.GetHandPosition(CurrentZone, BossHandStatus.Palm) && HandStatus == BossHandStatus.Fist)
            {
                if (player.IntersectsWithBody(BossZones.GetRectangle(CurrentZone)))
                    player.Hurt(damage);
                HandStatus = BossHandStatus.Palm;
            }
            x += MoveTo(x, target.X, speed);
            y += MoveTo(y, target.Y, speed);
        }

        private int MoveTo(int current, int target, int speed)
        {
            if (Math.Abs(target - current) > speed)
                if (target > current)
                    return risingSpeed;
                else
                    return -risingSpeed;
            return target - current;
        }

        public void Rise(Zone zone)
        {
            HandStatus = BossHandStatus.Fist;
            CurrentZone = zone;
            target = BossZones.GetHandPosition(CurrentZone, HandStatus);
            TeleportTo(x, y - 1);
            speed = risingSpeed;
        }

        public void Hit()
        {
            HandStatus = BossHandStatus.Fist;
            target = BossZones.GetHandPosition(CurrentZone, BossHandStatus.Palm);
            speed = hittingSpeed;
        }

        protected override void MoveY()
        {

        }
    }

    public static class BossZones
    {
        public static Rectangle[] ZoneToRectangle = new Rectangle[4];
        public static Point[,] ZoneAndStatusToPosition = new Point[4, 2];

        public static void Create()
        {
            ZoneToRectangle[(int)Zone.Left] = new Rectangle(new Point(0, 0), Resources.Boss.ZoneSize);
            ZoneToRectangle[(int)Zone.CenterLeft] = new Rectangle(new Point(Resources.Boss.ZoneSize.Width, 0), Resources.Boss.ZoneSize);
            ZoneToRectangle[(int)Zone.CenterRight] = new Rectangle(new Point(Resources.Boss.ZoneSize.Width, 0), Resources.Boss.ZoneSize);
            ZoneToRectangle[(int)Zone.Right] = new Rectangle(new Point(Resources.Boss.ZoneSize.Width * 2, 0), Resources.Boss.ZoneSize);

            var left = 0;
            var leftc = 200;
            var right = 600;
            var rightc = 400;
            var top = 0;
            var bottom = 419;

            ZoneAndStatusToPosition[(int)Zone.Left, (int)BossHandStatus.Fist] = new Point(left, top);
            ZoneAndStatusToPosition[(int)Zone.Left, (int)BossHandStatus.Palm] = new Point(left, bottom);
            ZoneAndStatusToPosition[(int)Zone.CenterLeft, (int)BossHandStatus.Fist] = new Point(leftc, top);
            ZoneAndStatusToPosition[(int)Zone.CenterLeft, (int)BossHandStatus.Palm] = new Point(leftc, bottom);
            ZoneAndStatusToPosition[(int)Zone.CenterRight, (int)BossHandStatus.Fist] = new Point(rightc, top);
            ZoneAndStatusToPosition[(int)Zone.CenterRight, (int)BossHandStatus.Palm] = new Point(rightc, bottom);
            ZoneAndStatusToPosition[(int)Zone.Right, (int)BossHandStatus.Fist] = new Point(right, top);
            ZoneAndStatusToPosition[(int)Zone.Right, (int)BossHandStatus.Palm] = new Point(right, bottom);
        }

        public static Rectangle GetRectangle(Zone zone) => ZoneToRectangle[(int)zone];

        public static Point GetHandPosition(Zone zone, BossHandStatus handStatus) => ZoneAndStatusToPosition[(int)zone, (int)handStatus];
    }
}

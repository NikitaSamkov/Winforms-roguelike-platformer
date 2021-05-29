using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winforms_platformer.Model;

namespace Winforms_platformer
{
    public class Enemy : Creature
    {
        protected Player player;
        protected Dictionary<LootType, int> dropChances = new Dictionary<LootType, int>();
        protected int treasureDropID = -1;
        protected int range = 400;
        protected int maxSpeed = 15;
        protected int minSpeed = 10;


        public Enemy(int x, int y, Collider collider, Func<Room> room,
            Player player)
            : base(x, y, collider, room)
        {
            this.player = player;
            HP = 20;
            MaxHP = 20;
            damage = 0;
            xSpeed = minSpeed;
            SetDropChances();
        }

        public override void Update()
        {
            MoveToPlayer();
            base.Update();
        }

        protected virtual void MoveToPlayer()
        {
            var distance = GetShotestDistanceToPlayer();
            if (distance > range)
            {
                if (status == Status.AttackMove || status == Status.Attack)
                    status = Status.Attack;
                else
                    status = Status.Idle;
                xSpeed = 0;
            }
            else
            {
                if (status == Status.AttackMove || status == Status.Attack)
                    status = Status.AttackMove;
                else
                    status = Status.Move;
                direction = (x - player.x >= 0) ? Direction.Left : Direction.Right;
                var neededX = (direction == Direction.Left) ?
                    player.x + player.collider.field.Width / 2 :
                    player.x - player.collider.field.Width / 2;

                if (distance > 150 && xSpeed < maxSpeed || distance <= 100 && xSpeed < minSpeed)
                    xSpeed++;
                else if (distance <= 100 && xSpeed > minSpeed)
                    xSpeed--;
                if (Math.Abs(x - neededX) < xSpeed)
                    xSpeed = Math.Abs(x - neededX);
                if (player.y + player.collider.field.Height < y + collider.field.Height && distance < 200)
                    Jump();
                if (player.y + player.collider.field.Height > y + collider.field.Height && distance <= range)
                    MoveDown(1);
            }
        }

        public virtual bool HitsPlayer()
        {
            return IntersectsWithBody(player);
        }

        protected virtual void SetDropChances()
        {

        }

        protected void SetDropChances(int heart, int ammo, int treasure)
        {
            dropChances[LootType.Heart] = heart;
            dropChances[LootType.Ammo] = ammo;
            dropChances[LootType.Treasure] = treasure;
        }

        public Loot GetDrop()
        {
            var random = new Random();
            foreach (var lootType in dropChances.Keys)
            {
                if (random.Next(1, 101) <= dropChances[lootType] * 50 / player.HP)
                    switch (lootType)
                    {
                        case LootType.Heart:
                            return new HeartLoot(x, y, new Collider(Resources.Loot.Size), CurrentRoom);
                        case LootType.Ammo:
                            return new AmmoLoot(x, y, new Collider(Resources.Loot.Size), CurrentRoom);
                        case LootType.Treasure:
                            return new TreasureItem(x, y, new Collider(Resources.Treasures.Size), CurrentRoom, treasureDropID);
                        default:
                            return new NotFoundedLoot(x, y, new Collider(Resources.Loot.Size), CurrentRoom);
                    }
            }
            return null;
        }

        public int GetShotestDistanceToPlayer()
        {
            return Math.Min(GetDistanceTo(player.x + player.collider.Left, player.y + player.collider.Top),
                   Math.Min(GetDistanceTo(player.x + player.collider.Left, player.y + player.collider.Bottom),
                   Math.Min(GetDistanceTo(player.x + player.collider.Right, player.y + player.collider.Top),
                   GetDistanceTo(player.x + player.collider.Right, player.y + player.collider.Bottom))));
        }
    }

    public class Slime : Enemy
    {
        public Slime(int x, int y, Collider collider, Func<Room> room, Player player) : base(x, y, collider, room, player)
        {
            HP = 10;
            MaxHP = HP;
            damage = 5;
            minSpeed = 5;
            maxSpeed = 7;
            xSpeed = minSpeed;
            treasureDropID = -1;
            range = 400;
            jumpStrength = 50;
            SetDropChances(10, 0, 0);
        }
    }

    public class Roller : Enemy
    {
        public Roller(int x, int y, Collider collider, Func<Room> room, Player player) : base(x, y, collider, room, player)
        {
            HP = 1;
            MaxHP = 1;
            damage = 10;
            minSpeed = 15;
            maxSpeed = 99;
            xSpeed = minSpeed;
            treasureDropID = -1;
            range = 1000;
            jumpStrength = 0;
            SetDropChances(5, 10, 0);
        }
    }

    public class Swordsman : Enemy
    {
        public Swordsman(int x, int y, Collider collider, Func<Room> room, Player player) : base(x, y, collider, room, player)
        {
            HP = 50;
            MaxHP = HP;
            damage = 10;
            minSpeed = 5;
            maxSpeed = 7;
            xSpeed = minSpeed;
            treasureDropID = -1;
            range = 500;
            jumpStrength = 30;
            SetDropChances(25, 25, 0);
        }

        public override void Update()
        {
            if (HitsAnybodyWithAttack(out var entities) && entities.Contains(player))
                status = Status.AttackMove;
            base.Update();
        }

        public override bool HitsPlayer()
        {
            return false;
        }
    }

    public class Archer : Enemy
    {
        private int reloadTime;
        private int reload;
        private Direction lastSafeDirection;
        private bool safe;
        public Archer(int x, int y, Collider collider, Func<Room> room, Player player) : base(x, y, collider, room, player)
        {
            HP = 5;
            MaxHP = HP;
            damage = 0;
            minSpeed = 5;
            maxSpeed = 10;
            xSpeed = minSpeed;
            treasureDropID = -1;
            range = 1000;
            jumpStrength = 60;
            reloadTime = 25;
            SetDropChances(0, 50, 0);
        }

        public override void Update()
        {
            MoveToPlayer();
            base.Update();
            if (reload == 0)
            {
                direction = (x - player.x >= 0) ? Direction.Left : Direction.Right;
                Shoot();
                reload = reloadTime;
            }
            else
                reload--;
            if (reload == 10 && Math.Abs(player.x - x) > 600)
                Jump();
        }

        protected override void MoveToPlayer()
        {
            var distance = GetShotestDistanceToPlayer();
            if (distance > 400)
            {
                direction = (x - player.x >= 0) ? Direction.Left : Direction.Right;
                status = Status.Idle;
                xSpeed = 0;
            }
            else if (distance <= 400 && distance >= 100)
            {
                status = Status.Move;
                direction = (x - player.x >= 0) ? Direction.Right : Direction.Left;
                safe = true;
                lastSafeDirection = direction;

                if (distance > 150 && xSpeed > minSpeed)
                    xSpeed--;

                if (player.y + player.collider.field.Height < y + collider.field.Height)
                    Jump();
                if (player.y + player.collider.field.Height > y + collider.field.Height)
                    MoveDown(1);
            }
            else if (distance <= 100)
            {
                if (safe)
                {
                    safe = false;
                    if (lastSafeDirection == Direction.Right)
                        lastSafeDirection = Direction.Left;
                    else
                        lastSafeDirection = Direction.Right;
                    direction = lastSafeDirection;
                }
                else
                    direction = lastSafeDirection;
                status = Status.Move;
                xSpeed++;
                if (player.y >= y)
                    Jump();
                else
                    MoveDown(1);
            }
        }

        public override bool HitsPlayer()
        {
            return false;
        }
    }
}

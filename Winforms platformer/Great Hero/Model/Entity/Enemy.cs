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
        public int difficulty { get; protected set; } // 1 - 5
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
                if (player.HP > 0 && random.Next(1, 101) <= dropChances[lootType] * player.MaxHP / (2 * player.HP))
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
            difficulty = 1;
            SetDropChances(10, 0, 0);
        }
    }

    public class Roller : Enemy
    {
        public Roller(int x, int y, Collider collider, Func<Room> room, Player player) : base(x, y, collider, room, player)
        {
            HP = 1;
            MaxHP = 1;
            damage = 20;
            minSpeed = 15;
            maxSpeed = 99;
            xSpeed = minSpeed;
            treasureDropID = -1;
            range = 1000;
            jumpStrength = 0;
            difficulty = 3;
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
            difficulty = 3;
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
            difficulty = 4;
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

    public class Magician : Enemy
    {
        private int power;
        private int lastPlayerY;
        public Magician(int x, int y, Collider collider, Func<Room> room, Player player) : base(x, y, collider, room, player)
        {
            HP = 10;
            MaxHP = HP;
            damage = 0;
            minSpeed = 0;
            maxSpeed = 0;
            xSpeed = minSpeed;
            treasureDropID = -1;
            range = 1000;
            jumpStrength = 0;
            power = 3;
            lastPlayerY = -1;
            difficulty = 1;
            SetDropChances(20, 20, 0);
        }

        protected override void MoveToPlayer()
        {
            var dx = 0;
            var dy = 0;
            if (lastPlayerY == -1)
                lastPlayerY = player.y + player.collider.field.Height;
            if (player.x > x)
                dx = -power;
            else
                dx = power;
            if (lastPlayerY > y + collider.field.Height)
                dy = -power;
            else
                dy = power;
            lastPlayerY += dy;
            player.TeleportTo(player.x + dx, lastPlayerY - player.collider.field.Height);
        }
    }

    public class SuperMagician : Enemy
    {
        private int cooldown;
        public SuperMagician(int x, int y, Collider collider, Func<Room> room, Player player) : base(x, y, collider, room, player)
        {
            var random = Game.Map.Random;
            HP = random.Next(1, 26);
            MaxHP = HP;
            damage = random.Next(25, 76);
            minSpeed = 0;
            maxSpeed = 0;
            xSpeed = minSpeed;
            treasureDropID = -1;
            range = 1000;
            jumpStrength = 0;
            difficulty = 3;
            cooldown = random.Next(75, 100);
            SetDropChances(random.Next(0, 101), random.Next(0, 101), 0);
        }

        protected override void MoveToPlayer()
        {
            if (cooldown == 0)
            {
                player.TeleportTo(x, y);
                cooldown = 100;
            }
            else
                cooldown--;
        }
    }

    public class BigCow : Enemy
    {
        private int timer;
        public BigCow(int x, int y, Collider collider, Func<Room> room, Player player) : base(x, y, collider, room, player)
        {
            HP = 100;
            MaxHP = HP;
            damage = 0;
            minSpeed = 0;
            maxSpeed = 15;
            xSpeed = minSpeed;
            treasureDropID = -1;
            range = 1000;
            jumpStrength = 100;
            difficulty = 1;
            SetDropChances(200, 0, 0);
        }

        protected override void MoveToPlayer()
        {
            if (timer == 0)
            {
                var random = Game.Map.Random;
                direction = (Direction)random.Next(0, 2);
                xSpeed = random.Next(0, 16);
                if (xSpeed == 0)
                    status = Status.Idle;
                else
                    status = Status.Move;
                if (random.Next(11) == 0)
                    Jump();
                if (random.Next(11) == 10)
                    MoveDown(1);
                timer = random.Next(0, 26);
            }
            else timer--;
        }
    }

    public class Ghost : Enemy
    {
        public Ghost(int x, int y, Collider collider, Func<Room> room, Player player) : base(x, y, collider, room, player)
        {
            HP = 50;
            MaxHP = HP;
            damage = 10;
            minSpeed = 0;
            maxSpeed = 0;
            xSpeed = minSpeed;
            treasureDropID = -1;
            range = 300;
            jumpStrength = 0;
            flying = true;
            difficulty = 2;
            SetDropChances(20, 0, 0);
        }

        protected override void MoveToPlayer()
        {
            if (player.x > x)
            {
                direction = Direction.Right;
                x += 2;
            }
            else
            {
                direction = Direction.Left;
                x -= 2;
            }
            if (player.y > y)
                y += 2;
            else
                y -= 2;
        }
    }

    public class InvisibleMan : Enemy
    {
        public InvisibleMan(int x, int y, Collider collider, Func<Room> room, Player player) : base(x, y, collider, room, player)
        {
            HP = 20;
            MaxHP = HP;
            damage = 10;
            minSpeed = 15;
            maxSpeed = 20;
            xSpeed = minSpeed;
            treasureDropID = -1;
            range = 1000;
            jumpStrength = 50;
            difficulty = 5;
            SetDropChances(50, 20, 0);
        }
    }

    public class Turret : Enemy
    {
        private int reloadTime = 15;
        private int timer = 0;

        public Turret(int x, int y, Collider collider, Func<Room> room, Player player) : base(x, y, collider, room, player)
        {
            HP = 1;
            MaxHP = HP;
            damage = 0;
            minSpeed = 0;
            maxSpeed = 0;
            xSpeed = minSpeed;
            treasureDropID = -1;
            range = 1000;
            jumpStrength = 0;
            ShootingPower = 20;
            difficulty = 5;
            SetDropChances(0, 50, 0);
        }

        protected override void MoveToPlayer()
        {
            if (timer == 0)
            {
                var dx = player.x - x;
                var dy = y - player.y;
                var angle = (int)(Math.Atan2(dy, dx) * 180 / Math.PI);
                Shoot(angle);
                if (reloadTime > 5)
                    reloadTime--;
                timer = reloadTime;
            }
            else timer--;
        }

        public override void Shoot(int angle = 0)
        {
            var plasma = new Plasma(x, y + collider.field.Height / 2,
                        new Collider(Resources.Plasma.IdleSize), CurrentRoom, angle, ShootingPower, ProjectileType.Enemy, this);
            plasma.status = Status.Move;
            CurrentRoom().ProjectilesList.Add(plasma);
        }
    }
}

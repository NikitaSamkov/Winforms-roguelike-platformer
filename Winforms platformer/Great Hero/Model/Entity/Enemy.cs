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
            treasureDropID = 10;
            range = 1000;
            jumpStrength = 0;
            difficulty = 2;
            SetDropChances(5, 10, 1);
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
            treasureDropID = 16;
            range = 500;
            jumpStrength = 30;
            difficulty = 3;
            SetDropChances(25, 25, 3);
        }

        public override void Update()
        {
            if (HitsAnybodyWithAttack(out var entities) && entities.Contains(player))
                status = Status.AttackMove;

            MoveToPlayer();

            if (status != Status.Idle && status != Status.Attack)
                switch (direction)
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
                player.Hurt(damage);
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
            treasureDropID = 1;
            range = 1000;
            jumpStrength = 60;
            reloadTime = 25;
            difficulty = 4;
            SetDropChances(0, 50, 2);
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
            treasureDropID = 9;
            range = 1000;
            jumpStrength = 0;
            difficulty = 3;
            cooldown = random.Next(75, 100);
            SetDropChances(random.Next(0, 101), random.Next(0, 101), random.Next(0, 16));
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
            treasureDropID = 9;
            range = 300;
            jumpStrength = 0;
            flying = true;
            difficulty = 2;
            SetDropChances(20, 0, 5);
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
            treasureDropID = 13;
            range = 1000;
            jumpStrength = 0;
            ShootingPower = 20;
            difficulty = 5;
            SetDropChances(0, 50, 5);
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

    public class Sticker : Enemy
    {
        public Status stickerStatus { get; private set; }

        public Sticker(int x, int y, Collider collider, Func<Room> room, Player player) : base(x, y, collider, room, player)
        {
            HP = 60;
            MaxHP = HP;
            damage = 5;
            minSpeed = 0;
            maxSpeed = 0;
            xSpeed = minSpeed;
            treasureDropID = -1;
            range = 0;
            jumpStrength = 0;
            difficulty = 3;
            damageInvincibility = 12;
            stickerStatus = Status.Idle;
            SetDropChances(50, 10, 0);
        }

        protected override void MoveToPlayer()
        {
            direction = (x - player.x >= 0) ? Direction.Left : Direction.Right;
            if (stickerStatus == Status.Idle && IntersectsWithBody(player))
            {
                stickerStatus = Status.Attack;
                flying = true;
                collider = new Collider(Resources.StickerAttack.IdleSize);
            }
            if (stickerStatus == Status.Attack)
                TeleportTo(player.x, player.y);
        }
    }

    public class Clone : Enemy
    {
        private int cloneCooldown = 5;
        private int timer;
        private int maxClones = 15;

        public Clone(int x, int y, Collider collider, Func<Room> room, Player player) : base(x, y, collider, room, player)
        {
            HP = 20;
            MaxHP = HP;
            damage = 5;
            minSpeed = 10;
            maxSpeed = 15;
            xSpeed = minSpeed;
            treasureDropID = -1;
            range = 500;
            jumpStrength = 50;
            difficulty = 3;
            timer = cloneCooldown;
            SetDropChances(0, 0, 0);
        }

        protected override void MoveToPlayer()
        {
            if (timer == 0)
            {
                var clones = CurrentRoom().EnemyList.Where(e => e is Clone).Count() + CurrentRoom().AdditionalEnemies.Where(e => e is Clone).Count();
                timer = cloneCooldown + clones + 1;
                if (clones < maxClones)
                    CreateNewClone();
            }
            if (timer == 5)
                status = Status.Idle;
            else if (timer > 5)
                base.MoveToPlayer();
            timer--;
        }

        private void CreateNewClone()
        {
            Clone clone = (Clone)this.MemberwiseClone();
            clone.x = x + collider.field.Width + collider.x;
            CurrentRoom().AdditionalEnemies.Add(clone);
        }
    }

    public class SuperRoller : Enemy
    {
        public SuperRoller(int x, int y, Collider collider, Func<Room> room, Player player) : base(x, y, collider, room, player)
        {
            HP = 2;
            MaxHP = 2;
            minSpeed = 20;
            damage = minSpeed;
            maxSpeed = 99;
            xSpeed = minSpeed;
            treasureDropID = 8;
            range = 1000;
            jumpStrength = 0;
            difficulty = 4;
            status = Status.Move;
            SetDropChances(10, 20, 10);
        }

        protected override void MoveToPlayer()
        {
            damage++;
            xSpeed++;
            direction = (x - player.x >= 0) ? Direction.Left : Direction.Right;
        }
    }

    public class Chameleon : Enemy
    {
        private int swapCooldown = 50;
        private int timer = 0;

        public Chameleon(int x, int y, Collider collider, Func<Room> room, Player player) : base(x, y, collider, room, player)
        {
            HP = 30;
            MaxHP = HP;
            damage = 10;
            minSpeed = 10;
            maxSpeed = 10;
            xSpeed = minSpeed;
            treasureDropID = -1;
            range = 1000;
            jumpStrength = 50;
            difficulty = 3;
            SetDropChances(13, 37, 0);
        }

        public override void Update()
        {
            if (timer == 0)
            {
                Swap();
                timer = swapCooldown + 1;
            }
            base.Update();
            timer--;
        }

        private void Swap()
        {
            switch (RandomEnemyGenerator.GetRandomEnemyType())
            {
                case EnemyType.Slime:
                    Resources.Chameleon.Swap(Resources.Slime);
                    collider = new Collider(Resources.Slime.IdleSize);
                    break;
                case EnemyType.Roller:
                    Resources.Chameleon.Swap(Resources.Roller);
                    collider = new Collider(Resources.Roller.IdleSize);
                    break;
                case EnemyType.SuperRoller:
                    Resources.Chameleon.Swap(Resources.SuperRoller);
                    collider = new Collider(Resources.SuperRoller.IdleSize);
                    break;
                case EnemyType.Swordsman:
                    Resources.Chameleon.Swap(Resources.Swordsman);
                    collider = new Collider(Resources.Swordsman.IdleSize);
                    break;
                case EnemyType.Archer:
                    Resources.Chameleon.Swap(Resources.Archer);
                    collider = new Collider(Resources.Archer.IdleSize);
                    break;
                case EnemyType.Magician:
                    Resources.Chameleon.Swap(Resources.Magician);
                    collider = new Collider(Resources.Magician.IdleSize);
                    break;
                case EnemyType.SuperMagician:
                    Resources.Chameleon.Swap(Resources.SuperMagician);
                    collider = new Collider(Resources.SuperMagician.IdleSize);
                    break;
                case EnemyType.BigCow:
                    Resources.Chameleon.Swap(Resources.BigCow);
                    collider = new Collider(Resources.BigCow.IdleSize);
                    break;
                case EnemyType.Ghost:
                    Resources.Chameleon.Swap(Resources.Ghost);
                    collider = new Collider(Resources.Ghost.IdleSize);
                    break;
                case EnemyType.InvisibleMan:
                    Resources.Chameleon.Swap(Resources.InvisibleMan);
                    collider = new Collider(Resources.InvisibleMan.IdleSize);
                    break;
                case EnemyType.Turret:
                    Resources.Chameleon.Swap(Resources.Turret);
                    collider = new Collider(Resources.Turret.IdleSize);
                    break;
                case EnemyType.Sticker:
                    Resources.Chameleon.Swap(Resources.Sticker);
                    collider = new Collider(Resources.Sticker.IdleSize);
                    break;
                case EnemyType.Clone:
                    Resources.Chameleon.Swap(Resources.Clone);
                    collider = new Collider(Resources.Clone.IdleSize);
                    break;
                case EnemyType.Dummy:
                case EnemyType.Chameleon:
                    Swap();
                    break;
            }
        }
    }
}

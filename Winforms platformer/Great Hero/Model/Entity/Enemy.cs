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
            var distance = Math.Min(GetDistanceTo(player.x + player.collider.Left, player.y + player.collider.Top),
                           Math.Min(GetDistanceTo(player.x + player.collider.Left, player.y + player.collider.Bottom),
                           Math.Min(GetDistanceTo(player.x + player.collider.Right, player.y + player.collider.Top),
                                    GetDistanceTo(player.x + player.collider.Right, player.y + player.collider.Bottom))));
            if (distance > range)
            {
                status = Status.Idle;
                xSpeed = 0;
            }
            else
            {
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
                if (player.y + player.collider.field.Height > y + collider.field.Height && distance < 200)
                    MoveDown(1);
            }
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
            treasureDropID = -1;
            range = 1000;
            jumpStrength = 0;
            SetDropChances(5, 10, 0);
        }
    }
}

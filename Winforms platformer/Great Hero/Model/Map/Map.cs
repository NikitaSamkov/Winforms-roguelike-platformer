using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winforms_platformer.View;

namespace Winforms_platformer.Model
{
    public class Map
    {
        private Player player;
        private List<Room> roomSamples;
        private List<Room> rooms;
        private int currentRoom;
        public Random Random;
        public readonly int seed;
        public Room CurrentRoom() => rooms[currentRoom];



        public Map(Player player, int? seed = null)
        {
            if (seed != null)
                this.seed = (int)seed;
            else
                this.seed = GenerateSeed();
            Random = new Random(this.seed);
            this.player = player;

            roomSamples = new List<Room>
            {
                new Room(RoomType.RegularRoom, player, new List<Platform>
                {
                    new Platform(200, 600, 350)
                }),
                new Room(RoomType.RegularRoom, player, new List<Platform>
                {
                    new Platform(100, 300, 350),
                    new Platform(500, 700, 350)
                }),
                new Room(RoomType.RegularRoom, player, new List<Platform>
                {
                    new Platform(100, 300, 400),
                    new Platform(500, 700, 400),
                    new Platform(300, 500, 300)
                }, null, 10),
                new Room(RoomType.RegularRoom, player, new List<Platform>(), null, 7, 250),
                new Room(RoomType.RegularRoom, player, new List<Platform>(), null, 1),
                new Room(RoomType.RegularRoom, player, new List<Platform> {new Platform(0, 800, 10) }, null, 0)
            };
        }

        public void Update()
        {
            ChangeRoom();

            foreach (var enemy in CurrentRoom().enemyList)
            {
                if (enemy.hp <= 0)
                {
                    CurrentRoom().enemyList.Remove(enemy);
                    break;
                }
                if (enemy.IntersectsWithBody(player))
                    player.Hurt(enemy.damage);
                enemy.Update();
            }

            foreach (var projectile in CurrentRoom().ProjectilesList)
            {
                if (projectile.y + projectile.collider.field.Height >= CurrentRoom().GroundLevel)
                {
                    CurrentRoom().ProjectilesList.Remove(projectile);
                    break;
                }
                var targets = CurrentRoom().GetIntersectedEntities(projectile);
                if (projectile.type == ProjectileType.Ally)
                    targets = targets.Where(target => target != player).ToList();
                if (projectile.type == ProjectileType.Enemy)
                    targets = targets.Where(target => target == player).ToList();
                if (targets.Count > 0)
                {
                    targets
                    .OrderBy(target => target.GetDistanceTo(projectile.x, projectile.y))
                    .First()
                    .hp -= projectile.damage;
                    CurrentRoom().ProjectilesList.Remove(projectile);
                    break;
                }
                projectile.Update();
            }

            foreach (var treasure in CurrentRoom().TreasuresList)
            {
                if (treasure.IntersectsWithBody(player))
                {
                    treasure.GiveToPlayer();
                    CurrentRoom().TreasuresList.Remove(treasure);
                    break;
                }
                treasure.Update();
            }
        }

        private int GenerateSeed()
        {
            var r = new Random();
            return r.Next(10000);
        }

        public void GenerateRooms(int roomsCount = 10)
        {
            var treasures = TreasurePool.GenerateItems(roomsCount / 3);
            currentRoom = 0;
            rooms = new List<Room>();
            rooms.Add(new Room(RoomType.StartingRoom, player));
            if (roomSamples.Count != 0)
                for (var i = 0; i < roomsCount - 1; i++)
                {
                    if ((i - 1) % 3 == 0)
                        rooms.Add(new Room(RoomType.TreasureRoom, player, new List<Platform>() 
                        { new Platform(350, 450, 350) }, new List<TreasureItem> 
                        { new TreasureItem(363, 250, new Collider(Resources.Treasures.Size), CurrentRoom,  
                        treasures[(i - 1) / 3].ID)}));
                    else
                        rooms.Add(roomSamples[Random.Next(roomSamples.Count)]);
                }
        }

        public void ChangeRoom()
        {
            if (player.x < 0)
            {
                if (CanPlayerGoToPreviousRoom())
                    GoToPrevious();
                else
                {
                    player.TeleportTo(0);
                    player.status = Status.Idle;
                }
            }
            if (player.x + player.collider.field.Width > Game.WindowSize.Width)
            {
                if (CanPlayerGoToNextRoom())
                    GoToNext();
                else
                {
                    player.TeleportTo(Game.WindowSize.Width - player.collider.field.Width);
                    player.status = Status.Idle;
                }
            }
        }

        public void GoToNext()
        {
            player.TeleportTo(0, rooms[currentRoom + 1].GroundLevel - (CurrentRoom().GroundLevel - player.y));
            currentRoom++;
        }

        public void GoToPrevious()
        {
            player.TeleportTo(Game.WindowSize.Width - player.collider.field.Width,
                rooms[currentRoom - 1].GroundLevel - (CurrentRoom().GroundLevel - player.y));
            currentRoom--;
        }

        public bool IsCurrentRoomLast() => currentRoom == rooms.Count - 1;

        public bool IsCurrentRoomFirst() => currentRoom <= 0;

        public bool CanPlayerGoToNextRoom() => !IsCurrentRoomLast() && CurrentRoom().enemyList.Count == 0;
        public bool CanPlayerGoToPreviousRoom() => !IsCurrentRoomFirst() && CurrentRoom().enemyList.Count == 0;
    }

}

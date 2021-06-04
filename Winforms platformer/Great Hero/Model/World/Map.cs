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
        private List<Room> roomTemplates;
        public List<Room> rooms;
        private int currentRoom;
        public Random Random;
        public readonly int seed;
        public Room CurrentRoom() => rooms[currentRoom];

        public Map(int? seed = null)
        {
            if (seed != null)
                this.seed = (int)seed;
            else
                this.seed = GenerateSeed();
            Random = new Random(this.seed);
            player = Game.Player;
        }

        public void SetRoomTemplates()
        {
            roomTemplates = new List<Room>
            {
                RoomGenerator.PictireToRoom(
@"9
         
---- ----
         
   ---   
  *   *  
- - - - -
         
#########", 486),

                RoomGenerator.PictireToRoom(
@"9
    *    =250
  -----  =350  
#########", 486),

                RoomGenerator.PictireToRoom(
@"9
  * * *  =250
 --- --- =350
#########", 486),

                RoomGenerator.PictireToRoom(
@"9
    *    =150
  -----  =300
  *   *  =350
 --   -- =400
#########", 486, 10),

                RoomGenerator.PictireToRoom(
@"9


- - - - -
 *     *
 - - - -
    *
- - - - -

 - - - -

- - - - -

#########", -1, 10),

                RoomGenerator.PictireToRoom(
@"9
 *  *  *
---------
-       -
-       -
-       -
-       -
-       -

#########", 486),

                RoomGenerator.PictireToRoom(
@"9

    *   *
---   ---=250
  -----  =350
#########", 486),

                RoomGenerator.PictireToRoom(
@"9
    * * *=250
---------=350
#########", 486),

                RoomGenerator.PictireToRoom(
@"9
*   *   *
-   -   -
-   -   -
-   -   -
-   -   -
-   -   -
-   -   -
-   -   -
#########", 486),

                RoomGenerator.PictireToRoom(
@"9
    * * *
-        
 -
  -
   -
    -
     -
      -
       -
        -
#########", 486),

                RoomGenerator.PictireToRoom(
@"9
        -
       -
      -
     -
    -
   -
  -
 -     
-      **
#########", 486),

                RoomGenerator.PictireToRoom(
@"5
 * *
- - -=325
 - - =350
#####", 486)

            };
        }

        public void Update()
        {
            if (Game.Boss.HP <= 0 && CurrentRoom().LootList.Where(t => t.ID == 15).Count() == 0)
                CurrentRoom().LootList.Add(Game.Boss.GetDrop());

            ChangeRoom();

            if (CurrentRoom().Type == RoomType.BossRoom && Game.Boss.HP > 0) 
                Game.Boss.Update();

            foreach (var enemy in CurrentRoom().EnemyList)
            {
                if (enemy.HP <= 0)
                {
                    var drop = enemy.GetDrop();
                    if (drop != null)
                        CurrentRoom().LootList.Add(drop);
                    CurrentRoom().EnemyList.Remove(enemy);
                    break;
                }
                if (enemy.HitsPlayer())
                    player.Hurt(enemy.damage);
                enemy.Update();

                if (player.treasures.Contains(TreasurePool.GetTreasureByID(17)))
                {
                    var treasure = TreasurePool.GetTreasureByID(17) as MindPower;
                    var dx = (enemy.x > player.x) ? 1 : -1;
                    var dy = (enemy.y + enemy.collider.field.Height > player.y + player.collider.field.Height) ? 1 : -1;
                    for (var i = 0; i < treasure.Power; i++)
                        if (enemy.GetShotestDistanceToPlayer() < treasure.Range * (treasure.Power - i) / treasure.Power)
                            enemy.TeleportTo(enemy.x + dx, enemy.y + dy);
                }    

                if (enemy.x + enemy.collider.field.Width > Game.WindowSize.Width)
                {
                    enemy.TeleportTo(Game.WindowSize.Width - enemy.collider.field.Width);
                    enemy.status = Status.Idle;
                }
                if (enemy.x < 0)
                {
                    enemy.TeleportTo(0);
                    enemy.status = Status.Idle;
                }
                if (enemy.y < 0)
                {
                    enemy.TeleportTo(enemy.x, 0);
                    enemy.FreezeInAir();
                }
            }

            foreach (var projectile in CurrentRoom().ProjectilesList)
            {
                if (projectile.y + projectile.collider.field.Height >= CurrentRoom().GroundLevel)
                {
                    CurrentRoom().ProjectilesList.Remove(projectile);
                    break;
                }
                var targets = CurrentRoom().GetIntersectedEntities(projectile).Where(t => t != projectile.Sender).ToList();
                if (projectile.type == ProjectileType.Enemy && !player.treasures.Contains(TreasurePool.GetTreasureByID(5)))
                    targets = targets.Where(target => target == player).ToList();
                if (targets.Count > 0)
                {
                    var target = targets
                    .OrderBy(t => t.GetDistanceTo(projectile.x, projectile.y))
                    .First();
                    if (target is Player)
                        target.Hurt(projectile.damage);
                    else
                        target.HP -= projectile.damage;
                    CurrentRoom().ProjectilesList.Remove(projectile);
                    break;
                }
                projectile.Update(); 
                if (projectile.y < 0)
                {
                    projectile.TeleportTo(projectile.x, 0);
                    projectile.FreezeInAir();
                }
            }

            foreach (var loot in CurrentRoom().LootList)
            {
                var entities = CurrentRoom().GetIntersectedEntities(loot);
                Entity target = null;
                if (player.treasures.Contains(TreasurePool.GetTreasureByID(3)) && loot is HeartLoot)
                    target = entities.FirstOrDefault();
                else if (loot.IntersectsWithBody(player))
                    target = player;
                if (target != null)
                {
                    loot.Pickup(target);
                    CurrentRoom().LootList.Remove(loot);
                    break;
                }
                loot.Update();
            }

            if (player.y < 0 && !(player.treasures.Contains(TreasurePool.GetTreasureByID(11))))
            {
                player.TeleportTo(player.x, 0);
                player.FreezeInAir();
            }
            if (CurrentRoom().AdditionalEnemies.Count != 0 && CurrentRoom().EnemyList.Count + CurrentRoom().AdditionalEnemies.Count < 32)
            {
                CurrentRoom().EnemyList = CurrentRoom().EnemyList.Concat(CurrentRoom().AdditionalEnemies).ToList();
                CurrentRoom().AdditionalEnemies = new List<Enemy>();
            }
        }

        private int GenerateSeed()
        {
            var r = new Random();
            return r.Next(10000);
        }

        public void GenerateMap(int roomsCount = 10)
        {
            var treasures = TreasurePool.GenerateItems(roomsCount / 3);
            currentRoom = 0;
            rooms = new List<Room>();
            rooms.Add(new Room(RoomType.StartingRoom));
            roomsCount = Math.Min(roomsCount - 1, roomTemplates.Count + roomTemplates.Count / 2);
            if (roomTemplates.Count != 0)
                for (var i = 0; i < roomsCount; i++)
                {
                    if (i == roomsCount - 1)
                    {
                        rooms.Add(new Room(RoomType.BossRoom, new List<Platform> { new Platform(0, 200, 350), new Platform(600, 800, 350) }));
                        break;
                    }
                    if ((i - 1) % 3 == 0)
                        rooms.Add(new Room(RoomType.TreasureRoom, new List<Platform>()
                        { new Platform(350, 450, 350) }, new List<Loot>
                        { new TreasureItem(363, 250, new Collider(Resources.Treasures.Size), CurrentRoom,
                        treasures[(i - 1) / 3].ID)}));
                    else
                    {
                        var template = Random.Next(roomTemplates.Count);
                        rooms.Add(roomTemplates[template]);
                        roomTemplates.RemoveAt(template);
                    }
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
            if (CurrentRoom().EnemySpots.Count != 0)
                SpawnEnemies();
        }

        public void GoToPrevious()
        {
            player.TeleportTo(Game.WindowSize.Width - player.collider.field.Width,
                rooms[currentRoom - 1].GroundLevel - (CurrentRoom().GroundLevel - player.y));
            currentRoom--;
            if (CurrentRoom().EnemySpots.Count != 0)
                SpawnEnemies();
        }

        private void SpawnEnemies()
        {
            CurrentRoom().EnemySpots.Reverse();
            var optimalDifficulty = 3 * CurrentRoom().EnemySpots.Count + 1;
            foreach (var spot in CurrentRoom().EnemySpots)
            {
                var enemy = RandomEnemyGenerator.GetRandomEnemy(spot.X, spot.Y);
                if (optimalDifficulty - enemy.difficulty >= 0)
                {
                    CurrentRoom().EnemyList.Add(enemy);
                    optimalDifficulty -= enemy.difficulty;
                }
            }
            CurrentRoom().EnemySpots = new List<System.Drawing.Point>();
        }

        public bool IsCurrentRoomLast() => currentRoom == rooms.Count - 1;

        public bool IsCurrentRoomFirst() => currentRoom <= 0;

        public bool CanPlayerGoToNextRoom() => !IsCurrentRoomLast() && CurrentRoom().EnemyList.Count == 0 && CurrentRoom().Type != RoomType.BossRoom;
        public bool CanPlayerGoToPreviousRoom() => !IsCurrentRoomFirst() && CurrentRoom().EnemyList.Count == 0 && CurrentRoom().Type != RoomType.BossRoom;

        public void ChangeGravitation(double multiplier)
        {
            foreach (var room in rooms)
            {
                room.gForce = (int)(room.gForce * multiplier);
                if (room.gForce == 0)
                    room.gForce = 1;
            }
        }
    }

}

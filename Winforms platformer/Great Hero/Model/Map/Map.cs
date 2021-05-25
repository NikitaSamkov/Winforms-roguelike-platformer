﻿using System;
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
        private Random roomSequenceRandom;
        private List<Room> rooms;
        private int currentRoom;
        public readonly int seed;
        public Room CurrentRoom => rooms[currentRoom];



        public Map(Player player, int? seed = null)
        {
            if (seed != null)
                this.seed = (int)seed;
            else
                this.seed = GenerateSeed();
            roomSequenceRandom = new Random(this.seed);
            this.player = player;

            roomSamples = new List<Room>
            {
                new Room(RoomType.EnemyRoom, new List<Platform>
                {
                    new Platform(200, 600, 350)
                }, player),
                new Room(RoomType.EnemyRoom, new List<Platform>
                {
                    new Platform(100, 300, 350),
                    new Platform(500, 700, 350)
                }, player),
                new Room(RoomType.EnemyRoom, new List<Platform>
                {
                    new Platform(100, 300, 400),
                    new Platform(500, 700, 400),
                    new Platform(300, 500, 300)
                }, player, 10),
                new Room(RoomType.EnemyRoom, new List<Platform>(), player, 7, 250),
                new Room(RoomType.EnemyRoom, new List<Platform>(), player, 1),
                new Room(RoomType.EnemyRoom, new List<Platform> {new Platform(0, 800, 10) }, player, 0)
            };
        }

        public void Update()
        {
            ChangeRoom();

            foreach (var enemy in CurrentRoom.enemyList)
            {
                if (enemy.hp <= 0)
                {
                    CurrentRoom.enemyList.Remove(enemy);
                    break;
                }
                enemy.Update();
            }

            foreach (var projectile in CurrentRoom.ProjectilesList)
            {
                if (projectile.y + projectile.collider.field.Height >= CurrentRoom.groundLevel)
                {
                    CurrentRoom.ProjectilesList.Remove(projectile);
                    break;
                }
                var targets = CurrentRoom.GetIntersectedEntities(projectile);
                if (projectile.type == ProjectileType.Ally)
                    targets = targets.Where(target => target != player).ToList();
                if (targets.Count > 0)
                {
                    targets
                    .Where(target => target != player)
                    .OrderBy(target => target.GetDistanceTo(projectile.x, projectile.y))
                    .First()
                    .hp -= projectile.damage;
                    CurrentRoom.ProjectilesList.Remove(projectile);
                    break;
                }
                projectile.Update();
            }
        }

        private int GenerateSeed()
        {
            var r = new Random();
            return r.Next(10000);
        }

        public void GenerateRooms(int roomsCount = 9)
        {
            currentRoom = 0;
            rooms = new List<Room>();
            rooms.Add(new Room(RoomType.StartingRoom, new List<Platform>(), player));
            if (roomSamples.Count != 0)
                for (var i = 0; i < roomsCount - 1; i++)
                    rooms.Add(roomSamples[roomSequenceRandom.Next(roomSamples.Count)]);
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
            if (player.x + player.collider.field.Width > Game.WindowWidth)
            {
                if (CanPlayerGoToNextRoom())
                    GoToNext();
                else
                {
                    player.TeleportTo(Game.WindowHeight - player.collider.field.Width);
                    player.status = Status.Idle;
                }
            }
        }

        public void GoToNext()
        {
            player.TeleportTo(0, rooms[currentRoom + 1].groundLevel - (CurrentRoom.groundLevel - player.y));
            currentRoom++;
            player.ChangeRoom(CurrentRoom);
            (GameRender.Renders[0] as RoomRender).ChangeRoom(CurrentRoom);

        }

        public void GoToPrevious()
        {
            player.TeleportTo(Game.WindowWidth - player.collider.field.Width, 
                rooms[currentRoom + 1].groundLevel - (CurrentRoom.groundLevel - player.y));
            currentRoom--;
            player.ChangeRoom(CurrentRoom);
            (GameRender.Renders[0] as RoomRender).ChangeRoom(CurrentRoom);
        }

        public bool IsCurrentRoomLast() => currentRoom == rooms.Count - 1;

        public bool IsCurrentRoomFirst() => currentRoom <= 0;

        public bool CanPlayerGoToNextRoom() => !IsCurrentRoomLast() && CurrentRoom.enemyList.Count == 0;
        public bool CanPlayerGoToPreviousRoom() => !IsCurrentRoomFirst() && CurrentRoom.enemyList.Count == 0;
    }
}
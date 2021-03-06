using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Winforms_platformer.View;

namespace Winforms_platformer.Model
{
    public static class Game
    {
        public static Player Player;
        public static Boss Boss;
        public static Map Map;
        public static Size WindowSize = new Size();
        public static Dictionary<Keys, Action> KeyBindings = new Dictionary<Keys, Action>();
        public static Keys lastKey;
        public static bool Death;
        public static bool Win;
        public static bool DeveloperToolsON = false;
        private static int superSecret;

        public static void Create()
        {
            Player = new Player(150, 150, new Collider(Res.Player.IdleSize, 0, 0,
                new Collider(Res.Player.AttackRange, -10, 0)), null);
            Map = new Map();
            TreasurePool.SetRandom();
            RandomEnemyGenerator.Start();
            Map.SetRoomTemplates();
            Map.GenerateMap();
            Player.CurrentRoom = Map.CurrentRoom;
            TreasurePool.SortPool();
            BossZones.Create();
            Boss = new Boss(150, 4, new Collider(Res.Boss.BodySize), Map.CurrentRoom, 500);
            Death = false;
            Win = false;
        }

        public static void Update()
        {
            if (!(Death || Win))
                Map.Update();
        }

        public static void SetWindowSize(int width, int height)
        {
            WindowSize.Width = width;
            WindowSize.Height = height;
        }

        public static void TryThrowTreasure(int mouseX, int mouseY)
        {
            if (TryGetTreasureIndex(mouseX, mouseY, out var i))
            {
                var x = (Player.direction == Direction.Right) ? Player.x + Player.collider.field.Width + 10 : 
                    Player.x - Res.Treasures.Size.Width - 10;
                var treasure = new TreasureItem(x, Player.y,
                    new Collider(Res.Treasures.Size), Map.CurrentRoom, Player.Treasures[i].ID);
                TreasurePool.RemoveFromPlayer(i, false);
                Map.CurrentRoom().LootList.Add(treasure);
            }
        }

        private static bool TryGetTreasureIndex(int mouseX, int mouseY, out int index)
        {
            index = 0;
            if (mouseX < WindowSize.Width - 150 || mouseY < 50)
                return false;
            var column = (mouseX - WindowSize.Width + 150) / 50;
            var row = (mouseY - 50) / 50;
            index = 3 * row + column;
            if (index < Player.Treasures.Count)
                return true;
            return false;
        }

        public static void SetKeyBindings()
        {
            KeyBindings[Keys.Left] = () =>
            {
                lastKey = Keys.Left;
                Player.MoveTo(Direction.Left);
                if (Player.status == Status.Attack)
                    Player.status = Status.AttackMove;
                if (Player.status != Status.AttackMove)
                    Player.status = Status.Move;
            };
            KeyBindings[Keys.A] = () =>
            {
                lastKey = Keys.A;
                Player.MoveTo(Direction.Left);
                if (Player.status == Status.Attack)
                    Player.status = Status.AttackMove;
                if (Player.status != Status.AttackMove)
                    Player.status = Status.Move;
            };

            KeyBindings[Keys.Right] = () =>
            {
                lastKey = Keys.Right;
                Player.MoveTo(Direction.Right);
                if (Player.status == Status.Attack)
                    Player.status = Status.AttackMove;
                if (Player.status != Status.AttackMove)
                    Player.status = Status.Move;
            };

            KeyBindings[Keys.D] = () =>
            {
                lastKey = Keys.D;
                Player.MoveTo(Direction.Right);
                if (Player.status == Status.Attack)
                    Player.status = Status.AttackMove;
                if (Player.status != Status.AttackMove)
                    Player.status = Status.Move;
            };

            KeyBindings[Keys.Up] = KeyBindings[Keys.Space] = () =>
            {
                if (Player.Treasures.Contains(TreasurePool.GetTreasureByID(0)))
                    Player.MoveUp();
                else
                    Player.Jump();
            };
            KeyBindings[Keys.W] = KeyBindings[Keys.Up];

            KeyBindings[Keys.Down] = () =>
            {
                if (Player.Treasures.Contains(TreasurePool.GetTreasureByID(0)))
                    Player.MoveDown();
                else
                    Player.MoveDown(1);
            };
            KeyBindings[Keys.S] = KeyBindings[Keys.Down];

            KeyBindings[Keys.K] = () => Console.WriteLine(Map.seed);

            KeyBindings[Keys.E] = () =>
            {
                if (Player.status != Status.Attack && Player.status != Status.AttackMove)
                    if (Player.status == Status.Idle)
                        Player.status = Status.Attack;
                    else
                        Player.status = Status.AttackMove;
            };

            KeyBindings[Keys.Q] = () => Player.Shoot();

            KeyBindings[Keys.D0] = () =>
            {
                for (var i = 0; i < Player.Treasures.Count; i++)
                    TreasurePool.RemoveFromPlayer(i, false);
                Create();
                GameRender.Create();
            };

            KeyBindings[Keys.R] = () =>
            Player.CurrentSecondaryWeapon = (Player.CurrentSecondaryWeapon + 1) % Player.SecondaryWeapons.Count;

            KeyBindings[Keys.Z] = () =>
            {
                if (DeveloperToolsON)
                    DeveloperToolsON = false;
                else
                    DeveloperToolsON = true;
            };

            var treasureID = TreasurePool.GetRandomItem(TreasurePool.GetPrice()).ID;
            //var treasureID = 20;

            KeyBindings[Keys.D1] = () =>
            {
                if (DeveloperToolsON)
                    Map.CurrentRoom().LootList.Add(
                    new TreasureItem(50, 0, new Collider(Res.Treasures.Size), Map.CurrentRoom, treasureID));
                treasureID = TreasurePool.GetRandomItem(TreasurePool.GetPrice()).ID;
            };


            KeyBindings[Keys.D2] = () =>
            {
                if (DeveloperToolsON)
                    TreasurePool.RemoveFromPlayer(treasureID);
            };

            KeyBindings[Keys.D3] = () =>
            {
                if (DeveloperToolsON)
                    Map.CurrentRoom().LootList.Add(
                new HeartLoot(Player.x - 50, Player.y, new Collider(Res.Loot.Size), Map.CurrentRoom));
            };

            KeyBindings[Keys.D4] = () =>
            {
                if (DeveloperToolsON)
                    Map.CurrentRoom().LootList.Add(
                new AmmoLoot(Player.x - 50, Player.y, new Collider(Res.Loot.Size), Map.CurrentRoom));
            };

            KeyBindings[Keys.D5] = () =>
            {
                if (DeveloperToolsON)
                    Map.CurrentRoom().EnemyList.Add(RandomEnemyGenerator.GetEnemy(Player.x - 50, Player.y - 50, EnemyType.Slime));
            };

            KeyBindings[Keys.T] = () =>
            {
                if (DeveloperToolsON)
                {
                    foreach (var treasureRoom in Map.rooms.Where(r => r.LootList.Count != 0))
                        foreach (var loot in treasureRoom.LootList.Where(l => l is TreasureItem))
                            Console.Write(loot.ID + "(" + (loot as TreasureItem).Treasure.Price + ")" + " ");
                    Console.WriteLine();
                }
            };

            KeyBindings[Keys.O] = () =>
            {
                if (superSecret == 0)
                    superSecret = 1;
                else
                    superSecret = 0;
            };

            KeyBindings[Keys.M] = () =>
            {
                if (superSecret == 1)
                    superSecret = 2;
                else
                    superSecret = 0;
            };

            KeyBindings[Keys.G] = () =>
            {
                if (superSecret == 2 && Map.CurrentRoom().Type != RoomType.BossRoom)
                    Res.Boss.Body = Res.Boss.Secret;
                else
                    superSecret = 0;
            };
        }
    }
}

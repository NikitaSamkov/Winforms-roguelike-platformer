using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        public static bool GameOver = false;
        public static bool DeveloperToolsON = false;
        private static int superSecret;

        static Game()
        {
            Player = new Player(150, 150, new Collider(Resources.Player.IdleSize, 0, 0,
                new Collider(Resources.Player.AttackRange, -10, 0)), null);
            Map = new Map();
            RandomEnemyGenerator.Start();
            Map.SetRoomTemplates();
            Map.GenerateMap(2);
            Player.CurrentRoom = Map.CurrentRoom;
            TreasurePool.SortPool();
            BossZones.Create();
            Boss = new Boss(150, 4, new Collider(Resources.Boss.BodySize), Map.CurrentRoom, Player);
            SetKeyBindings();
        }

        public static void Update()
        {
            if (!GameOver)
            {
                if (Player.HP <= 0)
                    GameOver = true;
                Player.Update();

                Map.Update();
            }
        }

        public static void SetWindowSize(int width, int height)
        {
            WindowSize.Width = width;
            WindowSize.Height = height;
        }

        static void SetKeyBindings()
        {
            KeyBindings[Keys.Left] = () =>
            {
                lastKey = Keys.A;
                Player.MoveTo(Direction.Left);
                if (Player.status == Status.Attack)
                    Player.status = Status.AttackMove;
                if (Player.status != Status.AttackMove)
                    Player.status = Status.Move;
            };
            KeyBindings[Keys.A] = KeyBindings[Keys.Left];

            KeyBindings[Keys.Right] = () =>
            {
                lastKey = Keys.D;
                Player.MoveTo(Direction.Right);
                if (Player.status == Status.Attack)
                    Player.status = Status.AttackMove;
                if (Player.status != Status.AttackMove)
                    Player.status = Status.Move;
            };
            KeyBindings[Keys.D] = KeyBindings[Keys.Right];

            KeyBindings[Keys.Up] = KeyBindings[Keys.Space] = () =>
            {
                if (Player.treasures.Contains(TreasurePool.GetTreasureByID(0)))
                    Player.MoveUp();
                else
                    Player.Jump();
            };
            KeyBindings[Keys.W] = KeyBindings[Keys.Up];

            KeyBindings[Keys.Down] = () =>
            {
                if (Player.treasures.Contains(TreasurePool.GetTreasureByID(0)))
                    Player.MoveDown();
                else
                    Player.MoveDown(1);
            };
            KeyBindings[Keys.S] = KeyBindings[Keys.Down];

            KeyBindings[Keys.M] = () => Console.WriteLine(Map.seed);

            KeyBindings[Keys.E] = () =>
            {
                if (Player.status != Status.Attack && Player.status != Status.AttackMove)
                    if (Player.status == Status.Idle)
                        Player.status = Status.Attack;
                    else
                        Player.status = Status.AttackMove;
            };

            KeyBindings[Keys.Q] = () => Player.Shoot();

            var treasureID = 0;

            KeyBindings[Keys.D1] = () => Map.CurrentRoom().LootList.Add(
                new TreasureItem(50, 0, new Collider(Resources.Treasures.Size), Map.CurrentRoom, treasureID));

            KeyBindings[Keys.D2] = () => TreasurePool.RemoveFromPlayer(treasureID);

            KeyBindings[Keys.D3] = () => Map.CurrentRoom().LootList.Add(
                new HeartLoot(Player.x - 50, Player.y, new Collider(Resources.Loot.Size), Map.CurrentRoom));

            KeyBindings[Keys.D4] = () => Map.CurrentRoom().LootList.Add(
                new AmmoLoot(Player.x - 50, Player.y, new Collider(Resources.Loot.Size), Map.CurrentRoom));

            KeyBindings[Keys.D5] = () => Map.CurrentRoom().EnemyList.Add(
                new Chameleon(Player.x - 50, Player.y, new Collider(Resources.Dummy.IdleSize), Map.CurrentRoom, Player));

            KeyBindings[Keys.Z] = () =>
            {
                if (DeveloperToolsON)
                    DeveloperToolsON = false;
                else
                    DeveloperToolsON = true;
            };

            KeyBindings[Keys.T] = () =>
            {
                foreach (var treasureRoom in Map.rooms.Where(r => r.LootList.Count != 0))
                    foreach (var loot in treasureRoom.LootList.Where(l => l is TreasureItem))
                        Console.Write(loot.ID + "(" + (loot as TreasureItem).Treasure.Price + ")" + " ");
                Console.WriteLine();
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
                    Resources.Boss.Body = new Bitmap(@"..\..\..\..\Sprites\Room\SuperSecret.png");
                else
                    superSecret = 0;
            };

            KeyBindings[Keys.P] = () =>
            {
                Console.WriteLine("ABOBA");
            };
        }
    }
}

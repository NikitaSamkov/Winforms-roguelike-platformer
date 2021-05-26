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
        public static Map Map;
        public static Size WindowSize = new Size();
        public static Dictionary<Keys, Action> KeyBindings = new Dictionary<Keys, Action>();
        public static bool GameOver = false;
        public static bool DeveloperToolsON = false;

        static Game()
        {
            Player = new Player(150, 150, new Collider(Resources.Player.IdleSize, 0, 0,
                new Collider(Resources.Player.AttackRange, -10, Resources.Player.Idle.Height / 8)),
                null);
            Map = new Map(Player);
            Map.GenerateRooms();
            Player.CurrentRoom = Map.CurrentRoom;
            TreasurePool.SortPool();
            SetKeyBindings();
        }

        public static void Update()
        {
            if (!GameOver)
            {
                Map.Update();

                if (Player.hp <= 0)
                    GameOver = true;
                Player.Update();
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
                Player.MoveTo(Direction.Left);
                if (Player.status == Status.Attack)
                    Player.status = Status.AttackMove;
                if (Player.status != Status.AttackMove)
                    Player.status = Status.Move;
            };
            KeyBindings[Keys.A] = KeyBindings[Keys.Left];

            KeyBindings[Keys.Right] = () =>
            {
                Player.MoveTo(Direction.Right);
                if (Player.status == Status.Attack)
                    Player.status = Status.AttackMove;
                if (Player.status != Status.AttackMove)
                    Player.status = Status.Move;
            };
            KeyBindings[Keys.D] = KeyBindings[Keys.Right];

            KeyBindings[Keys.Up] = KeyBindings[Keys.Space] = () =>
            {
                if (Player.flying)
                    Player.MoveUp();
                else
                    Player.Jump();
            };
            KeyBindings[Keys.W] = KeyBindings[Keys.Up];

            KeyBindings[Keys.Down] = () =>
            {
                if (Player.flying)
                    Player.MoveDown();
                else
                    Player.MoveDown(1);
            };
            KeyBindings[Keys.S] = KeyBindings[Keys.Down];

            KeyBindings[Keys.M] = () => Console.WriteLine(Map.seed);

            KeyBindings[Keys.D0] = () => Map.CurrentRoom().enemyList.Add(
                new Enemy(Player.x, Player.y, new Collider(Resources.Dummy.IdleSize), Map.CurrentRoom, Player));

            KeyBindings[Keys.E] = () =>
            {
                if (Player.status != Status.Attack && Player.status != Status.AttackMove)
                    if (Player.status == Status.Idle)
                        Player.status = Status.Attack;
                    else
                        Player.status = Status.AttackMove;
            };

            KeyBindings[Keys.Q] = () =>
            {
                var arrow = new Arrow(Player.x, Player.y + Player.collider.field.Height / 2,
                    new Collider(Resources.Arrow.IdleSize), Map.CurrentRoom, 15, Player.bowStrenght, ProjectileType.Ally);
                arrow.MoveTo(Player.direction);
                arrow.status = Status.Move;
                Map.CurrentRoom().ProjectilesList.Add(arrow);
            };

            KeyBindings[Keys.D1] = () => Map.CurrentRoom().TreasuresList.Add(
                new TreasureItem(50, 0, new Collider(Resources.Treasures.Size), Map.CurrentRoom, 0));

            KeyBindings[Keys.D2] = () => TreasurePool.RemoveFromPlayer(Player, 0);

            KeyBindings[Keys.Z] = () =>
            {
                if (DeveloperToolsON)
                    DeveloperToolsON = false;
                else
                    DeveloperToolsON = true;
            };

            KeyBindings[Keys.P] = () =>
            {
                Console.WriteLine("ABOBA");
            };
        }
    }
}

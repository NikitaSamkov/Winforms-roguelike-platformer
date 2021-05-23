using System;
using System.Collections.Generic;
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
        public static int WindowWidth;
        public static int WindowHeight;
        public static Dictionary<Keys, Action> keyBindings;

        static Game()
        {
            Player = new Player(150, 150, new Collider(Resources.Player.IdleSize, 0, 0,
                new Collider(Resources.Player.AttackRange, -10, Resources.Player.Idle.Height / 8)),
                null);
            Map = new Map(Player);
            Map.GenerateRooms();
            Player.room = Map.CurrentRoom;
            TreasurePool.SortPool();
            keyBindings = new Dictionary<Keys, Action>();
            SetKeyBindings();
        }

        public static void Update()
        {
            Map.Update();

            Player.Update();
        }

        public static void SetWindowSize(int width, int height)
        {
            WindowWidth = width;
            WindowHeight = height;
        }

        static void SetKeyBindings()
        {
            keyBindings[Keys.Left] = () =>
            {
                Player.MoveTo(Direction.Left);
                if (Player.status == Status.Attack)
                    Player.status = Status.AttackMove;
                if (Player.status != Status.AttackMove)
                    Player.status = Status.Move;
            };
            keyBindings[Keys.A] = keyBindings[Keys.Left];

            keyBindings[Keys.Right] = () =>
            {
                Player.MoveTo(Direction.Right);
                if (Player.status == Status.Attack)
                    Player.status = Status.AttackMove;
                if (Player.status != Status.AttackMove)
                    Player.status = Status.Move;
            };
            keyBindings[Keys.D] = keyBindings[Keys.Right];

            keyBindings[Keys.Up] = keyBindings[Keys.Space] = () =>
            {
                if (Player.flying)
                    Player.MoveUp();
                else
                    Player.Jump();
            };
            keyBindings[Keys.W] = keyBindings[Keys.Up];

            keyBindings[Keys.Down] = () =>
            {
                if (Player.flying)
                    Player.MoveDown();
                else
                    Player.MoveDown(1);
            };
            keyBindings[Keys.S] = keyBindings[Keys.Down];

            keyBindings[Keys.M] = () => Console.WriteLine(Map.seed);
            keyBindings[Keys.D0] = () => Map.CurrentRoom.enemyList.Add(
                new Enemy(Player.x, Player.y, new Collider(Resources.Dummy.IdleSize), Map.CurrentRoom, Player));
            keyBindings[Keys.E] = () =>
            {
                if (Player.status == Status.Idle)
                    Player.status = Status.Attack;
                else
                    Player.status = Status.AttackMove;
            };
            keyBindings[Keys.Q] = () => 
            {
                var arrow = new Arrow(Player.x, Player.y + Player.collider.field.Height / 2,
                    new Collider(Resources.Arrow.IdleSize), Map.CurrentRoom, 15, Player.bowStrenght, ProjectileType.Ally);
                arrow.MoveTo(Player.direction);
                arrow.status = Status.Move;
            };
            keyBindings[Keys.P] = () =>
            {
                Console.WriteLine("ABOBA");
            };
        }
    }
}

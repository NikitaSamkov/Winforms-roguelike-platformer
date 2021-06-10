using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Reflection;

namespace Winforms_platformer
{
    [TestFixture]
    public class Tests
    {
        Player player;
        Room room;

        [SetUp]
        public void SetUp()
        {
            player = new Player(0, 0, new Collider(new Size(40, 112), 0, 0, new Collider(new Size(60, 112), -10, 0)), () => room);
            room = new Room(player, 7, 486);
            TreasurePool.SetRandom(0);
            TreasurePool.SortPool();
        }

        [Test]
        public void RoomTest()
        {
            Assert.AreEqual(player.CurrentRoom(), room);
        }

        [TestCase("MoveY", 0, 7, 1)]
        [TestCase("MoveY", 0, 21, 2)]
        [TestCase("MoveY", 0, 42, 3)]
        [TestCase("MoveY", 373, 374, 1)]
        [TestCase("MoveY", 0, 374, 10)]
        [TestCase("MoveY", 999, 374, 1)]
        [TestCase("MoveY", 0, 0, 1, 0)]
        [TestCase("MoveY", 0, 0, 10, 0)]
        [TestCase("MoveY", 0, 1, 1, 1)]
        [TestCase("MoveY", 0, 3, 2, 1)]
        [TestCase("MoveY", 0, 50, 1, 50)]
        [TestCase("MoveY", 0, 374, 4, 50)]
        [TestCase("Jump", 374, 331, 1)]
        [TestCase("Jump", 373, 374, 1)]
        [TestCase("Jump", 374, 324, 1, 0)]
        [TestCase("Jump", 374, 374, 1, 50)]
        public void ChangingYTests(string methodName, int startY, int expectedY, int repeats, int gravity = 7)
        {
            player.CurrentRoom().gForce = gravity;
            player.TeleportTo(player.x, startY);
            var method = typeof(Player).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            for (var i = 0; i < repeats; i++)
            {
                if (methodName != "MoveY")
                    method.Invoke(player, new object[0]);
                player.Update();
            }
            Assert.AreEqual(expectedY, player.y);
        }

        [Test]
        public void AmuletOfFlyingTest()
        {
            TreasurePool.GiveToPlayer(0, player);
            player.TeleportTo(0, 0);
            player.Update();
            Assert.AreEqual(player.y, 0);
            TreasurePool.RemoveFromPlayer(0, true, player);
            player.Update();
            Assert.AreEqual(player.y, room.gForce);
        }

        [TestCase(1, 1)]
        [TestCase(1, 5)]
        [TestCase(1, 10)]
        [TestCase(1, 15)]
        [TestCase(1, 23)]
        [TestCase(5, 5)]
        public void HurtTest(int damage, int repeats)
        {
            var expected = player.HP - damage * player.hurtMultiplier;
            for (var i = 0; i < repeats; i++)
                player.Hurt(damage);
            Assert.AreEqual(expected, player.HP);
        }

        [Test]
        public void AttackTest()
        {
            player.TeleportTo(0, 0);
            var dummy = new Enemy(player.x + player.collider.field.Width, player.y, new Collider(new Size(40, 96)), player.CurrentRoom);
            room.EnemyList.Add(dummy);
            var expected = dummy.HP - player.damage * dummy.hurtMultiplier;
            player.MoveTo(Direction.Right);
            player.status = Status.Attack;
            player.Update();
            Assert.AreEqual(expected, dummy.HP);
        }

        [TestCase(Direction.Right)]
        [TestCase(Direction.Left)]
        public void MoveTest(Direction direction)
        {
            var expected = player.x + (-2 * (int)direction + 1) * player.xSpeed;
            player.MoveTo(direction);
            player.status = Status.Move;
            player.Update();
            Assert.AreEqual(expected, player.x);
        }

        [Test]
        public void MoveDownTest()
        {
            player.TeleportTo(0, 0);
            player.MoveDown();
            Assert.AreEqual(player.xSpeed, player.y);
        }

        [TestCase(1)]
        [TestCase(5)]
        [TestCase(100)]
        [TestCase(1000)]
        public void MoveDownTest(int distance)
        {
            player.TeleportTo(0, 0);
            player.MoveDown(distance);
            Assert.AreEqual(distance, player.y);
        }
    }
}

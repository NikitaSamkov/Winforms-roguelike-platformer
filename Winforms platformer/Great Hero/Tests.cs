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
    public class PhysicsTests
    {
        Player player;
        Room room;

        [SetUp]
        public void SetUp()
        {
            player = new Player(0, 0, new Collider(new Size(40, 112)), () => room);
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
    }

    [TestFixture]
    public class TresureTests
    {
        Player player;
        Room room;

        [SetUp]
        public void SetUp()
        {
            player = new Player(0, 0, new Collider(new Size(40, 112)), () => room);
            room = new Room(player, 7, 486);
            TreasurePool.SetRandom(0);
            TreasurePool.SortPool();
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
    }
}

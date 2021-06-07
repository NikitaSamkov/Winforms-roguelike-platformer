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
            room = new Room(RoomType.RegularRoom);
        }

        [Test]
        public void RoomTest()
        {
            Assert.AreEqual(player.CurrentRoom(), room);
        }

        [TestCase(0, 7)]
        public void MoveYTest(int startY, int expectedY)
        {
            var method = typeof(Entity).GetMethod("MoveY", BindingFlags.NonPublic | BindingFlags.Instance);
            method.Invoke(player, new object[0]);
            Assert.AreEqual(expectedY, player.y);
        }
    }
}

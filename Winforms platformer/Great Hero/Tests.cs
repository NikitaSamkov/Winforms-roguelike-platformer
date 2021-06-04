using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Winforms_platformer.Model;

namespace Winforms_platformer
{
    [TestFixture]
    public class TreasureTests
    {
        [SetUp]
        public void SetUp()
        {
            
        }

        [Test]
        public void TestRoom()
        {
            Assert.AreEqual(Game.Player.CurrentRoom(), Game.Map.CurrentRoom());
        }
    }
}

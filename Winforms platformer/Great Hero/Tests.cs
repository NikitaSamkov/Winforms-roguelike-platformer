﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Winforms_platformer;

[TestFixture]
public class Tests
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
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winforms_platformer.Model
{
    public static class RoomGenerator
    {
        public static Room PictireToRoom(string map, int customGroundLevel = -1, int customGravity = 7, string separator = "\r\n")
        {
            #region HOW TO DRAW A MAP
            /*
            @"7         <-- number => standart row symbol count
                        <-- 7 spaces => ' ' == nothing, air
               %   1    <-- 3 spaces, 1 percent, 3 spaces, and NUMBER => percent == treasure, NUMBER == treasure ID. !!ATTENTION!!
                            row has standart row symbol count + percent count + ',' count SYMBOLS! (if more than 1 treasure use ',')
              ---       <-- 2 spaces, 3 dashes, 2 spaces => dash line == platform, dash lenght == platform lenght
             *  **      <-- 1 space, 1 zvezdochka, 2 spaces, 2 zvezdochki, 1 space => zvezdochka == random monster spawn
            -------=150 <-- 7 dasges, 1 = and NUMBER => = + NUMBER == custom height of line
            #######"    <-- 7 hashs => hash == ground !!ATTENTION!! when generator will see hash, it will stop working immediately!
                                     it will calculate ground level by multiplying 1 symbol height by row count (vertical symbols)
            */
            #endregion
            var rows = map.Split(new[] { separator }, StringSplitOptions.None);
            var symbolWidth = 800 / int.Parse(rows[0]) + 1;
            var symbolHeight = 600 / rows.Length;
            var groundLevel = 0;
            var customGround = false;
            if (customGroundLevel > -1)
            {
                customGround = true;
                groundLevel = customGroundLevel;
            }
            var platforms = new List<Platform>();
            var enemies = new List<Point>();
            var treasures = new List<Loot>();
            for (var y = 1; y < rows.Length; y++)
            {
                var platform = false;
                var platformStart = 0;
                var tempPlatforms = new List<Platform>();
                var tempEnemies = new List<Point>();
                var tempTreasuresPoints = new List<Point>();
                var currentID = 0;
                var tempTreasures = new List<Loot>();
                var customHeight = false;
                var height = -1;
                for (var x = 0; x < rows[y].Length; x++)
                {
                    if (rows[y][x] != '#')
                    {
                        if (rows[y][x] != '-' && platform)
                        {
                            platform = false;
                            tempPlatforms.Add(new Platform(platformStart, x * symbolWidth, (y - 1) * symbolHeight));
                        }
                        if (int.TryParse(rows[y][x].ToString(), out var number))
                            if (customHeight)
                                height = height * 10 + number;
                            else
                                currentID = currentID * 10 + number;
                        switch (rows[y][x])
                        {
                            case '*':
                                tempEnemies.Add(new Point(x * symbolWidth, (y - 1) * symbolHeight));
                                break;
                            case '-':
                                if (!platform)
                                {
                                    platform = true;
                                    platformStart = x * symbolWidth;
                                }
                                break;
                            case '%':
                                tempTreasuresPoints.Add(new Point(x * symbolWidth, (y - 1) * symbolHeight));
                                break;
                            case '=':
                                customHeight = true;
                                height = 0;
                                break;
                            case ',':
                                if (customHeight)
                                    customHeight = false;
                                else
                                {
                                    tempTreasures.Add(
                                        new TreasureItem(tempTreasuresPoints[0].X, tempTreasuresPoints[0].Y,
                                        new Collider(Resources.Treasures.Size), Game.Map.CurrentRoom, currentID));
                                    currentID = 0;
                                    tempTreasuresPoints.Remove(tempTreasuresPoints[0]);
                                }
                                break;
                        }
                    }
                    else
                    {
                        if (!customGround)
                            groundLevel = (y - 1) * symbolHeight;
                        break;
                    }
                }
                if (groundLevel != 0 && !customGround)
                    break;
                if (platform)
                {
                    platform = false;
                    tempPlatforms.Add(new Platform(platformStart, rows[y].Length * symbolWidth, (y - 1) * symbolHeight));
                }
                if (height > -1)
                {
                    tempPlatforms = tempPlatforms.Select(p => new Platform(p.leftBorder, p.rightBorder, height)).ToList();
                    tempEnemies = tempEnemies.Select(e =>
                    {
                        e.Y = height;
                        return e;
                    }).ToList();
                    tempTreasures = tempTreasures.Select(t =>
                    {
                        t.TeleportTo(t.x, height);
                        return t;
                    }).ToList();
                }
                platforms = platforms.Concat(tempPlatforms).ToList();
                enemies = enemies.Concat(tempEnemies).ToList();
                treasures = treasures.Concat(tempTreasures).ToList();
            }

            return new Room(RoomType.RegularRoom, Game.Player, platforms, treasures, enemies, customGravity, groundLevel);
        }
    }
}

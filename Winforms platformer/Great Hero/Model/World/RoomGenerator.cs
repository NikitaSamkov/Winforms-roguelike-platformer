﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winforms_platformer.Model
{
    public static class RoomGenerator
    {
        public static Room PictireToRoom(string map, string separator = "\r\n")
        {
            #region HOW TO DRAW A MAP
            /*
            @"7       <-- number => standart row symbol count
                      <-- 7 spaces => ' ' == nothing, air
               %   1  <-- 3 spaces, 1 percent, 3 spaces, and NUMBER => percent == treasure, NUMBER == treasure ID. !!ATTENTION!!
                          row has standart row symbol count + percent count + ',' count SYMBOLS! (if more than 1 treasure use ',')
              ---     <-- 2 spaces, 3 dashes, 2 spaces => dash line == platform, dash lenght == platform lenght
             *  **    <-- 1 space, 1 zvezdochka, 2 spaces, 2 zvezdochki, 1 space => zvezdochka == random monster spawn
            #######"  <-- 7 hashs => hash == ground !!ATTENTION!! when generator will see hash, it will stop working immediately!
                                     it will calculate ground level by multiplying 1 symbol height by row count (vertical symbols)
            */
            #endregion
            var rows = map.Split(new[] { separator }, StringSplitOptions.None);
            var symbolWidth = 800 / int.Parse(rows[0]) + 1;
            var symbolHeight = 600 / rows.Length;
            var groundLevel = 0;
            var platforms = new List<Platform>();
            var enemies = new List<Enemy>();
            var treasures = new List<Loot>();
            for (var y = 1; y < rows.Length; y++)
            {
                var platform = false;
                var platformStart = 0;
                var tempPlatforms = new List<Platform>();
                var tempEnemies = new List<Enemy>();
                var tempTreasuresPoints = new List<Point>();
                var currentID = 0;
                var tempTreasures = new List<Loot>();
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
                            currentID = currentID * 10 + number;
                        switch (rows[y][x])
                        {
                            case '*':
                                tempEnemies.Add(RandomEnemyGenerator.GetRandomEnemy(
                                    x * symbolWidth, (y - 1) * symbolHeight));
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
                            case ',':
                                tempTreasures.Add(
                                    new TreasureItem(tempTreasuresPoints[0].X, tempTreasuresPoints[0].Y,
                                    new Collider(Resources.Treasures.Size), Game.Map.CurrentRoom, currentID));
                                currentID = 0;
                                tempTreasuresPoints.Remove(tempTreasuresPoints[0]);
                                break;
                        }
                    }
                    else
                    {
                        groundLevel = (y - 1) * symbolHeight;
                        break;
                    }
                }
                if (groundLevel != 0)
                    break;
                if (platform)
                {
                    platform = false;
                    tempPlatforms.Add(new Platform(platformStart, rows[y].Length * symbolWidth, (y - 1) * symbolHeight));
                }
                platforms = platforms.Concat(tempPlatforms).ToList();
                enemies = enemies.Concat(tempEnemies).ToList();
                treasures = treasures.Concat(tempTreasures).ToList();
            }

            return new Room(RoomType.RegularRoom, Game.Player, platforms, treasures, enemies, 7, groundLevel);
        }
    }
}

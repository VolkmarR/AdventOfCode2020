using AdventOfCode.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Day20
{
    public class Day20Tests
    {
        private readonly ITestOutputHelper Output;
        public Day20Tests(ITestOutputHelper output) => Output = output;

        [Fact]
        public void RunStep1() => Output.WriteLine(new Day20Solver().ExecutePuzzle1());

        [Fact]
        public void RunStep2() => Output.WriteLine(new Day20Solver().ExecutePuzzle2());
    }

    public class Tile
    {
        public long ID { get; set; }
        public List<string> Map { get; set; }
        public List<string[]> Border { get; set; }
    }

    public class PuzzeMatch
    {
        public Tile Tile { get; set; }
        public int borderIndex { get; set; }
    }

    public class Day20Solver : SolverBase
    {
        List<Tile> Data;
        int SquareBorderLength;

        string GetVerticalBorder(List<string> data, int rowIndex)
        {
            var line = "";
            foreach (var item in data)
                line += item[rowIndex];
            return line;
        }

        Tile ParseTile(string tileLine, List<string> data)
        {
            var result = new Tile
            {
                ID = tileLine[5..9].ToLong(),
                Map = data,
                Border = new List<string[]>()
            };

            void AddRotate()
            {
                var last = result.Border.Last();
                result.Border.Add(new string[] { last[3].Reverse(), last[0], last[1].Reverse(), last[2] });
            }

            result.Border.Add(new string[] { data[0], GetVerticalBorder(data, data[0].Length - 1), data[data.Count - 1], GetVerticalBorder(data, 0) });
            for (int i = 0; i < 3; i++)
                AddRotate();

            result.Border.Add(new string[] { result.Border[0][0].Reverse(), result.Border[0][3], result.Border[0][2].Reverse(), result.Border[0][1] });
            for (int i = 0; i < 3; i++)
                AddRotate();

            return result;

        }
        protected override void Parse(List<string> data)
        {
            Data = new();
            var i = 0;
            while (i < data.Count)
            {
                var tileLine = data[i++];
                var map = new List<string>();
                while (i < data.Count && data[i] != "")
                    map.Add(data[i++]);
                i++;
                Data.Add(ParseTile(tileLine, map));
            }

            SquareBorderLength = (int)Math.Sqrt(Data.Count);
        }

        List<PuzzeMatch> FindMatchingTiles(string borderRight, string borderBottom, List<Tile> tiles)
        {
            var result = new List<PuzzeMatch>();
            foreach (var tile in tiles)
                for (int i = 0; i < tile.Border.Count; i++)
                {
                    var border = tile.Border[i];
                    if ((borderBottom == null || border[0] == borderBottom) && borderRight == null || borderRight == border[3])
                        result.Add(new PuzzeMatch { Tile = tile, borderIndex = i });
                }

            return result;
        }



        protected override object Solve1()
        {
            var puzzle = new PuzzeMatch[SquareBorderLength, SquareBorderLength];
            // bool Puzzle
            bool SolvePuzzle(int x, int y, List<Tile> tiles)
            {
                if (y == SquareBorderLength)
                    return true;

                var borderRight = x > 0 ? puzzle[y, x - 1].Tile.Border[puzzle[y, x - 1].borderIndex][1] : null;
                var borderBottom = y > 0 ? puzzle[y - 1, x].Tile.Border[puzzle[y - 1, x].borderIndex][2] : null;
                var matches = FindMatchingTiles(borderRight, borderBottom, tiles);
                if (matches.Count > 0)
                {
                    var nextX = x + 1;
                    var nextY = y;
                    if (nextX == SquareBorderLength)
                    {
                        nextX = 0;
                        nextY++;
                    }

                    foreach (var item in matches)
                    {
                        puzzle[y, x] = item;
                        if (SolvePuzzle(nextX, nextY, tiles.Where(q => q != item.Tile).ToList()))
                            return true;
                    }
                }

                return false;
            }

            SolvePuzzle(0, 0, Data);


            return puzzle[0, 0].Tile.ID * puzzle[SquareBorderLength - 1, 0].Tile.ID * puzzle[0, SquareBorderLength - 1].Tile.ID * puzzle[SquareBorderLength - 1, SquareBorderLength - 1].Tile.ID;
        }

        protected override object Solve2()
        {
            throw new Exception("Solver error");
        }
    }
}

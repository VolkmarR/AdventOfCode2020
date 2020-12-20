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

    public class MapItem
    {
        public List<string> Map { get; set; }
        public string[] Borders { get; set; }
    }

    public class Tile
    {
        public long ID { get; set; }
        public List<MapItem> Maps { get; set; }
    }

    public class PuzzeMatch
    {
        public Tile Tile { get; set; }
        public int MapIndex { get; set; }
    }

    public class Day20Solver : SolverBase
    {
        List<Tile> Data;
        int SquareBorderLength;
        PuzzeMatch[,] Puzzle;
        List<string> BigMap;

        string GetVerticalBorder(List<string> data, int rowIndex)
        {
            var line = "";
            foreach (var item in data)
                line += item[rowIndex];
            return line;
        }

        List<string> Rotate90(List<string> data)
        {
            var result = data.Select(q => "").ToList();
            for (int y = 0; y < data.Count; y++)
                for (int x = 0; x < data[0].Length; x++)
                    result[y] += data[data.Count - 1 - x][y];

            return result;
        }

        List<string> Flip(List<string> data)
            => data.Select(q => q.Reverse()).ToList();

        Tile ParseTile(string tileLine, List<string> data)
        {
            var result = new Tile
            {
                ID = tileLine[5..9].ToLong(),
                Maps = new List<MapItem>()
            };

            void AddMap(List<string> map)
            {
                for (int i = 0; i < 4; i++)
                {
                    result.Maps.Add(new MapItem
                    {
                        Map = map,
                        Borders = new string[]
                        {
                        map[0],
                        GetVerticalBorder(map, map[0].Length - 1),
                        map[map.Count - 1],
                        GetVerticalBorder(map, 0)
                        }
                    });
                    map = Rotate90(map);
                }
            }

            AddMap(data);
            AddMap(Flip(data));

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
            Puzzle = new PuzzeMatch[SquareBorderLength, SquareBorderLength];
        }

        List<PuzzeMatch> FindMatchingTiles(string borderRight, string borderBottom, List<Tile> tiles)
        {
            var result = new List<PuzzeMatch>();
            foreach (var tile in tiles)
                for (int i = 0; i < tile.Maps.Count; i++)
                {
                    var borders = tile.Maps[i].Borders;
                    if ((borderBottom == null || borders[0] == borderBottom) && borderRight == null || borderRight == borders[3])
                        result.Add(new PuzzeMatch { Tile = tile, MapIndex = i });
                }

            return result;
        }

        bool SolvePuzzle(int x, int y, List<Tile> tiles)
        {
            if (y == SquareBorderLength)
                return true;

            var borderRight = x > 0 ? Puzzle[x - 1, y].Tile.Maps[Puzzle[x - 1, y].MapIndex].Borders[1] : null;
            var borderBottom = y > 0 ? Puzzle[x, y - 1].Tile.Maps[Puzzle[x, y - 1].MapIndex].Borders[2] : null;
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
                    Puzzle[x, y] = item;
                    if (SolvePuzzle(nextX, nextY, tiles.Where(q => q != item.Tile).ToList()))
                        return true;
                }
            }

            return false;
        }

        void InitBigMap()
        {
            SolvePuzzle(0, 0, Data);
            BigMap = Enumerable.Repeat("", SquareBorderLength * 8).ToList();
            for (int y = 0; y < SquareBorderLength; y++)
                for (int x = 0; x < SquareBorderLength; x++)
                    for (int yy = 1; yy < 9; yy++)
                        BigMap[y * 8 + yy - 1] += Puzzle[x, y].Tile.Maps[Puzzle[x, y].MapIndex].Map[yy][1..9];
        }

        protected override object Solve1()
        {
            SolvePuzzle(0, 0, Data);

            return Puzzle[0, 0].Tile.ID * Puzzle[SquareBorderLength - 1, 0].Tile.ID * Puzzle[0, SquareBorderLength - 1].Tile.ID * Puzzle[SquareBorderLength - 1, SquareBorderLength - 1].Tile.ID;
        }

        protected override object Solve2()
        {
            InitBigMap();


            throw new Exception("Solver error");
        }
    }
}

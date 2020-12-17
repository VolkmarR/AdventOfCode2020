using AdventOfCode.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Day17
{
    public class Day17Tests
    {
        private readonly ITestOutputHelper Output;
        public Day17Tests(ITestOutputHelper output) => Output = output;

        [Fact]
        public void RunStep1() => Output.WriteLine(new Day17Solver().ExecutePuzzle1());

        [Fact]
        public void RunStep2() => Output.WriteLine(new Day17Solver().ExecutePuzzle2());
    }

    public class Day17Solver : SolverBase
    {
        Dictionary<int, Dictionary<int, HashSet<int>>> Map;
        int MinZ = 0;
        int MaxZ = 0;
        int MinXY = 0;
        int MaxXY = 0;

        HashSet<int> GetXY(Dictionary<int, Dictionary<int, HashSet<int>>> map, int x, int y)
        {
            if (!map.TryGetValue(x, out var mapX))
            {
                mapX = new();
                map[x] = mapX;
            }

            if (!mapX.TryGetValue(y, out var mapY))
            {
                mapY = new();
                mapX[y] = mapY;
            }

            return mapY;
        }

        bool Get(Dictionary<int, Dictionary<int, HashSet<int>>> map, int x, int y, int z)
            => GetXY(map, x, y).Contains(z);

        void Set(Dictionary<int, Dictionary<int, HashSet<int>>> map, int x, int y, int z, bool value)
        {
            var mapZ = GetXY(map, x, y);
            if (!value && mapZ.Contains(z))
                mapZ.Remove(z);
            else if (value)
                mapZ.Add(z);
        }

        bool GetNewState(Dictionary<int, Dictionary<int, HashSet<int>>> map, int x, int y, int z)
        {
            int count = 0;
            for (int xx = x - 1; xx <= x + 1; xx++)
                for (int yy = y - 1; yy <= y + 1; yy++)
                {
                    for (int zz = z - 1; zz <= z + 1; zz++)
                        if (Get(map, xx, yy, zz) && !(xx == x && yy == y && zz == z))
                            count++;
                    if (count > 3)
                        break;
                }

            var current = Get(map, x, y, z);
            if (current && count is 2 or 3)
                return true;
            if (!current && count is 3)
                return true;

            return false;
        }

        int SumMap(Dictionary<int, Dictionary<int, HashSet<int>>> map)
        {
            var result = 0;
            foreach (var mapX in map)
                foreach (var mapY in mapX.Value)
                    result += mapY.Value.Count;
            return result;
        }

        protected override void Parse(List<string> data)
        {
            Map = new();
            for (int y = 0; y < data.Count; y++)
                for (int x = 0; x < data[y].Length; x++)
                    if (data[y][x] == '#')
                        Set(Map, x, y, 0, true);

            MinZ = 0;
            MaxZ = 0;
            MinXY = 0;
            MaxXY = data.Count - 1;
        }

        protected override object Solve1()
        {

            for (int i = 1; i <= 6; i++)
            {
                var newMap = new Dictionary<int, Dictionary<int, HashSet<int>>>();
                for (int x = MinXY - i; x <= MaxXY + i; x++)
                    for (int y = MinXY - i; y <= MaxXY + i; y++)
                        for (int z = MinZ - i; z <= MaxZ + i; z++)
                            Set(newMap, x, y, z, GetNewState(Map, x, y, z));

                Map = newMap;
            }

            return SumMap(Map);
        }

        protected override object Solve2()
        {
            throw new Exception("Solver error");
        }
    }
}

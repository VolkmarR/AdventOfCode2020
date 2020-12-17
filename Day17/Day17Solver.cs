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

    record Point4D(int x, int y, int z, int w);

    public class Day17Solver : SolverBase
    {
        HashSet<Point4D> Map;
        int MaxXY = 0;

        bool Get(HashSet<Point4D> map, int x, int y, int z, int w)
            => map.Contains(new Point4D(x, y, z, w));

        void Set(HashSet<Point4D> map, int x, int y, int z, int w, bool value)
        {
            var point = new Point4D(x, y, z, w);
            if (!value && map.Contains(point))
                map.Remove(point);
            else if (value)
                map.Add(point);
        }

        bool GetNewState3D(HashSet<Point4D> map, int x, int y, int z, int w)
        {
            int count = 0;
            for (int xx = x - 1; xx <= x + 1; xx++)
                for (int yy = y - 1; yy <= y + 1; yy++)
                {
                    for (int zz = z - 1; zz <= z + 1; zz++)
                        if (Get(map, xx, yy, zz, w) && !(xx == x && yy == y && zz == z))
                            count++;
                    if (count > 3)
                        break;
                }

            var current = Get(map, x, y, z, w);
            return (current && count is 2 or 3) || (!current && count is 3);
        }

        int SumMap(HashSet<Point4D> map)
        {
            return map.Count;
        }

        protected override void Parse(List<string> data)
        {
            Map = new();
            for (int y = 0; y < data.Count; y++)
                for (int x = 0; x < data[y].Length; x++)
                    if (data[y][x] == '#')
                        Set(Map, x, y, 0, 0, true);

            MaxXY = data.Count - 1;
        }

        protected override object Solve1()
        {
            for (int i = 1; i <= 6; i++)
            {
                var newMap = new HashSet<Point4D>();
                for (int x = -i; x <= MaxXY + i; x++)
                    for (int y = -i; y <= MaxXY + i; y++)
                        for (int z = -i; z <= i; z++)
                            Set(newMap, x, y, z, 0, GetNewState3D(Map, x, y, z, 0));

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

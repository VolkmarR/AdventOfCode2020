using AdventOfCode.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Day10
{
    public class Day10Tests
    {
        private readonly ITestOutputHelper Output;
        public Day10Tests(ITestOutputHelper output) => Output = output;

        [Fact]
        public void RunStep1() => Output.WriteLine(new Day10Solver().ExecutePuzzle1());

        [Fact]
        public void RunStep2() => Output.WriteLine(new Day10Solver().ExecutePuzzle2());
    }

    public class Day10Solver : SolverBase
    {
        List<int> Data;

        protected override void Parse(List<string> data)
        {
            Data = data.Select(q => int.Parse(q)).OrderBy(q => q).ToList();
            Data.Insert(0, 0);
            Data.Add(Data.Max() + 3);
        }

        protected override object Solve1()
        {
            var counters = new int[] { 0, 0, 0 };
            var current = 0;
            foreach (var item in Data.Skip(1))
            {
                var diff = item - current;
                counters[diff - 1]++;
                current = item;
            }

            return counters[0] * counters[2];
        }


        protected override object Solve2()
        {
            var combinations = 1L;

            var i = 0;
            while (i < Data.Count)
            {
                var j = 1;
                while (i + j < Data.Count && Data[i + j] - Data[i + j - 1] == 1)
                    j++;

                i += j;

                j-=2;
                if (j > 0)
                    combinations *= (long)((j * j + j + 2) / 2);
            }

            return combinations;
        }

    }
}

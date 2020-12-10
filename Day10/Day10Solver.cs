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
            Data.Add(Data.Max() + 3);
        }

        protected override object Solve1()
        {
            var counters = new int[] { 0, 0, 0 };
            var current = 0;
            foreach (var item in Data)
            {
                var diff = item - current;
                counters[diff - 1]++;
                current = item;
            }

            return counters[0] * counters[2];
        }


        protected override object Solve2()
        {
            long combinations = 0;

            void CountCombinations(int current, int startIndex)
            {
                while (startIndex + 1 < Data.Count && Data[startIndex + 1] - current > 3)
                {
                    current = Data[startIndex];
                    startIndex++;
                }

                if (startIndex == Data.Count)
                {
                    combinations++;
                    return;
                }

                for (int i = startIndex; i < Data.Count && Data[i] - current <= 3; i++)
                    CountCombinations(Data[i], i + 1);
            }

            CountCombinations(0, 0);

            return combinations;
        }
    }
}

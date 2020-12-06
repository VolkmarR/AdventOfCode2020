using AdventOfCode.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Day01
{
    public class Day01Tests
    {
        private readonly ITestOutputHelper Output;
        public Day01Tests(ITestOutputHelper output) => Output = output;

        [Fact]
        public void RunStep1() => Output.WriteLine(new Day01Solver().ExecutePuzzle1());
        
        [Fact]
        public void RunStep2() => Output.WriteLine(new Day01Solver().ExecutePuzzle2());
    }

    public class Day01Solver : SolverBase
    {
        List<int> Data;

        protected override void Parse(List<string> data)
            => Data = data.Select(q => int.Parse(q)).ToList();

        protected override object Solve1()
        {
            for (int i = 0; i < Data.Count; i++)
                for (int j = i +1; j < Data.Count; j++)
                {
                    if (Data[i] + Data[j] == 2020)
                        return (Data[i] * Data[j]);
                }

            throw new Exception("Solver Exception");
        }

        protected override object Solve2()
        {
            for (int i = 0; i < Data.Count; i++)
                for (int j = i + 1; j < Data.Count; j++)
                    for (int k = j + 1; k < Data.Count; k++)
                    {
                        if (Data[i] + Data[j] + Data[k] == 2020)
                        return (Data[i] * Data[j] * Data[k]);
                }

            throw new Exception("Solver Exception");
        }

    }
}

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
        protected override string Solve1(List<string> data)
        {
            var input = data.Select(q => int.Parse(q)).ToList();
            for (int i = 0; i < input.Count; i++)
                for (int j = i +1; j < input.Count; j++)
                {
                    if (input[i] + input[j] == 2020)
                        return (input[i] * input[j]).ToString();
                }

            return "??";
        }

        protected override string Solve2(List<string> data)
        {
            var input = data.Select(q => int.Parse(q)).ToList();
            for (int i = 0; i < input.Count; i++)
                for (int j = i + 1; j < input.Count; j++)
                    for (int k = j + 1; k < input.Count; k++)
                    {
                        if (input[i] + input[j] + input[k] == 2020)
                        return (input[i] * input[j] * input[k]).ToString();
                }
            return "??";
        }

    }
}

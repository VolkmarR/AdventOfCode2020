using AdventOfCode.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Day15
{
    public class Day15Tests
    {
        private readonly ITestOutputHelper Output;
        public Day15Tests(ITestOutputHelper output) => Output = output;

        [Fact]
        public void RunStep1() => Output.WriteLine(new Day15Solver().ExecutePuzzle1());
        
        [Fact]
        public void RunStep2() => Output.WriteLine(new Day15Solver().ExecutePuzzle2());
    }

    public class Day15Solver : SolverBase
    {
        List<int> Data;

        protected override void Parse(List<string> data)
        {
            Data = data[ 0].Split(',').Select(q => int.Parse(q)).ToList();
        }

        int Play(int rounds)
        {
            var lastSeen = new Dictionary<int, int>();

            for (int i = 1; i < Data.Count; i++)
                lastSeen[Data[i - 1]] = i;

            var number2 = Data[Data.Count - 2];
            var number1 = Data[Data.Count - 1];
            for (int i = Data.Count + 1; i <= rounds; i++)
            {
                lastSeen[number2] = i - 2;
                number2 = number1;
                if (lastSeen.ContainsKey(number1))
                    number1 = i - 1 - lastSeen[number1];
                else
                    number1 = 0;
            }

            return number1;
        }

        protected override object Solve1()
            => Play(2020);

        protected override object Solve2()
            => Play(30_000_000);
    }
}

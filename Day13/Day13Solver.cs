using AdventOfCode.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Day13
{
    public class Day13Tests
    {
        private readonly ITestOutputHelper Output;
        public Day13Tests(ITestOutputHelper output) => Output = output;

        [Fact]
        public void RunStep1() => Output.WriteLine(new Day13Solver().ExecutePuzzle1());

        [Fact]
        public void RunStep2() => Output.WriteLine(new Day13Solver().ExecutePuzzle2());
    }

    public class Day13Solver : SolverBase
    {
        int EarliestDepartTime;
        List<int?> Data;

        protected override void Parse(List<string> data)
        {
            EarliestDepartTime = int.Parse(data[0]);
            Data = data[1].Split(",").Select(q => q == "x" ? null : (int?)int.Parse(q)).ToList();
        }

        int? NextStartTime(int? busID)
        {
            if (busID == null)
                return null;

            int result = busID.Value * (EarliestDepartTime / busID.Value);
            if (result < EarliestDepartTime)
                result += busID.Value;
            return result;
        }

        protected override object Solve1()
        {
            var nextStart = Data.Select(NextStartTime).ToList();
            var best = nextStart.Where(q => q.HasValue).Min();
            return (best - EarliestDepartTime) * Data[nextStart.IndexOf(best)];
        }

        protected override object Solve2()
        {
            throw new Exception("Solver error");
        }
    }
}

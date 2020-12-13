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
        List<short> Data;
        List<short> DataOffset;

        protected override void Parse(List<string> data)
        {
            EarliestDepartTime = int.Parse(data[0]);
            Data = new List<short>();
            DataOffset = new List<short>();
            short offset = 0;
            var DataForSorting = new List<(short time, short offset)>();
            foreach (var item in data[1].Split(","))
            {
                if (item != "x")
                    DataForSorting.Add((short.Parse(item), offset));
                offset++;
            }

            DataForSorting.OrderByDescending(q => q.time);
            Data = DataForSorting.Select(q => q.time).ToList();
            DataOffset = DataForSorting.Select(q => q.offset).ToList();
        }

        static long NextStartTime(int busID, long timeStamp)
        {
            var result = busID * (timeStamp / busID);
            if (result < timeStamp)
                result += busID;
            return result;
        }

        protected override object Solve1()
        {
            var nextStart = Data.Select(q => NextStartTime(q, EarliestDepartTime)).ToList();
            var best = nextStart.Where(q => q > -1).Min();
            return (best - EarliestDepartTime) * Data[nextStart.IndexOf(best)];
        }

        protected override object Solve2()
        {
            // long startTimeStamp = NextStartTime(Data[0], 1013728);
            long startTimeStamp = NextStartTime(Data[0], 100_000_000_000_000);
            var counters = new long[Data.Count];
            for (var i = 0; i < Data.Count; i++)
                counters[i] = NextStartTime(Data[i], startTimeStamp) - DataOffset[i] - startTimeStamp;

            var notAllEqual = true;
            while (notAllEqual)
            {
                counters[0] += Data[0];
                notAllEqual = false;
                for (int i = 1; i < counters.Length && !notAllEqual; i++)
                {
                    while (counters[i] < counters[0])
                        counters[i] += Data[i];
                    notAllEqual = counters[0] != counters[i];
                }
            }

            return counters[0] + startTimeStamp;
        }
    }
}

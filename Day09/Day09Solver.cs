using AdventOfCode.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Day09
{
    public class Day09Tests
    {
        private readonly ITestOutputHelper Output;
        public Day09Tests(ITestOutputHelper output) => Output = output;

        [Fact]
        public void RunStep1() => Output.WriteLine(new Day09Solver().ExecutePuzzle1());

        [Fact]
        public void RunStep2() => Output.WriteLine(new Day09Solver().ExecutePuzzle2());
    }

    public class Day09Solver : SolverBase
    {
        List<long> Data;
        int PreableCount = 25;

        protected override void Parse(List<string> data)
        {
            Data = data.Select(q => long.Parse(q)).ToList();
        }

        bool ValueInPreable(int valueIndex)
        {
            var value = Data[valueIndex];
            for (int i = valueIndex - PreableCount; i < valueIndex; i++)
                for (int j = i + 1; j < valueIndex; j++)
                    if (Data[i] + Data[j] == value)
                        return true;
            return false;
        }

        (long value, int index) FindFirstValueMissingInPreamble()
        {
            for (var i = PreableCount; i < Data.Count; i++)
                if (!ValueInPreable(i))
                    return (Data[i], i);

            throw new Exception("Solver error");
        }

        protected override object Solve1()
            => FindFirstValueMissingInPreamble().value;

        protected override object Solve2()
        {
            (var missingValue, var index) = FindFirstValueMissingInPreamble();

            for (int i = 0; i < Data.Count - 1; i++)
            {
                var sum = Data[i];
                for (int j = i + 1; j < Data.Count; j++)
                {
                    sum += Data[j];
                    if (sum == missingValue)
                    {
                        var min = long.MaxValue;
                        var max = long.MinValue;
                        for (var k = i; k <= j; k++)
                        {
                            var currentValue = Data[k];
                            if (currentValue < min)
                                min = currentValue;
                            if (currentValue > max)
                                max = currentValue;
                        }

                        return min + max;
                    }
                    else if (sum > missingValue)
                        break;
                }

            }

            throw new Exception("Solver error");
        }
    }
}
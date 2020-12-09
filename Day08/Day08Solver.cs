using AdventOfCode.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Day08
{
    public class Day08Tests
    {
        private readonly ITestOutputHelper Output;
        public Day08Tests(ITestOutputHelper output) => Output = output;

        [Fact]
        public void RunStep1() => Output.WriteLine(new Day08Solver().ExecutePuzzle1());
        
        [Fact]
        public void RunStep2() => Output.WriteLine(new Day08Solver().ExecutePuzzle2());
    }

    public class Day08Solver : SolverBase
    {
        // List<??> Data;

        protected override void Parse(List<string> data)
        {
            // Data = new();
        }

        protected override object Solve1()
        {
            throw new Exception("Solver error");
        }

        protected override object Solve2()
        {
            throw new Exception("Solver error");
        }
    }
}

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
        Calculator Data;

        protected override void Parse(List<string> data)
        {
            Data = new(data);
        }

        bool ExecuteWithLoopDetection()
        {
            var pointer = new HashSet<int>();

            Data.Reset();
            while (Data.Running)
            {
                Data.ExecuteNextInstruction();
                if (pointer.Contains(Data.ExecutionPointer))
                    return false;

                pointer.Add(Data.ExecutionPointer);
            }

            return true;
        }

        protected override object Solve1()
        {
            ExecuteWithLoopDetection();
            return Data.GobalValue;
        }

        protected override object Solve2()
        {
            for (var i = 0; i < Data.Program.Count; i++)
            {
                var instruction = Data.Program[i];
                if (instruction.Operation == "jmp")
                    Data.Program[i] = instruction with { Operation = "nop" };
                else if (instruction.Operation == "nop" && instruction.Argument != 0)
                    Data.Program[i] = instruction with { Operation = "jmp" };
                else
                    continue;

                if (ExecuteWithLoopDetection())
                    return Data.GobalValue;

                Data.Program[i] = instruction;
            }

            throw new Exception("Solver error");
        }
    }
}

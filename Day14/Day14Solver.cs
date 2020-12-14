using AdventOfCode.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using System.Collections;

namespace Day14
{
    public class Day14Tests
    {
        private readonly ITestOutputHelper Output;
        public Day14Tests(ITestOutputHelper output) => Output = output;

        [Fact]
        public void RunStep1() => Output.WriteLine(new Day14Solver().ExecutePuzzle1());

        [Fact]
        public void RunStep2() => Output.WriteLine(new Day14Solver().ExecutePuzzle2());
    }


    public record Instruction(string mask, long address, long value);

    public record Mask(long maskAnd, long maskOr);

    public class Day14Solver : SolverBase
    {
        List<Instruction> Data;
        Dictionary<long, long> Mem = new Dictionary<long, long>();
        Mask CurrentMask = null;


        private Instruction ParseInstruction(string line)
        {
            if (line.StartsWith("mask ="))
                return new Instruction(line[7..], 0, 0);

            var parts = line.Split(StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries, '[', ']', '=');
            return new Instruction(null, long.Parse(parts[1]), uint.Parse(parts[2]));
        }

        Mask BuildMask(string mask)
        {
            var maskAnd = long.MaxValue; // to set 0
            var maskOr = 0L; // to set 1

            for (int i = 0; i < mask.Length; i++)
            {
                var bit = mask[mask.Length - 1 - i];
                if (bit != 'X')
                {
                    long bitValue = (long)Math.Pow(2, i);
                    if (bit == '0')
                        maskAnd -= bitValue;
                    else if (bit == '1')
                        maskOr += bitValue;
                }
            }
            return new Mask(maskAnd, maskOr);
        }

        protected override void Parse(List<string> data)
        {
            Data = data.Select(ParseInstruction).ToList();
        }

        long ApplyMask(long value)
            => (value & CurrentMask.maskAnd) | CurrentMask.maskOr;

        void Handle(Instruction instruction)
        {
            if (instruction.mask != null)
                CurrentMask = BuildMask(instruction.mask);
            else
                Mem[instruction.address] = ApplyMask(instruction.value);
        }

        protected override object Solve1()
        {
            foreach (var item in Data)
                Handle(item);

            return Mem.Values.Select(q => (long)q).Sum();
        }

        protected override object Solve2()
        {
            throw new Exception("Solver error");
        }
    }
}

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


    public record Instruction(string mask, uint address, long value);

    public class Day14Solver : SolverBase
    {
        List<Instruction> Data;
        Dictionary<long, long> Mem = new Dictionary<long, long>();
        string currentMask = null;


        private Instruction ParseInstruction(string line)
        {
            if (line.StartsWith("mask ="))
                return new Instruction(line[7..], 0, 0);

            var parts = line.Split(StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries,  '[', ']', '=');
            return new Instruction(null, uint.Parse(parts[1]), uint.Parse(parts[2]));
        }

        protected override void Parse(List<string> data)
        {
            Data = data.Select(ParseInstruction).ToList();
        }

        long ApplyMask(long value)
        {
            var result = new BitArray(BitConverter.GetBytes(value));

            for (int i = 0; i < currentMask.Length; i++)
                if (currentMask[i] != 'X')
                    result.Set(currentMask.Length - i - 1, currentMask[i] == '1');
            //         100000000 (9)
            //         0
            //  0000000100000000
            //  16 - (16 - 9 + i) =  
            var newValue = 0L;
            for (int i = 0; i < result.Length; i++)
                if (result[i])
                    newValue += (long)Math.Pow(2, i);

            return newValue;
        }

        void Handle(Instruction instruction)
        {
            if (instruction.mask != null)
                currentMask = instruction.mask;
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

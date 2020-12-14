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
        List<Mask> CurrentAddressMasks = new List<Mask>();
        StringBuilder Builder = new StringBuilder(36);

        private Instruction ParseInstruction(string line)
        {
            if (line.StartsWith("mask ="))
                return new Instruction(line[7..], 0, 0);

            var parts = line.Split(StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries, '[', ']', '=');
            return new Instruction(null, long.Parse(parts[1]), uint.Parse(parts[2]));
        }

        Mask BuildMask(string mask)
        {
            var maskAnd = (long)Math.Pow(2, 37) - 1; // to set 0
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

        void AssignAddressMask(string mask)
        {
            CurrentMask = BuildMask(mask);
            CurrentAddressMasks.Clear();
            AddAddressMask(mask, 0);
        }

        string SetBit(string mask, int index, char value)
            => mask[0..index] + value + mask[(index + 1)..];

        void AddAddressMask(string mask, int startBit)
        {
            for (int i = startBit; i < mask.Length; i++)
            {
                if (mask[i] == 'X')
                {
                    AddAddressMask(SetBit(mask, i, '0'), i + 1);
                    AddAddressMask(SetBit(mask, i, '1'), i + 1);
                    return;
                }
                else
                    mask = SetBit(mask, i, 'X');
            }

            CurrentAddressMasks.Add(BuildMask(mask));
        }

        protected override void Parse(List<string> data)
        {
            Data = data.Select(ParseInstruction).ToList();
        }

        long ApplyMask1(Mask mask, long value)
            => (value & mask.maskAnd) | mask.maskOr;

        long ApplyMask2(Mask mask, long value)
            => value | mask.maskOr;

        protected override object Solve1()
        {
            foreach (var item in Data)
            {
                if (item.mask != null)
                    CurrentMask = BuildMask(item.mask);
                else
                    Mem[item.address] = ApplyMask1(CurrentMask, item.value);
            }

            return Mem.Values.Sum();
        }

        protected override object Solve2()
        {
            foreach (var item in Data)
            {
                if (item.mask != null)
                    AssignAddressMask(item.mask);
                else
                {
                    var address = ApplyMask2(CurrentMask, item.address);
                    foreach (var addressMask in CurrentAddressMasks)
                    {
                        var address2 = ApplyMask1(addressMask, address);
                        Mem[address2] = item.value;
                    }
                }
            }

            return Mem.Values.Sum();
        }
    }
}

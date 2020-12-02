using AdventOfCode.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Day02
{
    public class Day02Tests
    {
        private readonly ITestOutputHelper Output;
        public Day02Tests(ITestOutputHelper output) => Output = output;

        [Fact]
        public void RunStep1() => Output.WriteLine(new Day02Solver().ExecutePuzzle1());

        [Fact]
        public void RunStep2() => Output.WriteLine(new Day02Solver().ExecutePuzzle2());
    }

    public record Password(int min, int max, char letter, string password);

    public class Day02Solver : SolverBase
    {
        Password Parse(string line)
        {
            var parts = line.Split(new char[] { '-', ':', ' ', }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            return new Password(int.Parse(parts[0]), int.Parse(parts[1]), parts[2][0], parts[3]);
        }

        bool Valid1(Password password)
        {
            var letterCount = password.password.Count(q => q == password.letter);
            return letterCount >= password.min && letterCount <= password.max;
        }
        bool Valid2(Password password)
        {
            return password.password.ElementAtOrDefault(password.min - 1) == password.letter ^ password.password.ElementAtOrDefault(password.max - 1) == password.letter;
        }

        protected override string Solve1(List<string> data)
            => data.Select(Parse).Count(Valid1).ToString();

        protected override string Solve2(List<string> data)
            => data.Select(Parse).Count(Valid2).ToString();


    }
}

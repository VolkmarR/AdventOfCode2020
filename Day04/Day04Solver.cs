using AdventOfCode.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using System.Text.RegularExpressions;

namespace Day04
{
    public class Day04Tests
    {
        private readonly ITestOutputHelper Output;
        public Day04Tests(ITestOutputHelper output) => Output = output;

        [Fact]
        public void RunStep1() => Output.WriteLine(new Day04Solver().ExecutePuzzle1());

        [Fact]
        public void RunStep2() => Output.WriteLine(new Day04Solver().ExecutePuzzle2());
    }

    public class Day04Solver : SolverBase
    {
        List<Dictionary<string, string>> Passports;

        void ParseAndAddItem(string current)
        {
            if (!string.IsNullOrWhiteSpace(current))
            {
                var passport = new Dictionary<string, string>();
                foreach (var element in current
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(q => q.Split(':')))
                {
                    passport.Add(element[0], element[1]);
                }

                Passports.Add(passport);
            }
        }

        protected override void Parse(List<string> data)
        {
            Passports = new List<Dictionary<string, string>>();

            var currentData = "";
            foreach (var line in data)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    ParseAndAddItem(currentData);
                    currentData = "";
                }
                else
                    currentData += line + " ";
            }

            ParseAndAddItem(currentData);
        }

        static readonly List<string> ValidKeys1 = new List<string> { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };
        static readonly HashSet<string> ValidEyeColors = new HashSet<string> { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };

        bool Valid1(Dictionary<string, string> passport)
            => ValidKeys1.All(q => passport.ContainsKey(q));

        static bool ValidIntBetween(string value, int min, int max)
            => int.TryParse(value, out var valueInt) && valueInt >= min && valueInt <= max;

        static bool ValidHeight(string value)
        {
            if (value.EndsWith("cm"))
                return ValidIntBetween(value.Substring(0, value.Length - 2), 150, 193);
            else if (value.EndsWith("in"))
                return ValidIntBetween(value.Substring(0, value.Length - 2), 59, 76);
            else
                return false;
        }

        static bool ValidHairColor(string value)
            => Regex.IsMatch(value, "#[a-f0-9]{6}");

        static bool ValidEyeColor(string value)
            => ValidEyeColors.Contains(value);

        static bool ValidPassportID(string value)
            => value.Length == 9 && value.All(q => char.IsDigit(q));

        Dictionary<string, Func<string, bool>> ValidKeys2 = new Dictionary<string, Func<string, bool>>
        {
            { "byr", q => ValidIntBetween(q, 1920, 2002) },
            { "iyr", q => ValidIntBetween(q, 2010, 2020) },
            { "eyr", q => ValidIntBetween(q, 2020, 2030) },
            { "hgt", q => ValidHeight(q) },
            { "hcl", q => ValidHairColor(q) },
            { "ecl", q => ValidEyeColor(q) },
            { "pid", q => ValidPassportID(q) }
        };

        bool Valid2(Dictionary<string, string> passport)
            => ValidKeys2.Keys.All(q => passport.TryGetValue(q, out var value) && ValidKeys2[q](value));

        protected override object Solve1()
            => Passports.Count(Valid1);

        protected override object Solve2()
            => Passports.Count(Valid2);

    }
}

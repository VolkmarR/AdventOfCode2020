using AdventOfCode.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Day07
{
    public class Day07Tests
    {
        private readonly ITestOutputHelper Output;
        public Day07Tests(ITestOutputHelper output) => Output = output;

        [Fact]
        public void RunStep1() => Output.WriteLine(new Day07Solver().ExecutePuzzle1());
        
        [Fact]
        public void RunStep2() => Output.WriteLine(new Day07Solver().ExecutePuzzle2());
    }

    record Rule(string color, List<RuleItem> canContain);

    record RuleItem(string color, int count);


    public class Day07Solver : SolverBase
    {

        List<Rule> Data;

        RuleItem ParseRuleItem(string item)
        {
            var parts = item.Split(' ', 2);
            return new RuleItem(parts[1], int.Parse(parts[0]));
        }

        Rule ParseRule(string line)
        {
            line = line.Replace("bags", "").Replace("bag", "").Replace("no other", "").Replace(".", "");

            var parts = line.Split(StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries, " contain ", ",");
            return new Rule(parts[0], parts.Skip(1).Select(ParseRuleItem).ToList());
        }

        protected override void Parse(List<string> data)
            => Data = data.Select(ParseRule).ToList();

        List<Rule> MatchRule(List<string> color)
            => Data.Where(q => color.Any(c => q.canContain.Any(s => s.color == c))).ToList();

        protected override object Solve1()
        {
            var colors = "shiny gold".ToList();
            var rulesCount = 0;
            while (true)
            {
                var rules = MatchRule(colors);
                if (rules.Count == rulesCount)
                    return rules.Count;

                rulesCount = rules.Count;
                colors = colors.Union(rules.Select(q => q.color)).Distinct().ToList();
            }
        }

        int CountBags(string color)
        {
            var rule = Data.FirstOrDefault(q => q.color == color);
            if (rule == null)
                return 0;

            var result = 0;
            foreach (var item in rule.canContain)
                result += item.count + item.count * CountBags(item.color);

            return result;
        }


        protected override object Solve2()
            => CountBags("shiny gold");
    }
}

using AdventOfCode.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Day16
{
    public class Day16Tests
    {
        private readonly ITestOutputHelper Output;
        public Day16Tests(ITestOutputHelper output) => Output = output;

        [Fact]
        public void RunStep1() => Output.WriteLine(new Day16Solver().ExecutePuzzle1());
        
        [Fact]
        public void RunStep2() => Output.WriteLine(new Day16Solver().ExecutePuzzle2());
    }

    public record RuleBetween (int start, int end);
    public record Rule(string name, RuleBetween[] rules);
    public record Ticket(int[] numbers);

    public class Day16Solver : SolverBase
    {
        List<Rule> Rules;
        List<Ticket> Tickets;

        void ParseRules(IEnumerable<string> data)
        {
            foreach (var item in data)
            {
                var parts = item.Split(StringSplitOptions.TrimEntries, ":");
                var partsBetween = parts[1].Split(StringSplitOptions.TrimEntries, "-", "or");
                Rules.Add(new Rule(parts[0], new RuleBetween[]
                            {
                                new RuleBetween(int.Parse(partsBetween[0]), int.Parse(partsBetween[1])),
                                new RuleBetween(int.Parse(partsBetween[2]), int.Parse(partsBetween[3]))
                            }));

            }

        }

        void ParseTickets(IEnumerable<string> data)
        {
            foreach (var item in data.Skip(1))
                Tickets.Add(new Ticket(item.Split(',').Select(int.Parse).ToArray()));
        }

        protected override void Parse(List<string> data)
        {
            Rules = new List<Rule>();
            Tickets = new List<Ticket>();

            Func<string, bool> NotEmptyLine = q => q != "";

            ParseRules(data.TakeWhile(NotEmptyLine));

            ParseTickets(data.SkipWhile(NotEmptyLine).Skip(1).TakeWhile(NotEmptyLine));
            ParseTickets(data.SkipWhile(NotEmptyLine).Skip(1).SkipWhile(NotEmptyLine).Skip(1));
        }

        protected override object Solve1()
        {
            var invalidNumbers = new List<int>();
            var rulesBetween = Rules.SelectMany(q => q.rules, (a, b) => b).ToList();


            foreach (var item in Tickets)
            {
                var invalid = item.numbers.Where(q => !rulesBetween.Any(s => q >= s.start && q <= s.end)).ToList();
                if (invalid.Count > 0)
                    invalidNumbers.AddRange(invalid);
            }

            return invalidNumbers.Sum();
        }

        protected override object Solve2()
        {
            throw new Exception("Solver error");
        }
    }
}

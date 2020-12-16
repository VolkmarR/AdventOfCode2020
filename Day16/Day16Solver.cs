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

    public record RuleBetween(int start, int end);
    public record Rule(string name, RuleBetween[] rules);
    public record Ticket(int[] numbers);

    public class Day16Solver : SolverBase
    {
        List<Rule> Rules;
        List<Ticket> Tickets;

        #region Parse

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
        #endregion

        #region Helper methods
        List<RuleBetween> GetAllRulesBetween() => Rules.SelectMany(q => q.rules, (a, b) => b).ToList();

        bool MatchRuleBetween(RuleBetween rule, int value)
            => value >= rule.start && value <= rule.end;

        bool MatchRule(Rule rule, int value)
            => MatchRuleBetween(rule.rules[0], value) || MatchRuleBetween(rule.rules[1], value);

        Rule OnlyOneRuleMatch(List<int> values, List<Rule> rules)
        {
            Rule result = null;
            foreach (var rule in rules)
                if (values.All(q => MatchRule(rule, q)))
                {
                    if (result == null)
                        result = rule;
                    else
                        return null;
                }

            return result;
        }

        void RemoveInvalidTickets()
        {
            var rulesBetween = GetAllRulesBetween();
            var i = 0;
            while (i < Tickets.Count)
                if (Tickets[i].numbers.Any(q => !rulesBetween.Any(s => MatchRuleBetween(s, q))))
                    Tickets.RemoveAt(i);
                else
                    i++;
        }
        #endregion

        protected override object Solve1()
        {
            var invalidNumbers = new List<int>();
            var rulesBetween = GetAllRulesBetween();

            foreach (var item in Tickets)
            {
                var invalid = item.numbers.Where(q => !rulesBetween.Any(s => MatchRuleBetween(s, q))).ToList();
                if (invalid.Count > 0)
                    invalidNumbers.AddRange(invalid);
            }

            return invalidNumbers.Sum();
        }




        protected override object Solve2()
        {
            RemoveInvalidTickets();

            var RuleForRow = new Dictionary<int, Rule>();
            var NumberRows = new List<List<int>>();
            for (int i = 0; i < Tickets[0].numbers.Length; i++)
                NumberRows.Add(Tickets.Select(q => q.numbers[i]).ToList());

            var rules = Rules.ToList();

            while (rules.Count > 0)
            {
                for (int i = 0; i < NumberRows.Count; i++)
                    if (!RuleForRow.ContainsKey(i))
                    {
                        var onlyRule = OnlyOneRuleMatch(NumberRows[i], rules);
                        if (onlyRule != null)
                        {
                            RuleForRow[i] = onlyRule;
                            rules.Remove(onlyRule);
                        }
                    }
            }

            var result = 1L;
            foreach (var item in RuleForRow)
                if (item.Value.name.StartsWith("departure"))
                    result *= Tickets[0].numbers[item.Key];

            return result;
        }
    }
}

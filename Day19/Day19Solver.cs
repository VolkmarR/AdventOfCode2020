using AdventOfCode.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using System.Text.RegularExpressions;

namespace Day19
{
    public class Day19Tests
    {
        private readonly ITestOutputHelper Output;
        public Day19Tests(ITestOutputHelper output) => Output = output;

        [Fact]
        public void RunStep1() => Output.WriteLine(new Day19Solver().ExecutePuzzle1());

        [Fact]
        public void RunStep2() => Output.WriteLine(new Day19Solver().ExecutePuzzle2());
    }

    class Rule
    {
        public string Text { get; set; }
        public string RegEx { get; set; }

    }

    public class Day19Solver : SolverBase
    {
        List<string> Data;
        SortedDictionary<int, Rule> Rules;

        void SetRegex(Rule rule)
        {
            if (!string.IsNullOrEmpty(rule.RegEx))
                return;

            if (rule.Text.StartsWith('"') && rule.Text.EndsWith('"'))
            {
                rule.RegEx = rule.Text[1].ToString();
                return;
            }

            var regexList = new List<string>();
            var regex = new StringBuilder();
            foreach (var ruleGroup in rule.Text.Split(StringSplitOptions.TrimEntries, " | "))
            {
                regex.Length = 0;
                foreach (var ruleLink in ruleGroup.Split(' ').Select(q => q.ToInt()))
                {
                    var linkedRule = Rules[ruleLink];
                    SetRegex(linkedRule);
                    if (linkedRule.RegEx.Contains('|'))
                        regex.Append($"({linkedRule.RegEx})");
                    else
                        regex.Append(linkedRule.RegEx);
                }
                regexList.Add(regex.ToString());
            }

            if (regexList.Count == 1)
                rule.RegEx = regexList[0];
            else
                rule.RegEx = "(" + regexList.Join(")|(") + ")";
        }

        protected override void Parse(List<string> data)
        {
            Rules = new();

            var i = 0;
            for (; i < data.Count && data[i] != ""; i++)
            {
                var parts = data[i].Split(StringSplitOptions.TrimEntries, ": ");
                Rules[parts[0].ToInt()] = new Rule { Text = parts[1] };
            }

            i++;
            Data = data.Skip(i).ToList();

            foreach (var rule in Rules.Values)
                SetRegex(rule);
        }

        protected override object Solve1()
        {
            var rx = new Regex(Rules[0].RegEx);
            var count = 0;
            foreach (var item in Data)
            {
                var x = rx.Match(item);
                if (x?.Value == item)
                    count++;
            }

            return count;
        }

        protected override object Solve2()
        {
            throw new Exception("Solver error");
        }
    }
}

using AdventOfCode.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Day06
{
    public class Day06Tests
    {
        private readonly ITestOutputHelper Output;
        public Day06Tests(ITestOutputHelper output) => Output = output;

        [Fact]
        public void RunStep1() => Output.WriteLine(new Day06Solver().ExecutePuzzle1());
        
        [Fact]
        public void RunStep2() => Output.WriteLine(new Day06Solver().ExecutePuzzle2());
    }

    public record Group (List<string> Answers);
    public class Day06Solver : SolverBase
    {
        List<Group> Groups;

        protected override void Parse(List<string> data)
        {
            Group group = null;
            void AddGroup()
            {
                group = new Group(new List<string>());
                Groups.Add(group);
            }

            Groups = new();
            AddGroup();
            foreach (var item in data)
            {
                if (item == "")
                    AddGroup();
                else
                    group.Answers.Add(item);
            }
        }


        protected override object Solve1()
        {
            var result = 0;
            foreach(var group in Groups)
            {
                var answers = new HashSet<char>();
                foreach (var answer in group.Answers)
                    foreach (var item in answer)
                        answers.Add(item);
                result += answers.Count;
            }

            return result;
        }

        protected override object Solve2()
        {
            var result = 0;
            foreach (var group in Groups)
            {
                var answers = new Dictionary<char, int>();
                foreach (var answer in group.Answers)
                    foreach (var item in answer)
                        answers.IncrementCount(item);

                result += answers.Count(q => q.Value == group.Answers.Count);
            }

            return result;
        }

    }
}

using AdventOfCode.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Day22
{
    public class Day22Tests
    {
        private readonly ITestOutputHelper Output;
        public Day22Tests(ITestOutputHelper output) => Output = output;

        [Fact]
        public void RunStep1() => Output.WriteLine(new Day22Solver().ExecutePuzzle1());
        
        [Fact]
        public void RunStep2() => Output.WriteLine(new Day22Solver().ExecutePuzzle2());
    }

    public class Day22Solver : SolverBase
    {
        Dictionary<int, Queue<int>> Players;

        void ParsePlayer(List<string> data)
        {
            var playerID = data[0][7..8].ToInt();
            var queue = new Queue<int>();
            Players[playerID] = queue;

            foreach (var item in data.Skip(1))
                queue.Enqueue(item.ToInt());
        }

        protected override void Parse(List<string> data)
        {
            Players = new();

            ParsePlayer(data.TakeWhile(q => q != "").ToList());
            ParsePlayer(data.SkipWhile(q => q != "").Skip(1).ToList());
        }

        List<int> PlayGame()
        {
            var round = new Dictionary<int, int>();
            while (Players.Values.All(q => q.Count > 0))
            {
                round.Clear();
                foreach (var player in Players.Keys)
                    round[player] = Players[player].Dequeue();

                var winner = round.OrderByDescending(q => q.Value).ToList();
                foreach (var item in winner)
                    Players[winner[0].Key].Enqueue(item.Value);
            }

            return Players.FirstOrDefault(q => q.Value.Count > 0).Value.ToList();
        }

        protected override object Solve1()
        {
            var gameResult = PlayGame();
            var result = 0;
            var multiplier = gameResult.Count;
            foreach (var item in gameResult)
            {
                result += item * multiplier;
                multiplier--;
            }
            return result;
        }

        protected override object Solve2()
        {
            throw new Exception("Solver error");
        }
    }
}

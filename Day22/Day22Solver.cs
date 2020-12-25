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
        Dictionary<int, Queue<int>> Game;

        void ParsePlayer(List<string> data)
        {
            var playerID = data[0][7..8].ToInt();
            var queue = new Queue<int>();
            Game[playerID] = queue;

            foreach (var item in data.Skip(1))
                queue.Enqueue(item.ToInt());
        }

        protected override void Parse(List<string> data)
        {
            Game = new();

            ParsePlayer(data.TakeWhile(q => q != "").ToList());
            ParsePlayer(data.SkipWhile(q => q != "").Skip(1).ToList());
        }

        int PlayGame()
        {
            var round = new Dictionary<int, int>();
            while (Game.Values.All(q => q.Count > 0))
            {
                round.Clear();
                foreach (var player in Game.Keys)
                    round[player] = Game[player].Dequeue();

                var winner = round.OrderByDescending(q => q.Value).ToList();
                foreach (var item in winner)
                    Game[winner[0].Key].Enqueue(item.Value);
            }

            return Game.FirstOrDefault(q => q.Value.Count > 0).Key;
        }

        int PlayRecursiveGame(Dictionary<int, Queue<int>> game)
        {
            var round = new Dictionary<int, int>();
            var lastRounds = new HashSet<string>();
            while (game.Values.All(q => q.Count > 0))
            {
                var currentCards = game.Select(q => q.Value.ToArray().Select(v => v.ToString()).Join(",")).Join("/");
                if (lastRounds.Contains(currentCards))
                    return 1;
                lastRounds.Add(currentCards);

                round.Clear();
                foreach (var player in game.Keys)
                    round[player] = game[player].Dequeue();

                if (game.Keys.All(q => game[q].Count >= round[q]))
                {
                    // reverse Combat
                    var subGame = new Dictionary<int, Queue<int>>();
                    foreach (var player in game.Keys)
                        subGame[player] = new Queue<int>(game[player].ToArray().Take(round[player]));
                    var winner = PlayRecursiveGame(subGame);
                    game[winner].Enqueue(round.First(q => q.Key == winner).Value);
                    game[winner].Enqueue(round.First(q => q.Key != winner).Value);
                }
                else
                {
                    var winner = round.OrderByDescending(q => q.Value).ToList();
                    foreach (var item in winner)
                        game[winner[0].Key].Enqueue(item.Value);
                }
            }

            return game.FirstOrDefault(q => q.Value.Count > 0).Key;
        }

        int WinningScore(int winner)
        {
            var result = 0;
            var multiplier = Game[winner].Count;
            foreach (var item in Game[winner].ToArray())
            {
                result += item * multiplier;
                multiplier--;
            }
            return result;
        }

        protected override object Solve1()
            => WinningScore(PlayGame());

        protected override object Solve2()
            => WinningScore(PlayRecursiveGame(Game));
    }
}

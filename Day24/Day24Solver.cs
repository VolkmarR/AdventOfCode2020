using AdventOfCode.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Day24
{
    public class Day24Tests
    {
        private readonly ITestOutputHelper Output;
        public Day24Tests(ITestOutputHelper output) => Output = output;

        [Fact]
        public void RunStep1() => Output.WriteLine(new Day24Solver().ExecutePuzzle1());

        [Fact]
        public void RunStep2() => Output.WriteLine(new Day24Solver().ExecutePuzzle2());
    }

    public class Day24Solver : SolverBase
    {
        List<List<string>> Data;
        HashSet<(int, int)> BlackTiles = new();

        void ParseLine(string line)
        {
            var moves = new List<string>();
            string lastChar = "";
            foreach (char item in line)
            {
                if (item == 's' || item == 'n')
                    lastChar = item.ToString();
                else
                {
                    moves.Add(lastChar + item.ToString());
                    lastChar = "";
                }
            }

            Data.Add(moves);
        }

        protected override void Parse(List<string> data)
        {
            Data = new();
            foreach (string item in data)
                ParseLine(item);
        }

        (int x, int y) Step(string move, (int x, int y) pos)
        {
            if (move == "ne")
                return (pos.x + 1, pos.y + 1);
            if (move == "e")
                return (pos.x + 2, pos.y);
            if (move == "se")
                return (pos.x + 1, pos.y - 1);
            if (move == "sw")
                return (pos.x - 1, pos.y - 1);
            if (move == "w")
                return (pos.x - 2, pos.y);
            if (move == "nw")
                return (pos.x - 1, pos.y + 1);

            return pos;
        }


        (int x, int y) NavigateToTile(List<string> moves, (int x, int y) pos)
        {
            foreach (string move in moves)
                pos = Step(move, pos);

            return pos;
        }

        void FlipTile((int x, int y) pos)
        {
            if (BlackTiles.Contains(pos))
                BlackTiles.Remove(pos);
            else
                BlackTiles.Add(pos);
        }

        protected override object Solve1()
        {
            foreach (var moves in Data)
                FlipTile(NavigateToTile(moves, (0, 0)));

            return BlackTiles.Count();
        }

        protected override object Solve2()
        {
            throw new Exception("Solver error");
        }
    }
}

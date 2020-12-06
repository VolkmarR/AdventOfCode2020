using AdventOfCode.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Day03
{
    public class Day03Tests
    {
        private readonly ITestOutputHelper Output;
        public Day03Tests(ITestOutputHelper output) => Output = output;

        [Fact]
        public void RunStep1() => Output.WriteLine(new Day03Solver().ExecutePuzzle1());
        
        [Fact]
        public void RunStep2() => Output.WriteLine(new Day03Solver().ExecutePuzzle2());
    }

    public class Day03Solver : SolverBase
    {
        int X;
        int Y;
        int X_max;
        int Y_max;
        bool[,] Map;
        
        protected override void Parse(List<string> data)
        {
            X_max = data[0].Length;
            Y_max = data.Count;
            Map = new bool[X_max, Y_max];
            for (int y = 0; y < Y_max; y++)
                for (int x = 0; x < X_max; x++)
                    Map[x, y] = data[y][x] == '#';
        }

        void Move(int deltaX, int deltaY)
        {
            X += deltaX; 
            Y += deltaY;
            if (X >= X_max)
                X -= X_max;
        }

        int CountTrees(int deltaX, int deltaY)
        {
            X = 0;
            Y = 0;
            var trees = 0;
            while (Y < Y_max)
            {
                if (Map[X, Y])
                    trees++;
                Move(deltaX, deltaY);
            }
            return trees;
        }

        protected override object Solve1()
            => CountTrees(3, 1);

        protected override object Solve2()
        {
            var deltas = new List<(int x, int y)> { (1, 1), (3, 1), (5, 1), (7, 1), (1, 2) };
            long trees = 1;
            foreach ((int x, int y) in deltas)
                trees *= CountTrees(x, y);

            return trees;
        }

    }
}

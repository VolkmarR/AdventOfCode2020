using AdventOfCode.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Day11
{
    public class Day11Tests
    {
        private readonly ITestOutputHelper Output;
        public Day11Tests(ITestOutputHelper output) => Output = output;

        [Fact]
        public void RunStep1() => Output.WriteLine(new Day11Solver().ExecutePuzzle1());

        [Fact]
        public void RunStep2() => Output.WriteLine(new Day11Solver().ExecutePuzzle2());
    }

    public class Day11Solver : SolverBase
    {
        char[,] Data;
        int MaxX;
        int MaxY;

        char[,] Init()
        {
            var result = new char[MaxX, MaxY];
            for (int x = 0; x < MaxX; x++)
            {
                result[x, 0] = '.';
                result[x, MaxY - 1] = '.';
            }

            for (int y = 0; y < MaxY; y++)
            {
                result[0, y] = '.';
                result[MaxX - 1, y] = '.';
            }

            return result;
        }

        IEnumerable<(int x, int y)> AllSeats()
        {
            for (int y = 1; y < MaxY - 1; y++)
                for (int x = 1; x < MaxX - 1; x++)
                    yield return (x, y);
        }

        protected override void Parse(List<string> data)
        {
            MaxX = data[0].Length + 2;
            MaxY = data.Count + 2;
            Data = Init();

            foreach ((var x, var y) in AllSeats())
                Data[x, y] = data[y - 1][x - 1];
        }

        int CountAdjacentSeats(char[,] data, int x, int y)
        {
            int CountAdjacentSeat(int deltaX, int deltaY) => data[x + deltaX, y + deltaY] == '#' ? 1 : 0;
            return CountAdjacentSeat(-1, -1) + CountAdjacentSeat(-1, 0) + CountAdjacentSeat(-1, 1) +
                CountAdjacentSeat(0, -1) + CountAdjacentSeat(0, 1) +
                CountAdjacentSeat(1, -1) + CountAdjacentSeat(1, 0) + CountAdjacentSeat(1, 1);
        }


        char[,] Fill(char[,] data)
        {
            var result = Init();
            foreach ((var x, var y) in AllSeats())
            {
                var count = data[x, y] != '.' ? CountAdjacentSeats(data, x, y) : 0;
                if (data[x, y] == 'L' && count == 0)
                    result[x, y] = '#';
                else if (data[x, y] == '#' && count >= 4)
                    result[x, y] = 'L';
                else
                    result[x, y] = data[x, y];
            }

            return result;
        }

        bool Equal(char[,] item1, char[,] item2)
        {
            foreach ((var x, var y) in AllSeats())
                if (item1[x, y] != item2[x, y])
                    return false;
            return true;
        }

        int CountOccupiedSeats(char[,] item)
            => AllSeats().Sum(q => item[q.x, q.y] == '#' ? 1 : 0);

        protected override object Solve1()
        {
            var newData = Data;
            var data = Data;
            do
            {
                data = newData;
                newData = Fill(data);
            } while (!Equal(data, newData));

            return CountOccupiedSeats(newData);
        }

        protected override object Solve2()
        {
            throw new Exception("Solver error");
        }
    }
}

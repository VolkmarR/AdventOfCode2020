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

        int CountAdjacentSeat(int x, int y, int deltaX, int deltaY) => Data[x + deltaX, y + deltaY] == '#' ? 1 : 0;

        int CountVisibleAdjacentSeat(int x, int y, int deltaX, int deltaY)
        {
            var nX = x + deltaX;
            var nY = y + deltaY;
            while (nX > 0 && nY > 0 && nX < MaxX && nY < MaxY)
            {
                if (Data[nX, nY] == '#')
                    return 1;
                if (Data[nX, nY] == 'L')
                    return 0;
                nX = nX + deltaX;
                nY = nY + deltaY;
            }
            return 0;
        }

        char MutateSeat(int x, int y, Func<int, int, int, int, int> countAdjacentSeat, int maxCountForClear)
        {
            var countOccpuied = countAdjacentSeat(x, y, -1, -1) + countAdjacentSeat(x, y, -1, 0) + countAdjacentSeat(x, y, -1, 1) +
                countAdjacentSeat(x, y, 0, -1) + countAdjacentSeat(x, y, 0, 1) +
                countAdjacentSeat(x, y, 1, -1) + countAdjacentSeat(x, y, 1, 0) + countAdjacentSeat(x, y, 1, 1);

            if (Data[x, y] == 'L' && countOccpuied == 0)
                return '#';
            if (Data[x, y] == '#' && countOccpuied >= maxCountForClear)
                return 'L';

            return Data[x, y];
        }

        char[,] Fill(Func<int, int, int, int, int> countAdjacentSeat, int maxCountForClear)
        {
            var result = Init();
            foreach ((var x, var y) in AllSeats())
                result[x, y] = MutateSeat(x, y, countAdjacentSeat, maxCountForClear);

            return result;
        }

        bool Equal(char[,] compareItem)
            => AllSeats().All(q => Data[q.x, q.y] == compareItem[q.x, q.y]);

        int CountOccupiedSeats(char[,] item)
            => AllSeats().Sum(q => item[q.x, q.y] == '#' ? 1 : 0);

        int Solve(Func<int, int, int, int, int> countAdjacentSeat, int maxCountForClear)
        {
            var newData = Data;
            do
            {
                Data = newData;
                newData = Fill(countAdjacentSeat, maxCountForClear);
            } while (!Equal(newData));

            return CountOccupiedSeats(newData);
        }
        protected override object Solve1() => Solve(CountAdjacentSeat, 4);

        protected override object Solve2() => Solve(CountVisibleAdjacentSeat, 5);
    }
}

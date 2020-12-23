using AdventOfCode.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Day23
{
    public class Day23Tests
    {
        private readonly ITestOutputHelper Output;
        public Day23Tests(ITestOutputHelper output) => Output = output;

        [Fact]
        public void RunStep1() => Output.WriteLine(new Day23Solver().ExecutePuzzle1());
        
        [Fact]
        public void RunStep2() => Output.WriteLine(new Day23Solver().ExecutePuzzle2());
    }

    public class Day23Solver : SolverBase
    {
        LinkedList<int> Data = new();
        Dictionary<int, LinkedListNode<int>> Index = new();
        List<int> PickedUp = new();
        LinkedListNode<int> Current;
        int Max;

        protected override void Parse(List<string> data)
        {
            foreach (int item in data[0].Select(q => q.ToString().ToInt()))
                Index[item] = Data.AddLast(item);
            Current = Data.First;
            Max = Index.Keys.Max();
        }

        LinkedListNode<int> Next(LinkedListNode<int> item)
            => item.Next ?? item.List.First;

        int Destination()
        {
            var result = Current.Value;
            do
            {
                result--;
                if (result < 1)
                    result = Max;
            } while (PickedUp.Contains(result));

            return result;
        }

        void PlayRound()
        {
            PickedUp.Clear();
            var pickup = Next(Current);
            for (int i = 0; i < 3; i++)
            {
                PickedUp.Add(pickup.Value);
                pickup = Next(pickup);
            }

            var destination = Index[Destination()];
            foreach (var item in PickedUp)
            {
                Data.Remove(Index[item]);
                destination = Data.AddAfter(destination, item);
                Index[item] = destination;
            }

            Current = Next(Current);
        }

        string GetResultNumber()
        {
            var result = "";
            var current = Next(Index[1]);
            for (int i = 0; i < Index.Count - 1; i++)
            {
                result += current.Value.ToString();
                current = Next(current);
            }

            return result;
        }

        protected override object Solve1()
        {
            for (int i = 1; i <= 100; i++)
                PlayRound();

            return GetResultNumber();
        }

        protected override object Solve2()
        {
            for (int i = Max + 1; i <= 1000000; i++)
                Index[i] = Data.AddLast(i);
            Max = 1000000;

            for (int i = 1; i <= 10000000; i++)
                PlayRound();

            var resultNode = Index[1];
            var value1 = Next(resultNode);
            var value2 = Next(value1);

            return (long)value1.Value * (long)value2.Value;
        }
    }
}

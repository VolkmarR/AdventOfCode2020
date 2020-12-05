using AdventOfCode.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Day05
{
    public class Day05Tests
    {
        private readonly ITestOutputHelper Output;
        public Day05Tests(ITestOutputHelper output) => Output = output;

        [Fact]
        public void RunStep1() => Output.WriteLine(new Day05Solver().ExecutePuzzle1());

        [Fact]
        public void RunStep2() => Output.WriteLine(new Day05Solver().ExecutePuzzle2());
    }

    public record Ticket(int Row, int Column)
    {
        public int SeatID => Row * 8 + Column;
    }

    public class Day05Solver : SolverBase
    {
        List<Ticket> Tickets;

        bool IsLower(char direction) => direction is 'F' or 'L';

        int ParsePath(string path, int count)
        {
            int low = 0;
            int high = count - 1;
            for (var i = 0; i < path.Length - 1; i++)
            {
                count = count / 2;
                if (IsLower(path[i]))
                    high -= count;
                else
                    low += count;
            }
            if (IsLower(path.Last()))
                return low;
            return high;
        }

        Ticket ParseTicket(string line)
            => new Ticket(ParsePath(line.Substring(0, 7), 128), ParsePath(line.Substring(7, 3), 8));

        void Parse(List<string> data)
            => Tickets = data.Select(ParseTicket).ToList();

        protected override string Solve1(List<string> data)
        {
            Parse(data);
            return Tickets.Max(q => q.SeatID).ToString();
        }

        protected override string Solve2(List<string> data)
        {
            Parse(data);
            var seatIDs = Tickets.Select(q => q.SeatID).OrderBy(q => q).ToList();
            var last = seatIDs.First() + 1;
            foreach (var item in seatIDs.Skip(1))
            {
                if (last != item)
                    return last.ToString();

                last++;
            }
            return "??";
        }

    }
}

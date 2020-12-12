using AdventOfCode.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Day12
{
    public class Day12Tests
    {
        private readonly ITestOutputHelper Output;
        public Day12Tests(ITestOutputHelper output) => Output = output;

        [Fact]
        public void RunStep1() => Output.WriteLine(new Day12Solver().ExecutePuzzle1());
        
        [Fact]
        public void RunStep2() => Output.WriteLine(new Day12Solver().ExecutePuzzle2());
    }

    public record Action (string Direction, int Units);

    public class Day12Solver : SolverBase
    {
        List<Action> Data;

        List<string> DirectionsRotateLeft = new List<string> { "N", "W", "S", "E" };

        string ShipFacing = "E";
        int PositionNorthSouth = 0;
        int PositionEastWest = 0;

        string Rotate(string direction, int degree)
        {
            var offset = (degree / 90 % 4);
            var index = DirectionsRotateLeft.IndexOf(direction) + offset;
            if (index < 0)
                index += DirectionsRotateLeft.Count;
            else if (index >= DirectionsRotateLeft.Count)
                index -= DirectionsRotateLeft.Count;
            return DirectionsRotateLeft[index];
        }

        void Move(string direction, int value)
        {
            if (direction == "N")
                PositionNorthSouth += value;
            else if (direction == "S")
                PositionNorthSouth -= value;
            else if (direction == "E")
                PositionEastWest += value;
            else if (direction == "W")
                PositionEastWest -= value;
            else if (direction == "L")
                ShipFacing = Rotate(ShipFacing, value);
            else if (direction == "R")
                ShipFacing = Rotate(ShipFacing, -value);
            else if (direction == "F")
                Move(ShipFacing, value);
        }

        Action ParseAction(string item)
            => new Action(item.Substring(0, 1), int.Parse(item.Substring(1)));

        protected override void Parse(List<string> data)
        {
            Data = data.Select(ParseAction).ToList();
        }

        protected override object Solve1()
        {
            foreach (var item in Data)
                Move(item.Direction, item.Units);

            return Math.Abs(PositionNorthSouth) + Math.Abs(PositionEastWest);
        }

        protected override object Solve2()
        {
            throw new Exception("Solver error");
        }
    }
}

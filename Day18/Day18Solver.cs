using AdventOfCode.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Day18
{
    public class Day18Tests
    {
        private readonly ITestOutputHelper Output;
        public Day18Tests(ITestOutputHelper output) => Output = output;

        [Fact]
        public void RunStep1() => Output.WriteLine(new Day18Solver().ExecutePuzzle1());

        [Fact]
        public void RunStep2() => Output.WriteLine(new Day18Solver().ExecutePuzzle2());
    }

    public class Day18Solver : SolverBase
    {
        List<string> Data;

        protected override void Parse(List<string> data)
        {
            Data = data;
        }


        long CalcSimpleExpression(string expression)
        {
            var parts = expression.Split(' ');
            var result = long.Parse(parts[0]);
            for (var i = 1; i < parts.Length; i += 2)
            {
                var value2 = long.Parse(parts[i + 1]);
                if (parts[i] == "+")
                    result += value2;
                if (parts[i] == "*")
                    result *= value2;
            }

            return result;
        }

        long CalcAdvancedExpression(string expression)
        {
            var parts = expression.Split(' ');

            for (var i = 1; i < parts.Length; i += 2)
                if (parts[i] == "+")
                {
                    parts[i + 1] = (long.Parse(parts[i - 1]) + long.Parse(parts[i + 1])).ToString();
                    parts[i - 1] = "";
                    parts[i] = "";
                }

            parts = parts.Where(q => q != "").ToArray();
            var result = long.Parse(parts[0]);
            for (var i = 1; i < parts.Length; i += 2)
                result *= long.Parse(parts[i + 1]);

            return result;
        }


        long CalcExpression(string expression, Func<string, long> simpleExpr)
        {
            while (expression.Contains('('))
            {
                var start = expression.IndexOf('(');
                int end;
                var count = 1;
                for (end = start + 1; end < expression.Length && count > 0; end++)
                {
                    if (expression[end] == '(')
                        count++;
                    else if (expression[end] == ')')
                        count--;
                }

                var subResult = CalcExpression(expression[(start + 1)..(end - 1)], simpleExpr);
                expression = expression[..start] + subResult.ToString() + expression[(end)..];
            }

            // Calc values
            return simpleExpr(expression);
        }


        protected override object Solve1()
            => Data.Select(q => CalcExpression(q, CalcSimpleExpression)).Sum();

        protected override object Solve2()
            => Data.Select(q => CalcExpression(q, CalcAdvancedExpression)).Sum();
    }
}

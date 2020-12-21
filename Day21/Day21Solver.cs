using AdventOfCode.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Day21
{
    public class Day21Tests
    {
        private readonly ITestOutputHelper Output;
        public Day21Tests(ITestOutputHelper output) => Output = output;

        [Fact]
        public void RunStep1() => Output.WriteLine(new Day21Solver().ExecutePuzzle1());

        [Fact]
        public void RunStep2() => Output.WriteLine(new Day21Solver().ExecutePuzzle2());
    }

    public record Food(List<string> ingredients, List<string> allergens);

    public class Day21Solver : SolverBase
    {
        List<Food> Data;
        HashSet<string> Allergens;
        Dictionary<string, string> IngredientAllergenMap;

        Food ParseLine(string line)
        {
            var parts = line.Split(StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries, "(contains ", ")");
            var ingredients = parts[0].Split(' ').ToList();
            var allergens = new List<string>();
            if (parts.Length > 1)
                allergens = parts[1].Split(StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries, ",").ToList();

            return new Food(ingredients, allergens);
        }

        protected override void Parse(List<string> data)
        {
            Data = data.Select(ParseLine).ToList();
            Allergens = Data.SelectMany(q => q.allergens, (a, b) => b).ToHashSet();
        }

        void BuildMap()
        {
            IngredientAllergenMap = new Dictionary<string, string>();
            var used = new HashSet<string>();
            var allergens = Allergens.ToList();

            while (allergens.Count > 0)
            {
                var i = 0;
                while (i < allergens.Count)
                {
                    var allergen = allergens[i];
                    List<string> ingredients = null;
                    foreach (var data in Data)
                    {
                        if (data.allergens.Contains(allergen))
                        {
                            if (ingredients == null)
                                ingredients = data.ingredients.Where(q => !used.Contains(q)).ToList();
                            else
                                ingredients = ingredients.Intersect(data.ingredients.Where(q => !used.Contains(q))).ToList();
                            if (ingredients.Count == 1)
                            {
                                IngredientAllergenMap[ingredients[0]] = allergen;
                                allergens.RemoveAt(i);
                                used.Add(ingredients[0]);
                                break;
                            }
                        }
                    }

                    if (ingredients.Count != 1)
                        i++;
                }
            }
        }

        protected override object Solve1()
        {
            BuildMap();

            var count = 0;
            foreach (var item in Data)
                count += item.ingredients.Count(q => !IngredientAllergenMap.ContainsKey(q));

            return count;
        }

        protected override object Solve2()
        {
            BuildMap();

            return IngredientAllergenMap.OrderBy(q => q.Value).Select(q => q.Key).ToList().Join(",");
        }
    }
}

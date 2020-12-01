using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode.Base
{
    public abstract class SolverBase
    {
        string DayDirectory => $"{AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("bin"))}\\{GetType().Name.Substring(0, 5)}\\";

        List<string> Load(string inputFileName)
            => File.ReadAllLines(DayDirectory + inputFileName).ToList();

        string Save(string outputFileName, string data)
        {
            File.WriteAllText(DayDirectory + outputFileName, data);
            return data;
        }

        public string ExecutePuzzle1(string inputFileName = "input.txt", string outputFileName = "output1.txt")
            => Save(outputFileName, Solve1(Load(inputFileName)));

        public string ExecutePuzzle2(string inputFileName = "input.txt", string outputFileName = "output2.txt")
            => Save(outputFileName, Solve2(Load(inputFileName)));

        protected abstract string Solve1(List<string> data);

        protected abstract string Solve2(List<string> data);
    }
}

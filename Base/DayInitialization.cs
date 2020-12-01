using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Base
{
    class DayInitialization
    {
        public static void Execute(int day)
        {
            var projectDir = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("bin"));

            var dayStr = day.ToString("00");
            var dayDir = projectDir + @"\Day" + dayStr + "\\";

            if (Directory.Exists(dayDir))
                Console.WriteLine($"Directory {dayDir} was allready initialized");

            Directory.CreateDirectory(dayDir);

            var template = File.ReadAllText(projectDir + @"Base\solver.txt");
            template = template.Replace("[Day]", dayStr);
            File.WriteAllText(dayDir + $"Day{ dayStr}Solver.cs", template);

            var inputFileName = dayDir + "input.txt";
            File.WriteAllText(inputFileName, "");

            Process.Start(new ProcessStartInfo(inputFileName) { UseShellExecute = true, });

        }
    }
}

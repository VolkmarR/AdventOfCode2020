# AdventOfCode CSharp Starter Kit

This template can be used as starter kit for the [AdventOfCode](https://www.AdventOfCode.com) advent calendar. Just click the "Use Template" button, enter a name for your repository and you are ready to code. 

To get your started quickly, the starter kit assists you with a generator for initializing the 
source code to solve the puzzles.

## Initialize a new day

Just run the application and enter the number of the day, you want to initialize.

The generator creates the following:
- A new directory for the requested day
- The boilerplate code for the solver
- The Unit-Tests to execute the solver
- An empty text file called input.txt (which will be opened in the standard text editor)

## Implement the solver

After initializing a day, you have to paste the input data from the website to the input.txt file 
and implement the Solve1 and Solve2 methods. 

```csharp

// Solver for AoC2018, day 1
protected override string Solve1(List<string> data)
{
    return data.Select(q => Int.Parse(q)).Sum().ToString();
}

```

## Executing the solvers

To Execute the solvers, you have to use the generated unit tests. You can run them using the Test Explorer 
in Visual Studio. The unit tests load the input.txt, call the Solve method and save the output to a text 
file called output1.txt or output2.txt.

You can change the name of the input and output filename by passing alternative filename as parameter.

```csharp

[Fact]
public void RunStep1() => Output.WriteLine(new Day[Day]Solver().ExecutePuzzle1("testinput.txt"));

```

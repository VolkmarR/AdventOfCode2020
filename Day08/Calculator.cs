using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day08
{

    class Calculator
    {
        public record Instruction(string Operation, int Argument);

        public readonly List<Instruction> Program;
        public int ExecutionPointer { get; private set; } = 0;
        public int GobalValue { get; private set; } = 0;
        public bool Running => ExecutionPointer < Program.Count;

        void Jump(int argument) => ExecutionPointer += argument;

        void Accumulator(int argument)
        {
            GobalValue += argument;
            Jump(1);
        }

        void Nop() => Jump(1);

        public void Reset()
        {
            ExecutionPointer = 0;
            GobalValue = 0;
        }

        public void ExecuteNextInstruction()
        {
            var instruction = Program[ExecutionPointer];
            if (instruction.Operation == "acc")
                Accumulator(instruction.Argument);
            else if (instruction.Operation == "jmp")
                Jump(instruction.Argument);
            else if (instruction.Operation == "nop")
                Nop();
        }

        public Calculator(List<string> data)
        {
            Program = data.Select(ParseInstruction).ToList();
        }

        private Instruction ParseInstruction(string line)
        {
            var parts = line.Split(' ');
            return new Instruction (parts[0], int.Parse(parts[1]));
        }
    }
}

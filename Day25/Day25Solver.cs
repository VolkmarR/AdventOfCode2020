using AdventOfCode.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Day25
{
    public class Day25Tests
    {
        private readonly ITestOutputHelper Output;
        public Day25Tests(ITestOutputHelper output) => Output = output;

        [Fact]
        public void RunStep1() => Output.WriteLine(new Day25Solver().ExecutePuzzle1());
        
        [Fact]
        public void RunStep2() => Output.WriteLine(new Day25Solver().ExecutePuzzle2());
    }

    public class Day25Solver : SolverBase
    {
        long CardPublicKey;
        long DoorPublicKey;
        long SubjectNumber = 7;
        long Divider = 20201227;
        long CardSecretLoop;
        long DoorSecretLoop;

        protected override void Parse(List<string> data)
        {
            CardPublicKey = data[0].ToLong();
            DoorPublicKey = data[1].ToLong();
        }

        long CalcStep(long startValue, long subjectNumber)
            => (startValue * subjectNumber) % Divider;

        long FindSecretLoopNumber(long publicKey)
        {
            long loopNumber = 0;
            long value = 1;
            for (; value != publicKey; loopNumber++)
                value = CalcStep(value, SubjectNumber);

            return loopNumber;
        }

        long GetEncryptionKey(long publicKey, long loopSize)
        {
            var result = 1L;
            for (var i = 0L; i < loopSize; i++)
                result = CalcStep(result, publicKey);
            return result;
        }

        protected override object Solve1()
        {
            CardSecretLoop = FindSecretLoopNumber(CardPublicKey);
            DoorSecretLoop = FindSecretLoopNumber(DoorPublicKey);

            return GetEncryptionKey(CardPublicKey, DoorSecretLoop);
        }

        protected override object Solve2()
        {
            throw new Exception("Solver error");
        }
    }
}

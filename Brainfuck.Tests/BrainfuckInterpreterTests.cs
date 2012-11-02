using FakeItEasy;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brainfuck.Tests
{
    [TestFixture]
    public class BrainfuckInterpreterTests
    {
        private Interpreter interpreter;

        [SetUp]
        public void SetUp()
        {
            var inputOutputProvider = A.Fake<IInputOutputProvider>();
            interpreter = new Interpreter(inputOutputProvider);
        }

        [Test]
        public void TestInterpreterInitialState()
        {
            Assert.AreEqual(1, interpreter.MemoryCells.Count, "Memory cells count should be equal to 1 at start of program");
            Assert.AreEqual(0, interpreter.MemoryCells[0], "First memory cell should equal 0 at start of program");
            Assert.AreEqual(0, interpreter.PointerPosition, "Pointer position should be 0 at start of program");
        }

        [Test]
        public void TestAddMemoryCells()
        {
            String program = ">>>";
            interpreter.Execute(program);

            Assert.AreEqual(4, interpreter.MemoryCells.Count, "Should have 4 cells after moving 3 times to the right");
            Assert.AreEqual(3, interpreter.PointerPosition, "Pointer position should be 3 when moving 3 times to the right");
        }

        [Test]
        public void TestDecrementPointerPosition()
        {
            String program = ">><<";
            interpreter.Execute(program);

            Assert.AreEqual(0, interpreter.PointerPosition, "Should be at position 0 after moving two to the right and two to the left");
            Assert.AreEqual(3, interpreter.MemoryCells.Count, "Should have 3 memory cells after moving twice to the right");
        }

        [Test]
        public void TestIncrementMemoryCell()
        {
            String program = "+++++";
            interpreter.Execute(program);

            Assert.AreEqual(5, interpreter.CurrentCellValue, "Cell #0 should be 5 when incrementing 5 times");
        }

        [Test]
        public void TestDecrementMemoryCell()
        {
            String program = "++-";
            interpreter.Execute(program);

            Assert.AreEqual(1, interpreter.CurrentCellValue, "Cell #0 should be 1 when incrementing twice and decrementing once");
        }

        [Test]
        public void TestEnterLoopWhenNot0()
        {
            String program = "+[->+++<]>-";
            interpreter.Execute(program);

            Assert.AreEqual(2, interpreter.CurrentCellValue);
        }

        [Test]
        public void TestNotEnterLoopWhen0()
        {
            String program = "[->+++<]>-";
            interpreter.Execute(program);

            Assert.AreEqual(-1, interpreter.CurrentCellValue);
        }

        [Test]
        public void TestComplexProgram()
        {
            //This program multiplies both numbers in cells #0 and #1
            String program = "+++++>++++++<[>[->+>+<<]>>[-<<+>>]<<<-]>>";
            interpreter.Execute(program);
            
            Assert.AreEqual(30, interpreter.CurrentCellValue);
        }

        [Test]
        public void TestInput()
        {
            var inputOutput = A.Fake<IInputOutputProvider>();
            A.CallTo(() => inputOutput.Get()).ReturnsNextFromSequence(new string[] {"4", "3", "-1"});
            interpreter = new Interpreter(inputOutput);

            string program = ",>,>,<<";
            interpreter.Execute(program);

            Assert.AreEqual(4, interpreter.MemoryCells[0]);
            Assert.AreEqual(3, interpreter.MemoryCells[1]);
            Assert.AreEqual(-1, interpreter.MemoryCells[2]);
        }

        [Test]
        public void TestOutput()
        {
            string output = "";
            var inputOutput = A.Fake<IInputOutputProvider>();
            A.CallTo(() => inputOutput.Write(A<String>.Ignored))
                .Invokes(call => output += call.Arguments[0]);

            interpreter = new Interpreter(inputOutput);

            string program = "++++.--.>+++++.";
            interpreter.Execute(program);

            Assert.AreEqual("425", output);
        }
    }
}

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
        }

        [Test]
        public void TestAddMemoryCells()
        {
            String program = ">>>";
            interpreter.Execute(program);

            Assert.AreEqual(4, interpreter.MemoryCells.Count);
        }

        [Test]
        public void TestIncrementMemoryCell()
        {
            String program = "+++++";
            interpreter.Execute(program);

            Assert.AreEqual(5, interpreter.MemoryCells[0]);
        }
    }
}

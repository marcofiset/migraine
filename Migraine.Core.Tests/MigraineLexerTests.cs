using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migraine.Core.Tests
{
    [TestFixture]
    public class MigraineLexerTests
    {
        private MigraineLexer _lexer;

        private void Verify(String program, TokenStream tokens)
        {
            program.Split(' ').ToList().ForEach(s => Assert.IsTrue(tokens.Consume(s)));
        }

        [SetUp]
        public void SetUp()
        {
            _lexer = new MigraineLexer();
        }

        [Test]
        public void CanProcudeNumberTokens()
        {
            var program = "5 6.5 0.523";
            var tokens = _lexer.Tokenize(program);

            Verify(program, tokens);
        }

        [Test]
        public void CanProduceIdentifierTokens()
        {
            var program = "var1 var_2 VAR3 Var_4 _898";
            var tokens = _lexer.Tokenize(program);

            Verify(program, tokens);
        }

        [Test]
        public void CanProduceOperatorTokens()
        {
            var program = "+ - * / ( )";
            var tokens = _lexer.Tokenize(program);

            Verify(program, tokens);
        }

        [Test]
        public void CanProduceTerminatorToken()
        {
            var program = ";";

            var tokens = _lexer.Tokenize(program);
            Assert.IsTrue(tokens.Consume(TokenType.Terminator));
        }

        [Test]
        public void CanProduceAssignmentToken()
        {
            var program = "=";
            var tokens = _lexer.Tokenize(program);

            Assert.IsTrue(tokens.Consume("="));
        }
    }
}

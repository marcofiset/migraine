using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Migraine.Core.Tests
{
    public class ScopeTests
    {
        private Scope _parentScope;
        private Scope _innerScope;

        [SetUp]
        public void SetUp()
        {
            _parentScope = new Scope();
            _parentScope.AssignVariable("var1", 5);
            _parentScope.AssignVariable("var2", 6);

            _innerScope = new Scope(_parentScope);
            _innerScope.AssignVariable("innerVar", 12);
        }

        [Test]
        public void ScopeCanResolveVariableName()
        {
            Assert.AreEqual(5, _parentScope.ResolveVariable("var1"));
        }

        [Test]
        public void ResolveThrowsOnUnknownVariable()
        {
            Assert.Throws<UndefinedIdentifierException>(() => _parentScope.ResolveVariable("unknown"));
        }

        [Test]
        public void CanResolveVariablesInParentScopes()
        {
            Assert.AreEqual(5, _innerScope.ResolveVariable("var1"));
        }

        [Test]
        public void CanAskIfScopeDefinesVariable()
        {
            Assert.True(_innerScope.DefinesVariable("innerVar"));
            Assert.False(_innerScope.DefinesVariable("var1"));
        }

        [Test]
        public void CanAskIfScopeResolvesVariable()
        {
            Assert.True(_innerScope.ResolvesVariable("innerVar"));
            Assert.True(_innerScope.ResolvesVariable("var1"));
        }

        [Test]
        public void AssignExistingVariableOverwritesExistingOne()
        {
            Assert.Fail();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Migraine.Core.Exceptions;
using NUnit.Framework;

namespace Migraine.Core.Tests
{
    public class ScopeTests
    {
        private Scope parentScope;
        private Scope innerScope;

        [SetUp]
        public void SetUp()
        {
            parentScope = new Scope();
            parentScope.AssignVariable("var1", 5);
            parentScope.AssignVariable("var2", 6);

            innerScope = new Scope(parentScope);
            innerScope.AssignVariable("innerVar", 12);
        }

        [Test]
        public void ScopeCanResolveVariableName()
        {
            Assert.AreEqual(5, parentScope.ResolveVariable("var1"));
        }

        [Test]
        public void ResolveThrowsOnUnknownVariable()
        {
            Assert.Throws<UndefinedIdentifier>(() => parentScope.ResolveVariable("unknown"));
        }

        [Test]
        public void CanResolveVariablesInParentScopes()
        {
            Assert.AreEqual(5, innerScope.ResolveVariable("var1"));
        }

        [Test]
        public void CanAskIfScopeDefinesVariable()
        {
            Assert.True(innerScope.DefinesVariable("innerVar"));
            Assert.False(innerScope.DefinesVariable("var1"));
        }

        [Test]
        public void CanAskIfScopeResolvesVariable()
        {
            Assert.True(innerScope.ResolvesVariable("innerVar"));
            Assert.True(innerScope.ResolvesVariable("var1"));
        }

        [Test]
        public void AssignExistingVariableOverwritesExistingOneInParent()
        {
            innerScope.AssignVariable("var1", 10);

            Assert.AreEqual(10, parentScope.ResolveVariable("var1"));
        }

        [Test]
        public void DefineVariableCreatesNewVariableDirectlyInScope()
        {
            innerScope.DefineVariable("var1", 50);

            Assert.AreEqual(50, innerScope.ResolveVariable("var1"));
            Assert.AreEqual(5, parentScope.ResolveVariable("var1"));
        }
    }
}

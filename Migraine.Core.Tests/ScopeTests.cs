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
        private Scope<Double> parentScope;
        private Scope<Double> innerScope;

        [SetUp]
        public void SetUp()
        {
            parentScope = new Scope<Double>();
            parentScope.Assign("var1", 5);
            parentScope.Assign("var2", 6);

            innerScope = new Scope<Double>(parentScope);
            innerScope.Assign("innerVar", 12);
        }

        [Test]
        public void ScopeCanResolveVariableName()
        {
            Assert.AreEqual(5, parentScope.Resolve("var1"));
        }

        [Test]
        public void ResolveThrowsOnUnknownVariable()
        {
            Assert.Throws<UndefinedIdentifier>(() => parentScope.Resolve("unknown"));
        }

        [Test]
        public void CanResolveVariablesInParentScopes()
        {
            Assert.AreEqual(5, innerScope.Resolve("var1"));
        }

        [Test]
        public void CanAskIfScopeDefinesVariable()
        {
            Assert.True(innerScope.Defines("innerVar"));
            Assert.False(innerScope.Defines("var1"));
        }

        [Test]
        public void CanAskIfScopeResolvesVariable()
        {
            Assert.True(innerScope.Resolves("innerVar"));
            Assert.True(innerScope.Resolves("var1"));
        }

        [Test]
        public void AssignExistingVariableOverwritesExistingOneInParent()
        {
            innerScope.Assign("var1", 10);

            Assert.AreEqual(10, parentScope.Resolve("var1"));
        }

        [Test]
        public void DefineVariableCreatesNewVariableDirectlyInScope()
        {
            innerScope.Define("var1", 50);

            Assert.AreEqual(50, innerScope.Resolve("var1"));
            Assert.AreEqual(5, parentScope.Resolve("var1"));
        }
    }
}

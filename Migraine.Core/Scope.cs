using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migraine.Core
{
    public class UndefinedIdentifierException : Exception
    {
        public UndefinedIdentifierException(String identifier) : base("Identifier undefined : " + identifier) { }
    }

    public class Scope
    {
        private Scope _parent;
        private Dictionary<String, Double> _variables;

        /// <summary>
        /// Creates a new scope instance
        /// </summary>
        /// <param name="parent">The parent scope that the new one is tied to</param>
        public Scope(Scope parent = null)
        {
            _parent = parent;
            _variables = new Dictionary<String, Double>();
        }

        /// <summary>
        /// Defines a variable in the scope
        /// </summary>
        /// <param name="name">The name of the variable</param>
        /// <param name="value">The value of the variable</param>
        public void AssignVariable(String name, Double value)
        {
            _variables.Add(name, value);
        }

        /// <summary>
        /// Resolves a variable in the current scope.
        /// If the variable does not exist in the current scope, we walk up
        /// the parent scopes until we find it or utimately throw an exception
        /// if no variable exists with the name given
        /// </summary>
        /// <param name="name">The name of the variable</param>
        /// <returns>The value of the variable</returns>
        public Double ResolveVariable(String name)
        {
            if (_variables.ContainsKey(name))
                return _variables[name];

            if (_parent == null)
                throw new UndefinedIdentifierException(name);

            return _parent.ResolveVariable(name);
        }
    }
}

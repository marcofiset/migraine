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
        private Scope parent;
        private Dictionary<String, Double> variables;

        /// <summary>
        /// Creates a new scope instance
        /// </summary>
        /// <param name="parent">The parent scope that the new one is tied to</param>
        public Scope(Scope parent = null)
        {
            this.parent = parent;
            variables = new Dictionary<String, Double>();
        }

        /// <summary>
        /// Assigns a variable
        /// </summary>
        /// <remarks>
        /// If the variable
        /// </remarks>
        /// <param name="name">The name of the variable</param>
        /// <param name="value">The value of the variable</param>
        public void AssignVariable(String name, Double value)
        {
            var parentScope = this;

            //Start from this scope, and work up the parents to find the one
            //that defines the variable.
            while (parentScope != null)
            {
                if (parentScope.DefinesVariable(name))
                {
                    parentScope.variables[name] = value;
                    return;
                }

                parentScope = parentScope.parent;
            }

            variables.Add(name, value);
        }

        /// <summary>
        /// Defines a variable in the scope, regardless of its parents' variables
        /// </summary>
        /// <param name="name">Name of the variable</param>
        /// <param name="value">Value of the variable</param>
        public void DefineVariable(String name, Double value)
        {
            variables.Add(name, value);
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
            if (variables.ContainsKey(name))
                return variables[name];

            if (parent == null)
                throw new UndefinedIdentifierException(name);

            return parent.ResolveVariable(name);
        }

        /// <summary>
        /// Indicates whether or not the scope directly defines the variable
        /// </summary>
        /// <param name="name">The name of the variable we want to check</param>
        /// <returns>True or false, if the variable exists or not in the current scope</returns>
        public Boolean DefinesVariable(String name)
        {
            return variables.ContainsKey(name);
        }

        /// <summary>
        /// Indicates whether or not the scope or any of its parent can resolve the variable
        /// </summary>
        /// <param name="name">The name of the variable we want to check</param>
        /// <returns>True or false, if the variable is resolvable by the current scope or its parents</returns>
        public Boolean ResolvesVariable(String name)
        {
            if (variables.ContainsKey(name))
                return true;

            if (parent == null)
                return false;

            return parent.ResolvesVariable(name);
        }
    }
}

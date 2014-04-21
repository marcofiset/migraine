using System;
using System.Collections.Generic;
using Migraine.Core.Exceptions;

namespace Migraine.Core
{
    public class Scope<T>
    {
        private Scope<T> parent;
        private Dictionary<String, T> variables;

        public Scope(Scope<T> parent = null)
        {
            this.parent = parent;
            variables = new Dictionary<String, T>();
        }

        public void Assign(String name, T value)
        {
            var parentScope = this;

            //Start from this scope, and work up the parents to find the one
            //that defines the variable.
            while (parentScope != null)
            {
                if (parentScope.Defines(name))
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
        public void Define(String name, T value)
        {
            variables.Add(name, value);
        }

        /// <summary>
        /// Resolves a variable in the current scope.
        /// If the variable does not exist in the current scope, we walk up
        /// the parent scopes until we find it or utimately throw an exception
        /// if no variable exists with the name given
        /// </summary>
        public T Resolve(String name)
        {
            if (variables.ContainsKey(name))
                return variables[name];

            if (parent == null)
                throw new UndefinedIdentifier(name);

            return parent.Resolve(name);
        }

        /// <summary>
        /// Indicates whether or not the scope directly defines the variable
        /// </summary>
        public Boolean Defines(String name)
        {
            return variables.ContainsKey(name);
        }

        /// <summary>
        /// Indicates whether or not the scope or any of its parent can resolve the variable
        /// </summary>
        public Boolean Resolves(String name)
        {
            if (variables.ContainsKey(name))
                return true;

            if (parent == null)
                return false;

            return parent.Resolves(name);
        }
    }
}

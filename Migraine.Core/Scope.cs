﻿using System;
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
            var assigned = false;

            //Start from this scope, and work up the parents to find the one
            //that defines the variable.
            while (parentScope != null)
            {
                if (parentScope.DefinesVariable(name))
                {
                    parentScope._variables[name] = value;
                    assigned = true;
                }

                parentScope = parentScope._parent;
            }

            //If we did not find the variable, we create it in the current scope
            if (!assigned)
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

        /// <summary>
        /// Indicates whether or not the scope directly defines the variable
        /// </summary>
        /// <param name="name">The name of the variable we want to check</param>
        /// <returns>True or false, if the variable exists or not in the current scope</returns>
        public Boolean DefinesVariable(String name)
        {
            return _variables.ContainsKey(name);
        }

        /// <summary>
        /// Indicates whether or not the scope or any of its parent can resolve the variable
        /// </summary>
        /// <param name="name">The name of the variable we want to check</param>
        /// <returns>True or false, if the variable is resolvable by the current scope or its parents</returns>
        public Boolean ResolvesVariable(String name)
        {
            if (_variables.ContainsKey(name))
                return true;

            if (_parent == null)
                return false;

            return _parent.ResolvesVariable(name);
        }
    }
}

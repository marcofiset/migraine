using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migraine.Core.Exceptions
{
    public class UndefinedIdentifier : Exception
    {
        public UndefinedIdentifier(String name) : base("Undefined identifier: " + name) { }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Migraine.Core.Exceptions
{
    public class UndefinedFunction : Exception
    {
        public UndefinedFunction(String name) : base(String.Format("Function {0} is undefined.", name)) {}
    }
}

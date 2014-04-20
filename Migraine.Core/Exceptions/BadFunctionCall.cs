using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migraine.Core.Exceptions
{
    public class BadFunctionCall : Exception
    {
        public BadFunctionCall(string name, int expected, int received) : 
            base(String.Format("{0} expected {1} arguments, but received {2}.", name, expected, received)) { }
    }
}

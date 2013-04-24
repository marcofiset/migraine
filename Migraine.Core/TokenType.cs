using System;
using System.Collections.Generic;
using System.Text;

namespace Migraine.Core
{
    public enum TokenType
    {
        Number,
        Operator,
        Symbol,
        Identifier,
        Whitespace,
        Terminator
    }
}
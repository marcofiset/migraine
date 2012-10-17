using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brainfuck
{
    public interface IOutputProvider
    {
        void Write(string value);
    }
}

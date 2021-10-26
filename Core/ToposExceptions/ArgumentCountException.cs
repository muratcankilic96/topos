using System;
using System.Collections.Generic;
using System.Text;

namespace Topos.Core.ToposExceptions
{
    class ArgumentCountException : ToposException
    {
        public ArgumentCountException() : base("Invalid number of operands.")
        {

        }

        public ArgumentCountException(uint a) : base($"Unexpected {a} argument(s).")
        {

        }

        public ArgumentCountException(string message) : base(message)
        {

        }
    }
}

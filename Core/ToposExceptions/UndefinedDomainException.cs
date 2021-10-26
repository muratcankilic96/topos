using System;
using System.Collections.Generic;
using System.Text;

namespace Topos.Core.ToposExceptions
{
    class UndefinedDomainException : ToposException
    {
        public UndefinedDomainException()
        {

        }

        public UndefinedDomainException(Number a) : base($"Input {a} is not in the domain of the function.")
        {

        }

        public UndefinedDomainException(string message) : base(message)
        {

        }
    }
}

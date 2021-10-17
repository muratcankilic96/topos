using System;
using System.Collections.Generic;
using System.Text;

namespace Topos.Core.ToposExceptions
{
    class ComplexDomainException : ToposException
    {
        public ComplexDomainException() : base("Cannot operate on the field of complex numbers.")
        {

        }

    }
}

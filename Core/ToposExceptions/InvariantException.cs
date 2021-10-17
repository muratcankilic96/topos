using System;
using System.Collections.Generic;
using System.Text;

namespace Topos.Core.ToposExceptions
{
    class InvariantException : ToposException
    {
        public InvariantException() : base("Invariant member is not a number.")
        {

        }

    }
}

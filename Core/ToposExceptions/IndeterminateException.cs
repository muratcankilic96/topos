using System;
using System.Collections.Generic;
using System.Text;

namespace Topos.Core.ToposExceptions
{
    class IndeterminateException : ToposException
    {
        public IndeterminateException() : base("Indeterminate member is not a number.")
        {

        }

    }
}

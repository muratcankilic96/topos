using System;
using System.Collections.Generic;
using System.Text;

namespace Topos.Core.ToposExceptions
{
    class ToposException : Exception
    {
        public ToposException() 
        {

        }
        public ToposException(string message) : base(message)
        {

        }

    }
}

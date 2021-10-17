using System;
using System.Collections.Generic;
using System.Text;

namespace Topos.Core.ToposExceptions
{
    class DimensionMismatchException : ToposException
    {
        public DimensionMismatchException()
        {

        }

        public DimensionMismatchException(uint a, uint b) : base($"Dimensions {a} and {b} mismatch.")
        {

        }

        public DimensionMismatchException((uint, uint) a, (uint, uint) b): base($"Dimensions ({a.Item1}, {b.Item2}) and ({b.Item1}, {b.Item2}) mismatch.")
        {

        }
    }
}

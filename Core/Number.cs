using System;
using System.Collections.Generic;
using System.Text;

namespace Topos.Core
{
    /// <summary>
    /// Number is a type of measure, and the basis of many mathematical fields.
    /// </summary>
    public abstract class Number : Element
    {
        public virtual double Value { get; set; }
    }
}

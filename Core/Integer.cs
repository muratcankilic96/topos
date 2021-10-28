using System;
using System.Collections.Generic;
using System.Text;

namespace Topos.Core
{
    /// <summary>
    /// Integers are whole numbers.
    /// </summary>
    public class Integer : Real
    {
        private long m_value;
        public override double Value {
            get
            {
                return m_value;
            }
            set
            {
                m_value = Convert.ToInt64(value);
            }
        }

        /// <summary>
        /// Creates an integer that equals to 0
        /// </summary>
        public Integer()
        {
            Value = 0;
        }

        /// <summary>
        /// Creates an integer
        /// </summary>
        /// <param name="value">Value of the integer</param>
        public Integer(long value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static implicit operator Integer(long i) => new Integer(i);
        public static implicit operator Integer(Complex c) => new Integer((long) c.Real.Value);
        public static implicit operator long(Integer i) => (long) i.Value;
        public static implicit operator double(Integer i) => (double) i.Value;
    }
}

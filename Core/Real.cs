using System;
using System.Collections.Generic;
using System.Text;

namespace Topos.Core
{
    /// <summary>
    /// A real number is a number that can be irrational or rational.
    /// In computer implementation, it is impossible to represent an irrational number.
    /// </summary>
    public class Real : Number
    {
        public override double Value { get; set; }

        /// <summary>
        /// Creates a real number that equals to 0
        /// </summary>
        public Real()
        {
            Value = 0;
        }

        /// <summary>
        /// Creates a real number
        /// </summary>
        /// <param name="value">Value of the real number</param>
        public Real(double value)
        {
            Value = value;
        }

        public override string ToString() => Value.ToString();

        public static implicit operator Real(double d) => new Real(d);
        public static implicit operator double(Real r) => r.Value;

        // All real numbers are comparable. Hence every real number also have their comparison operators.
        public static bool operator ==(Real a, Real b)
        {
            return a.Value == b.Value;
        }

        public static bool operator !=(Real a, Real b)
        {
            return a.Value != b.Value;
        }

        public override bool Equals(object obj) 
        {
            return this == (Real) obj;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }


        // Ordered operators.

        public static bool operator <(Real a, Real b)
        {
            return a.Value < b.Value;
        }

        public static bool operator >(Real a, Real b)
        {
            return a.Value > b.Value;
        }

        public static bool operator <=(Real a, Real b)
        {
            return a.Value <= b.Value;
        }

        public static bool operator >=(Real a, Real b)
        {
            return a.Value >= b.Value;
        }
    }
}

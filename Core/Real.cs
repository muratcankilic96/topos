using System;
using System.Collections.Generic;
using System.Text;

namespace Topos.Core
{
    /// <summary>
    /// A RealConstant is a constant that is an element of the set of real numbers.
    /// </summary>
    public enum RealConstant {
        /// <summary>
        /// π = 3.1415926535897932...
        /// </summary>
        pi,
        /// <summary>
        /// e = 2.7182818284590451...
        /// </summary>
        e,
        /// <summary>
        /// Φ = 1.6180339887498948...
        /// </summary>
        phi,
        /// <summary>
        /// √2 = 1.4142135623730950...
        /// </summary>
        sqrt2,
        /// <summary>
        /// √3 = 1.7320508075688772...
        /// </summary>
        sqrt3,
        /// <summary>
        /// ln2 = 0.6931471805599453...
        /// </summary>
        ln2
    }

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

        /// <summary>
        /// Creates a constant real number
        /// </summary>
        /// <param name="constant">Identifier of the constant</param>
        public static Real Constant(RealConstant constant)
        {
            switch(constant)
            {
                case RealConstant.pi:
                    return 3.1415926535897932;
                case RealConstant.e:
                    return 2.7182818284590451;
                case RealConstant.phi:
                    return 1.6180339887498948;
                case RealConstant.sqrt2:
                    return 1.4142135623730950;
                case RealConstant.sqrt3:
                    return 1.7320508075688772;
                case RealConstant.ln2:
                    return 0.6931471805599453;
                default:
                    return 0;
            }
        }

        public override string ToString() => Value.ToString();

        public static implicit operator Real(double d) => new Real(d);
        public static implicit operator double(Real r) => (double) r.Value;
        public static implicit operator Real(Complex c) => new Real(c.Real);

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
            if (obj is Real)
                return this == (Real)obj;
            else return false;

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

using System;
using System.Collections.Generic;
using System.Text;
using Topos.NumberTheory;

namespace Topos.Core
{
    /// <summary>
    /// A rational number is a number that can be written of the form a/b where a and b are integers.
    /// </summary>
    public class Rational : Real
    {
        public Integer Numerator { get; set; }

        private Integer m_denominator;
        public Integer Denominator
        {
            get => m_denominator; 
            set
            {
                if (value.Value != 0)
                    m_denominator = value;
                else
                    throw new DivideByZeroException();
            }
        }

        public override double Value
        {
            get
            {
                return Numerator.Value / Denominator.Value;
            }
        }

        /// <summary>
        /// Creates a rational number that equals to 0
        /// </summary>
        public Rational()
        {
            Numerator = 0;
            Denominator = 1;
        }

        /// <summary>
        /// Creates a rational number from numerator and denominator
        /// </summary>
        /// <param name="numerator">Numerator (upper part) of the rational number</param>
        /// <param name="denominator">Denominator (lower part) of the rational number</param>
        public Rational(int numerator, int denominator)
        {
            Numerator = numerator;
            if (denominator != 0)
                Denominator = denominator;
            else
                throw new DivideByZeroException();
        }

        /// <summary>
        /// Creates a rational number from the decimal presentation.
        /// Due to floating-point limitations, rational numbers use 6-digit precision.
        /// </summary>
        /// <param name="decim">Decimal representation of a rational number</param>
        public Rational(double decim) : this()
        {
            // Uses the default constructor if 0 given as input
            if (decim != 0)
            {
                // Assume the number is the form decim/1, where numerator is not necessarily an integer
                double numerator = Math.Round(decim, 6);
                int denominator = 1;

                // Repeat until numerator is an integer
                while (Math.Abs(numerator - (int)numerator) > 1e-6)
                {
                    numerator *= 10;
                    denominator *= 10;
                }

                // Compute Gcd to simplify the representation of rational number
                int divisor = Gcd.ComputeGcd((int)numerator, denominator);

                // Assign values
                Numerator = (int)(numerator / divisor);
                Denominator = denominator / divisor;
            }
        }

        public static implicit operator Rational((int, int) i) => new Rational(i.Item1, i.Item2);
        public static implicit operator Rational(double d) => new Rational(d);
        public static implicit operator double(Rational q) => q.Value;

        public override string ToString()
        {
            if (Denominator == 1 || Denominator == -1)
                return (Denominator.Value * Numerator.Value).ToString();
            else
                return Numerator.Value + "/" + Denominator.Value;
        }
    }
}

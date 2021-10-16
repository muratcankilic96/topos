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
        // Precision a read-only property of rationals that provided to prevent floating-point precision problems.
        private readonly int PRECISION = 6;
        /// <summary>
        /// Upper part of the fraction.
        /// </summary>
        public Integer Numerator { get; set; }

        private Integer m_denominator;
        /// <summary>
        /// Lower part of the fraction.
        /// </summary>
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
                double numerator = Math.Round(decim, PRECISION);
                int denominator = 1;

                // Repeat until numerator is an integer
                while (Math.Abs(numerator - (int)numerator) > 1e-6)
                {
                    numerator *= 10;
                    denominator *= 10;
                }

                // Compute Gcd to simplify the representation of rational number
                int divisor = Division.Gcd((int)numerator, denominator);

                // Assign values
                Numerator = (int)(numerator / divisor);
                Denominator = denominator / divisor;
            }
        }

        public override string ToString()
        {
            if (Denominator == 1 || Denominator == -1)
                return (Denominator.Value * Numerator.Value).ToString();
            else
                return Numerator.Value + "/" + Denominator.Value;
        }

        public static implicit operator Rational((int, int) i) => new Rational(i.Item1, i.Item2);
        public static implicit operator Rational(double d) => new Rational(d);
        public static implicit operator Rational(Complex c) => new Rational(c.Real.Value);

        // Operations between rational numbers are overridden.
        
        //// Additive operations use the similar pattern, so they are combined in single function.
        private static Rational Additive(Rational a, Rational b)
        {
            int denominator = Division.Lcm(a.Denominator, b.Denominator);
            int leftMultiplier = denominator / a.Denominator;
            int rightMultiplier = denominator / b.Denominator;

            return new Rational(a.Numerator * leftMultiplier + b.Numerator * rightMultiplier, denominator);
        }

        public static Rational operator +(Rational a, Rational b)
        {
            return Additive(a, b);
        }

        public static Rational operator -(Rational a, Rational b)
        {
            return Additive(a, -b);
        }

        public static Rational operator *(Rational a, Rational b)
        {
            return new Rational(a.Numerator * b.Numerator, a.Denominator * b.Denominator);
        }

        public static Rational operator /(Rational a, Rational b)
        {
            return new Rational(a.Numerator * b.Denominator, a.Denominator * b.Numerator);
        }

        /// <summary>
        /// Gets the additive inverse of a rational number. The negation is applied onto numerator part for simplicity purposes.
        /// </summary>
        /// <param name="q">The rational number</param>
        /// <returns>Additive inverse of the rational number</returns>
        public static Rational operator -(Rational q)
        {
            return new Rational(-q.Numerator, q.Denominator);
        }
    }
}

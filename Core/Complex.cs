using System;
using static System.Math;
using System.Collections.Generic;
using System.Text;

namespace Topos.Core
{
    /// <summary>
    /// A complex number is a number that represents two parts: a real part and an imaginary part.
    /// </summary>
    public class Complex : Number
    {
        public Real Imaginary { get; set; }
        public Real Real { get; set; }

        /// <summary>
        /// Synonymous with magnitude.
        /// </summary>
        public override double Value
        {
            get
            {
                return Sqrt(Pow(Imaginary.Value, 2) + Pow(Real.Value, 2));
            }
        }

        /// <summary>
        /// Magnitude represents the magnitude of a complex number.
        /// </summary>
        public double Magnitude
        {
            get => Value;
        }

        /// <summary>
        /// Creates a complex number that equals to 0
        /// </summary>
        public Complex()
        {
            Real = 0;
            Imaginary = 0;
        }

        /// <summary>
        /// Creates a complex number
        /// </summary>
        /// <param name="real">Value of the real part</param>
        /// /// <param name="imaginary">Value of the imaginary part</param>
        public Complex(double real, double imaginary)
        {
            Real = real;
            Imaginary = imaginary;
        }

        public override string ToString()
        {
            string sign = "";

            // Format sign of the imaginary part.
            if (Imaginary > 0)
                sign = "+";

            // Print number for each case.
            if (Real == 0 && Imaginary == 0)
                return $"{0}";
            else if (Real == 0 && Imaginary != 0)
                return $"{Imaginary}i";
            else if (Real != 0 && Imaginary == 0)
                return $"{Real}";
            else
                return $"{Real}{sign}{Imaginary}i";
        }


        // Complex numbers are not orderable. Hence, their only comparison operators are in terms of equality.
        public static bool operator ==(Complex a, Complex b)
        {
            return (a.Real == b.Real) && (a.Imaginary == b.Imaginary);
        }

        public static bool operator !=(Complex a, Complex b)
        {
            return !((a.Real == b.Real) && (a.Imaginary == b.Imaginary));
        }

        public override bool Equals(object obj)
        {
            return this == (Complex)obj;
        }

        public override int GetHashCode()
        {
            return (Real, Imaginary).GetHashCode();
        }

        // Operations between complex numbers are overridden.
        public static Complex operator +(Complex a, Complex b)
        {
            return new Complex(a.Real.Value + b.Real.Value, a.Imaginary.Value + b.Imaginary.Value);
        }

        public static Complex operator -(Complex a, Complex b)
        {
            return new Complex(a.Real.Value - b.Real.Value, a.Imaginary.Value - b.Imaginary.Value);
        }

        public static Complex operator *(Complex a, Complex b)
        {
            return new Complex(a.Real.Value * b.Real.Value - a.Imaginary.Value * b.Imaginary.Value, 
                a.Real.Value * b.Imaginary.Value + a.Imaginary.Value * b.Real.Value);
        }

        public static Complex operator /(Complex a, Complex b)
        {
            Real denom = b.Real.Value * b.Real.Value + b.Imaginary.Value * b.Imaginary.Value;
            return new Complex((a.Real.Value * b.Real.Value + a.Value * b.Value) / denom.Value,
                (a.Imaginary.Value * b.Real.Value - a.Real.Value * b.Imaginary.Value) / denom.Value);
        }

        public static implicit operator Complex((double, double) t) => new Complex(t.Item1, t.Item2);
        public static implicit operator Complex(Real r) => new Complex(r.Value, 0);
        public static implicit operator Complex(Integer i) => new Complex((int) i.Value, 0);
        public static implicit operator Complex(Rational q) => new Complex(q.Value, 0);
    }
}

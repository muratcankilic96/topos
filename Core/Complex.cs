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
        /// Value represents the magnitude of a complex number.
        /// </summary>
        public override double Value
        {
            get
            {
                return Sqrt(Pow(Imaginary.Value, 2) + Pow(Real.Value, 2));
            }
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

        public static implicit operator Complex((double, double) t) => new Complex(t.Item1, t.Item2);
    }
}

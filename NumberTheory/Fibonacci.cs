using static System.Math;
using System;
using Topos.Core;
using Topos.Core.ToposExceptions;

namespace Topos.NumberTheory
{
    public static class Fibonacci
    {
        /// <summary>
        /// Computes the value of the Fibonacci sequence for given real number.
        /// Uses Binet's formula for faster computation.
        /// </summary>
        /// <param name="x">A real number</param>
        /// <returns>The value of Fib(x)</returns>
        public static Real ComputeFibonacci(Real x)
        {
            Real pi = Real.Constant(RealConstant.pi);
            Real phi = Real.Constant(RealConstant.phi);
            return (Pow(phi, x) - Cos(x * pi) * Pow(phi, -x)) / Sqrt(5);
        }

        /// <summary>
        /// Computes the value of the Fibonacci sequence for given integer.
        /// Uses Binet's formula for faster computation.
        /// Supports negative inputs, which equals to Fib(-n) = (-1)^(n+1) Fib(n).
        /// </summary>
        /// <param name="n">An integer</param>
        /// <returns>nth value of the Fibonacci sequence</returns>
        public static Integer ComputeFibonacci(Integer n) => (long) ComputeFibonacci((Real)n);

        /// <summary>
        /// Computes the value of the Lucas sequence for given real number.
        /// Uses the property L(n) = F(n-1) + F(n+1)
        /// </summary>
        /// <param name="x">A real number</param>
        /// <returns>The value of Fib(x)</returns>
        public static Real ComputeLucas(Real x)
        {
            return ComputeFibonacci(x - 1) + ComputeFibonacci(x + 1);
        }

        /// <summary>
        /// Computes the value of the Lucas sequence for given integer.
        /// Uses the property L(n) = Fib(n-1) + Fib(n+1)
        /// </summary>
        /// <param name="n">An integer</param>
        /// <returns>nth value of the Lucas sequence</returns>
        public static Integer ComputeLucas(Integer n)
        {
            return (long) (ComputeFibonacci((Real)n - 1) + ComputeFibonacci((Real)n + 1));
        }

        /// <summary>
        /// Rewrites a positive integer n in its Zeckendorf representation.
        /// Zeckendorf presentation of n is of the form of sum
        /// of Fibonacci numbers where no terms are consecutive.
        /// A Zeckendorf representation is unique for integer n up to their ordering.
        /// </summary>
        /// <exception cref="UndefinedDomainException">Zeckendorf representation must take positive integers.</exception>
        /// <param name="n">A positive integer n</param>
        /// <returns>Zeckendorf representation of n</returns>
        public static string Zeckendorf(Integer n)
        {
            // Nonpositive domain is undefined.
            if (n < 1)
                throw new UndefinedDomainException("Zeckendorf representation must take positive integers.");

            // Trivial cases
            if (n == 1) return "1";
            if (n == 2) return "2";

            // Choose largest Fibonacci number that is smaller than or equal to n.
            Integer fib;
            Integer i = 1;
            while (ComputeFibonacci(++i) <= n);
            fib = ComputeFibonacci(--i);

            // Greedy algorithm
            string representation = $"{fib} ";
            Integer remainder = n - fib;
            while(remainder != 0)
            {
                // Choose largest Fibonacci number that is smaller than or equal to remainder.
                i = 1;
                while (ComputeFibonacci(++i) <= remainder) ;
                fib = ComputeFibonacci(--i);
                representation += $"+ {fib} ";
                remainder -= fib;
            }
            return representation;
        }
    }
}

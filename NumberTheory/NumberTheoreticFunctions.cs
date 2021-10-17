using System;
using System.Collections.Generic;
using System.Text;
using Topos.Core;

namespace Topos.NumberTheory
{
    /// <summary>
    /// A collection of several number-theoretic functions.
    /// Implements Euler totient function, divisor sigma function, divisor tau function, Möbius function.
    /// Depends on prime factorization.
    /// </summary>
    public static class NumberTheoreticFunctions
    {
        /// <summary>
        /// Computes how many positive relatively prime integers are there, up to the given integer.
        /// </summary>
        /// <param name="n">A nonnegative integer</param>
        /// <returns>Number of relatively prime integers 0 \< x \< n</returns>
        public static Integer EulerTotient(this Integer n)
        {
            // Negative domain is undefined.
            if (n < 0)
                throw new ArgumentOutOfRangeException("Euler totient function must take nonnegative integers.");

            // Base cases.
            else if (n < 2) return n;

            // Assume all integers up to the given integer are relatively prime.
            int count = n, multiples;

            // Iterate up to the square root of n.
            // Smallest prime factor of any n is less than or equal to sqrt(n).
            for(int i = 2; Math.Pow(i, 2) <= n; i++)
            {
                // If n is divisible by i, i is a factor of n.
                if(n % i == 0)
                {
                    // Eliminate all integers divisible by i, and its multiples from the list of relatively primes.
                    multiples = count / i;
                    count -= multiples;

                    // Disregard the given prime factor ({p_i}^{n_i}) from the number.
                    while (n % i == 0)
                        n /= i;
                }
            }
            // Eliminate remaining possible multiples
            multiples = count / n;

            return count - multiples;
        }

        /// <summary>
        /// Computes a special case of divisor function. 
        /// Returns the number of divisors of n.
        /// Degree 0: σ₀(n) = τ(n)
        /// </summary>
        /// <param name="n">A positive integer</param>
        /// <returns>Number of divisors of n</returns>
        public static Integer DivisorTau(this Integer n)
        {
            // Nonpositive domain is undefined.
            if (n <= 0)
                throw new ArgumentOutOfRangeException("Divisor function must take positive integers.");

            // Base case
            if (n == 1) return 1;
            List<MathObject> primeFactors = n.Factorize().ToList();

            List<Integer> primeExponents = new List<Integer>();

            // Extract exponents.
            foreach (MathObject m in primeFactors) 
            {
                Exponential exp = (Exponential)m;
                primeExponents.Add(exp.Index as Integer);
            }

            int sum = 1;

            foreach (Integer i in primeExponents) {
                sum *= (i + 1);
            }

            return sum;
        }

        #region to_do
        /// <summary>
        /// Computes a special case of divisor function. 
        /// Returns the sum of divisors of n.
        /// Degree 1: σ₁(n) = τ(n)
        /// </summary>
        /// <param name="n">A positive integer</param>
        /// <returns>Sum of divisors of n</returns>
        public static Integer DivisorSigma(this Integer n)
        {
            // TO-DO
            return null;
        }

        public static Integer MobiusMu(this Integer n)
        {
            // TO-DO
            return null;
        }

        #endregion
    }
}

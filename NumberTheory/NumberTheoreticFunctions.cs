using System;
using System.Collections.Generic;
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
        /// <exception cref="ArgumentOutOfRangeException">Euler totient function can only take nonnegative integers.</exception>
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
            int count = n;

            // Iterate up to the square root of n.
            // Smallest prime factor of any n is less than or equal to sqrt(n).
            for(int i = 2; Math.Pow(i, 2) <= n; i++)
            {
                // If n is divisible by i, i is a factor of n.
                if(n % i == 0)
                {
                    // Disregard the given prime factor ({p_i}^{n_i}) from the number.
                    while (n % i == 0)
                        n /= i;

                    // Eliminate all integers divisible by i, and its multiples from the list of relatively primes.
                    count -= count / i;
                }
            }
            // Eliminate remaining possible multiples
            if(n > 1) count -= count / n;

            return count;
        }

        /// <summary>
        /// Computes a special case of divisor function. 
        /// Returns the number of divisors of n.
        /// Degree 0: σ₀(n) = τ(n)
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Divisor function can only take positive integers.</exception>
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

            Integer product = 1;

            foreach (Integer i in primeExponents) {
                product *= (i + 1);
            }

            return product;
        }

        /// <summary>
        /// Computes a special case of divisor function. 
        /// Returns the sum of divisors of n.
        /// Degree 1: σ₁(n) = σ(n)
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Divisor function can only take positive integers.</exception>
        /// <param name="n">A positive integer</param>
        /// <returns>Sum of divisors of n</returns>
        public static Integer DivisorSigma(this Integer n)
        {
            // Call generalized divisor function of degree 1
            return (Integer) DivisorFunction(n, 1);
        }

        /// <summary>
        /// Computes the divisor function. 
        /// Returns the sum of xth powers of divisors of n.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Divisor function can only take positive integers.</exception>
        /// <param name="n">A positive integer</param>
        /// <param name="x">Degree of the divisor function</param>
        /// <returns>Sum of xth powers of divisors of n</returns>
        public static Real DivisorFunction(this Integer n, Real x)
        {
            // Nonpositive domain is undefined.
            if (n <= 0)
                throw new ArgumentOutOfRangeException("Divisor function must take positive integers.");

            // Call divisor Tau special case if x = 0 (General formula encounters division by zero)
            if (x == 0) return DivisorTau(n);

            // Base case
            if (n == 1) return 1;
            List<MathObject> primeFactors = n.Factorize().ToList();

            // Extract primes.

            Real product = 1;

            foreach (Exponential exp in primeFactors)
            {
                exp.Index = ((Real)exp.Index + 1) * x;
                Real expBase = Math.Pow((Real)exp.Base, x);
                product *= (exp.Compute() - 1) / (expBase - 1);
            }

            return product;
        }

        /// <summary>
        /// Möbius μ function is a function that returns either -1, 0, or 1 depending on
        /// the integer. It is used for Möbius inversion formula.
        /// </summary>
        /// <param name="n">A positive integer</param>
        /// <returns></returns>
        public static Integer MoebiusMu(this Integer n)
        {
            // Nonpositive domain is undefined.
            if (n <= 0)
                throw new ArgumentOutOfRangeException("Möbius μ function must take positive integers.");

            // Base case
            if(n == 1) return 1;

            // Factorize n
            Set primeFactorization = n.Factorize();

            // n is not square-free
            if (!primeFactorization.Equals(n.FactorizeUnique())) return 0;

            // n is square-free
            // Even number of primes return 1, odd number of primes return -1
            else return (int) -(primeFactorization.Cardinality % 2 * 2 - 1);
        }
    }
}

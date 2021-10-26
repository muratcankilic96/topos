using System;
using static System.Math;
using System.Collections.Generic;
using Topos.Core.ToposExceptions;
using Topos.Core;

namespace Topos.NumberTheory
{
    /// <summary>
    /// Primality class consists of methods regarding prime numbers and their factorization.
    /// </summary>
    public static class Primality
    {
        /// <summary>
        /// Checks whether the integer p is a prime.
        /// </summary>
        /// <param name="p">Input integer</param>
        /// <returns>Whether the integer p is a prime</returns>
        public static bool IsPrime(this Integer p)
        {
            // Theorem: φ(p) = p - 1 if and only if p is a prime.
            return p >= 0 && NumberTheoreticFunctions.EulerTotient(p) == p - 1;
        }

        /// <summary>
        /// Checks whether the integer p is a power of prime.
        /// </summary>
        /// <param name="p">Input integer</param>
        /// <returns>Whether the integer p is a power of prime</returns>
        public static bool IsPrimePower(this Integer p)
        {
            // Trivial
            if (p <= 1) return false;
            // 1th power of prime
            if (p.IsPrime()) return true;
            // Other powers
            Set factorization = p.FactorizeUnique();

            // Prime factorization of a power of prime only holds one unique prime factor
            if (factorization.Cardinality > 1) 
                return false;
            else 
                return true;
        }

        /// <summary>
        /// Checks whether the integer n is a composite.
        /// </summary>
        /// <param name="n">Input integer</param>
        /// <returns>Whether the integer n is a composite</returns>
        public static bool IsComposite(this Integer n)
        {
            // 0 and 1 are neither prime nor composite.
            return n > 1 && !IsPrime(n);
        }

        /// <summary>
        /// Returns the set of primes up to the given nonnegative integer.
        /// </summary>
        /// <param name="n">Upper bound</param>
        /// <returns>Set of primes up to the upper bound</returns>
        public static Set PrimesUpTo(Integer n)
        {
            // Negative domain is undefined.
            if (n < 0)
                throw new UndefinedDomainException("Prime factorization must take nonnegative integers.");
            // Some trivial cases are handled.
            else if (n == 1 || n == 0) return new Set();
            else if (n == 2) return new Set(2);

            // << TO-DO: Performance tweaqk for very large integers. Maybe Sieve of Atkin? >>

            // Initialize the list of numbers.
            Set sieve = new Set();

            // Assign the numbers to the list in order. First item is 2.
            // Multiples of 2 are redundant because the only even prime is 2.
            sieve.Add(2);
            for (int i = 3; i <= n; i += 2)
                sieve.Add(i);

            // Eliminate every odd multiple of encountered number except the number itself, up to square root of n
            for (int i = 3; i <= Sqrt(n); i++)
                for(int j = 3; j <= (n / i); j += 2)
                    sieve.Remove(i * j);

            // Return the pruned sieve, which only holds primes*/
            return sieve;
        }

        /// <summary>
        /// Expresses a positive integer n in terms of its prime factors.
        /// Since the factors are not unique, they are stored in terms of
        /// exponential forms.
        /// Computed by the support of Sieve of Eratosthenes.
        /// </summary>
        /// <param name="n">Input integer to factorize</param>
        /// <returns>Set of prime factors in terms of exponential forms</returns>
        public static Set Factorize(this Integer n)
        {
            // Get set elements.
            List<MathObject> setElements = PrimesUpTo(n).ToList();

            // Initialize the factorization set.
            Set factorSet = new Set();

            for(int i = 0; i < setElements.Count; i++)
            {
                // Extract the prime from the list.
                Integer p = (Integer)setElements[i];

                if (n % p == 0) 
                {
                    int exp = 0;
                    // Use value type to protect the reference.
                    int j = n;

                    // Divide until division is not possible.
                    while (j % p == 0) 
                    { 
                        j /= p;
                        exp++;
                    }
                    factorSet.Add(new Exponential(p, exp));
                }
            }

            return factorSet;
        }

        /// <summary>
        /// Expresses a positive integer n in terms of its unique prime factors.
        /// Computed by the support of Sieve of Eratosthenes.
        /// </summary>
        /// <param name="n">Input integer to factorize</param>
        /// <returns>Set of unique prime factors</returns>
        public static Set FactorizeUnique(this Integer n)
        {
            // Get set elements.
            List<MathObject> setElements = PrimesUpTo(n).ToList();

            // Initialize the factorization set.
            Set factorSet = new Set();

            for (int i = 0; i < setElements.Count; i++)
            {
                // Extract the prime from the list.
                Integer p = (Integer)setElements[i];

                if (n % p == 0)
                    factorSet.Add(p);
            }

            return factorSet;
        }
    }
}

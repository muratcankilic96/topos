using System;
using static System.Math;
using Topos.Core;
using System.Text;

namespace Topos.NumberTheory
{
    /// <summary>
    /// Division is a class that includes the functions related to the integer division.
    /// </summary>
    public static class Division
    {
        /// <summary>
        /// Checks whether Integer a is divisible by Integer b.
        /// Implemented as an extension method.
        /// </summary>
        /// <param name="a">Integer to be divided by b</param>
        /// <param name="b">Integer that divides a</param>
        /// <returns></returns>
        public static bool IsDivisibleBy(this Integer a, Integer b)
        {
            // Ignore signs.
            return Abs(a) % Abs(b) == 0;
        }

        /// <summary>
        /// Checks whether given two integers are relatively prime.
        /// Implemented as an extension method.
        /// </summary>
        /// <param name="a">First integer</param>
        /// <param name="b">Second integer</param>
        /// <returns>Whether given two integers are relatively prime</returns>
        public static bool IsRelativelyPrime(this Integer a, Integer b)
        {
            return Gcd(a, b) == 1;
        }

        /// <summary>
        /// Checks whether the integers listed are relatively prime.
        /// </summary>
        /// <param name="numbers">List of integers</param>
        /// <returns>Whether the integers listed are relatively prime</returns>
        public static bool IsRelativelyPrime(Integer[] numbers)
        {
            return Gcd(numbers) == 1;
        }

        /// <summary>
        /// Computes the greatest common divisor of two integers.
        /// </summary>
        /// <param name="a">First integer</param>
        /// <param name="b">Second integer</param>
        /// <returns>Gcd of given two integers</returns>
        public static Integer Gcd(Integer a, Integer b)
        {
            // Pass integers by value to avoid modifying reference
            // It is proven that gcd(a, b) = gcd(-a, b) = gcd(a, -b) = gcd(-a, -b),
            // however % operator is not modulus and works different
            // in negative numbers.
            int a_int = Abs(a);
            int b_int = Abs(b);

            // If at least one of the integers is 0, return the other integer as result
            if (a_int == 0 || b_int == 0)
                return Max(a_int, b_int);

            // Select smaller integer as modulus
            int modulus = Min(a_int, b_int);

            // Select larger integer as dividend
            int dividend = Max(a_int, b_int);

            // Start Euclidean algorithm
            do
            {
                // Store previous value of modulus
                int temp = modulus;

                // Iterate
                modulus = dividend % modulus;
                dividend = temp;
            } while (modulus != 0);

            // Return the remainder as result
            return dividend;
        }

        /// <summary>
        /// Computes the greatest common divisor of the integers listed.
        /// </summary>
        /// <param name="numbers">List of integers</param>
        /// <returns>Gcd of the listed integers</returns>
        public static Integer Gcd(params Integer[] numbers)
        {
            // Throw exception for invalid number of inputs
            if (numbers.Length < 2)
                throw new ArgumentException();

            // Use recursive definition to compute Gcd from left to right for each number
            for(int i = 0; i < numbers.Length - 1; i++)
                numbers[i + 1] = Gcd(numbers[i], numbers[i + 1]);

            // Return the result
            return numbers[numbers.Length - 1];
        }

        /// <summary>
        /// Computes the least common multiple of two integers.
        /// </summary>
        /// <param name="a">First integer</param>
        /// <param name="b">Second integer</param>
        /// <returns>Lcm of given two integers</returns>
        public static Integer Lcm(Integer a, Integer b)
        {
            return (a * b) / Gcd(a, b);
        }

        /// <summary>
        /// Computes the least common multiple of the integers listed.
        /// </summary>
        /// <param name="numbers">List of integers</param>
        /// <returns>Lcm of the listed integers</returns>
        public static Integer Lcm(params Integer[] numbers)
        {
            // Throw exception for invalid number of inputs
            if (numbers.Length < 2)
                throw new ArgumentException();

            // Use recursive definition to compute Lcm from left to right for each number
            for (int i = 0; i < numbers.Length - 1; i++)
                numbers[i + 1] = Lcm(numbers[i], numbers[i + 1]);

            // Return the result
            return numbers[numbers.Length - 1];
        }
    }
}

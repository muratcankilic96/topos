using System;
using System.Collections.Generic;
using System.Text;
using Topos.Core;

namespace Topos.NumberTheory
{
    /// <summary>
    /// Integer congruence relations provide modular arithmetic on base n.
    /// </summary>
    public class IntegerCongruence: Congruence<Integer>
    {
        /// <summary>
        /// Base of the integer congruence relation structure.
        /// </summary>
        public override Integer Base
        {
            get; set;
        }

        /// <summary>
        /// Defines an integer congruence relation structure on given base.
        /// </summary>
        /// <param name="modulusBase">Base of the congruence relation</param>
        public IntegerCongruence(Integer n)
        {
            if (n > 1)
                Base = n;
            else
                throw new ArgumentOutOfRangeException("Base of congruence relation structure must be larger than 1.");
        }

        /// <summary>
        /// Checks whether given integers are congruent to each other mod n.
        /// </summary>
        /// <param name="a">First integer</param>
        /// <param name="b">Second integer</param>
        /// <returns>Whether given integers are congruent to each other mod n</returns>
        public override bool IsCongruent(Integer a, Integer b)
        {
            return Mod(a) == Mod(b);
        }

        /// <summary>
        /// Applies modulo operation on the given integer.
        /// </summary>
        /// <param name="a">Integer to be operated</param>
        /// <returns>Result of the modulo operation</returns>
        public override Integer Mod(Integer a)
        {
            return (a % Base + Base) % Base;
        }

        /// <summary>
        /// Applies modulo operation on the given exponential.
        /// Both the base and the index of the exponential must be Integer types.
        /// Uses congruence properties for faster computation.
        /// </summary>
        /// <param name="a">Exponential to be operated</param>
        /// <returns>Result of the modulo operation</returns>
        public Integer Mod(Exponential exp)
        {
            // Extract base and index
            int expBase  = (Integer) exp.Base;
            int expIndex = (Integer) exp.Index;

            // Index must non-negative.
            if(expIndex < 0)
                throw new ArgumentOutOfRangeException("Index of the exponent must be non-negative.");

            // Theorem: For a^b = x (mod n), b = y (mod φ(n)).
            // With this, a large index can be reduced into a smaller one.
            expIndex = new IntegerCongruence(Base.EulerTotient()).Mod(expIndex);

            // Trivial case
            if (expIndex == 0)
                return 1;

            // Iteration
            expBase = Mod(expBase);
            int val = expBase;
            for (Integer i = 2; i <= expIndex; i++)
                val = Mod(val * expBase);
            return val;
        }

        /// <summary>
        /// Computes the order of a mod n.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public Integer Order(Integer a)
        {
            // Theorem: x | φ(n) where x is order of a mod n
            Integer phi = Base.EulerTotient();
            Set divisors = phi.Divisors();

            // Compare possible orders
            foreach(Integer divisor in divisors)
            {
                Exponential exp = new Exponential(a, divisor);
                if (IsCongruent(Mod(exp), 1)) 
                {
                    return divisor;
                }
            }
            return phi;
        }
    }
}

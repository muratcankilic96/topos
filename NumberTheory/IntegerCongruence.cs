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
    }
}

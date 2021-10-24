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
        #region core
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
            if (n > 0)
                Base = n;
            else
                throw new ArgumentOutOfRangeException("Base of congruence relation structure must be larger than 0.");
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

            // Theorem: For a^b ≡ x (mod n), b ≡ y (mod φ(n)).
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

        #endregion

        #region order

        /// <summary>
        /// Computes the order of a mod n.
        /// gcd(a, n) = 1 must hold.
        /// </summary>
        /// <param name="a">Integer a modulo n</param>
        /// <returns>Order a modulo n, or 0 if gcd(a, n) = 1 does not hold</returns>
        public Integer Order(Integer a)
        {
            if (!Division.IsRelativelyPrime(a, Base)) return 0;
            // Theorem: x | φ(n) where x is order of a mod n
            Integer phi = Base.EulerTotient();
            Set divisors = phi.Divisors();

            // Compare possible orders
            foreach(Integer divisor in divisors)
            {
                Exponential exp = new Exponential(Mod(a), divisor);
                if (IsCongruent(Mod(exp), 1)) 
                {
                    return divisor;
                }
            }
            return phi;
        }

        #endregion

        #region primitive_roots

        /// <summary>
        /// Counts how many primitive roots the integer congruence
        /// relation structure can hold.
        /// </summary>
        /// <returns>Number of possible primitive roots of the integer congruence</returns>
        public Integer CountPrimitiveRoots()
        {
            if (HasPrimitiveRoots())
                return Base.EulerTotient().EulerTotient();
            else return 0;
        }

        /// <summary>
        /// Checks whether the integer congruence relation structure has at least 1 primitive
        /// roots. 
        /// Only bases 1, 2, 4, and numbers of the form p^k, 2p^k where p is an odd prime can hold primitive roots. 
        /// </summary>
        /// <returns>Whether the integer congruence relation structure has primitive roots</returns>
        public bool HasPrimitiveRoots() 
        {
            // Trivial case
            if (Base == 1 || Base == 2 || Base == 4) return true;
            // Check odd prime power
            if(Primality.IsPrimePower(Base)) 
            { 
                // Even prime powers are invalid.
                if (Base % 2 == 0) return false;
                else return true;
            }
            // Check double of odd prime power.
            else if (Base % 2 == 0 && Primality.IsPrimePower(Base / 2)) return true;

            // Otherwise it holds 0 primitive roots.
            else return false; 
        }

        /// <summary>
        /// Checks whether the r is a primitive root.
        /// r is a primitive root if order of r modulo n is 1.
        /// </summary>
        /// <param name="r">An integer</param>
        /// <returns>Whether the r is a primitive root</returns>
        public bool IsPrimitiveRoot(Integer r) 
        {
            if (Order(r) == Base.EulerTotient()) return true;
            else return false;
        }

        /// <summary>
        /// Returns the set of all primitive roots modulo n.
        /// </summary>
        /// <returns>Set of all primitive roots modulo n</returns>
        public Set PrimitiveRoots() 
        {
            // Trivial case
            if (Base == 1) return new Set(0);

            List<Integer> primitiveRoots = new List<Integer>();

            if (!HasPrimitiveRoots()) return new Set();

            // Find a primitive root
            Integer r = 0;

            while (!IsPrimitiveRoot(++r)) ;

            primitiveRoots.Add(r);

            // Powers of primitive roots that are relatively prime with φ(n)
            // are also primitive roots. We can generate other primitive roots
            // from the first one.

            Integer phi_n = Base.EulerTotient();

            for (Integer i = 2; i <= phi_n; i++)
            {
                if(i.IsRelativelyPrime(phi_n))
                {
                    Integer r_pow = Mod(new Exponential(r, i));
                    primitiveRoots.Add(r_pow);
                }
            }

            // Sort the divisors.
            primitiveRoots.Sort((x, y) => ((int)x).CompareTo(y));

            return new Set(primitiveRoots.ToArray());
        }

        #endregion

        #region inverse

        /// <summary>
        /// Determines the additive inverse of x modulo n.
        /// </summary>
        /// <param name="x">An integer modulo n</param>
        /// <returns>Additive inverse of x modulo n</returns>
        public Integer AdditiveInverse(Integer x) 
        { 
            return Mod(-x); 
        }

        /// <summary>
        /// Determines the multiplicative inverse of a modulo n.
        /// gcd(a, n) = 1 must hold, otherwise n does not have 
        /// a multiplicative inverse.
        /// </summary>
        /// <param name="a">An integer modulo n</param>
        /// <returns>Multiplicative inverse of a modulo n, or 0 if gcd(a, n) = 1 does not hold</returns>
        public Integer MultiplicativeInverse(Integer a)
        {
            if (!a.IsRelativelyPrime(Base)) return 0;
            // Theorem: (Euler's generalization of Fermat's little theorem) a^φ(n) ≡ 1 (mod n)
            // Use this fact to rewrite the equation as  a^(-1) ≡ a^(-1) * a^φ(n) ≡ a^(φ(n) - 1) (mod n)
            return Mod(new Exponential(a, Base.EulerTotient() - 1));
        }

        #endregion

        #region to-do
        public Integer Index(Integer a, Integer r) { return 0; }

        public Set SolveLinear(Integer a, Integer b) { return new Set(); }

        #endregion
    }
}

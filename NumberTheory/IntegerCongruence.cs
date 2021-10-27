using static System.Math;
using System.Collections.Generic;
using Topos.Core;
using Topos.Core.ToposExceptions;
using System;

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
                throw new UndefinedDomainException("Base of congruence relation structure must be larger than 0.");
        }

        /// <summary>
        /// Checks whether given integers are congruent to each other mod n.
        /// </summary>
        /// <param name="a">First integer modulo n</param>
        /// <param name="b">Second integer modulo n</param>
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
        /// For negative indices, the base must have a multiplicative inverse.
        /// </summary>
        /// <exception cref="UndefinedDomainException">For negative indices, the base must have a multiplicative inverse</exception>
        /// <param name="a">Exponential to be operated</param>
        /// <returns>Result of the modulo operation</returns>
        public Integer Mod(Exponential exp)
        {
            // Extract base and index
            int expBase  = (Integer) exp.Base;
            int expIndex = (Integer) exp.Index;

            // Index must non-negative.
            if(expIndex < 0 && MultiplicativeInverse(expBase) == 0)
                throw new UndefinedDomainException("For negative indices, the base must have a multiplicative inverse.");

            // Theorem: Whenever gcd(a, n) = 1, for a^b ≡ x (mod n), b ≡ y (mod φ(n)).
            // With this, a large index can be reduced into a smaller one.
            if(Division.IsRelativelyPrime(expBase, Base))
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
        /// <param name="a">An integer modulo n</param>
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
        /// r is a primitive root if order of r modulo n is φ(n).
        /// </summary>
        /// <param name="r">An integer modulo n</param>
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

        #region linear_congruence

        /// <summary>
        /// Solves the linear congruence ax ≡ b (mod n).
        /// If there are no solutions, returns an empty set.
        /// </summary>
        /// <param name="a">Integer a modulo n</param>
        /// <param name="b">Integer b modulo n</param>
        /// <returns>Set of solutions of the linear congruence</returns>
        public Set SolveLinear(Integer a, Integer b)
        {
            // Theorem: The linear congruence ax ≡ b (mod n) has a solution if and only if d = gcd(a, n) | b.
            // Theorem: If a linear congruence ax ≡ b (mod n) has a solution, then d is equal to the number of solutions modulo n.
            Integer d = Division.Gcd(a, Base);
            if (!b.IsDivisibleBy(d)) return new Set();

            // Theorem: If ca ≡ cb (mod n) and d = gcd(c, n), then a ≡ b (mod n / d).
            // Other solutions can be generated from the solution of reduced congruence.
            Integer reducedModulo = Base / d;
            IntegerCongruence reducedCongruence = new IntegerCongruence(reducedModulo);

            Set solutions = new Set();

            // First solution.
            Integer x_0 = reducedCongruence.Mod(reducedCongruence.MultiplicativeInverse(a) * b);

            solutions.Add(x_0);

            // Generate other solutions
            for(Integer i = 1; i < d; i++)
                solutions.Add(x_0 + i * reducedModulo);

            return solutions;
        }

        #endregion

        #region index

        /// <summary>
        /// Finds the index base r of a modulo n.
        /// Index is the value x in r^x ≡ a (mod n).
        /// <exception cref="UndefinedDomainException">Throws exception if the input r is not a primitive root or input a mod n is 0.</exception>
        /// </summary>
        /// <param name="a">An integer modulo n</param>
        /// <param name="r">A primitive root modulo n</param>
        /// <returns>Index base r of a</returns>
        public Integer Index(Integer a, Integer r) 
        {
            // Congruence
            r = Mod(r);
            a = Mod(a);

            // r must be a primitive root and a must be non-zero mod n.
            if (!IsPrimitiveRoot(r)) throw new UndefinedDomainException($"{r} must be a primitive root.");
            else if (a == 0) throw new UndefinedDomainException($"Index base {r} of 0 is undefined.");

            // Trivial cases
            else if (a == 1) return 0;
            else if (a == r) return 1;

            // Uses Baby-Step Giant-Step for efficiency since integer mod n is a finite cyclic group.
            
            Integer c = (int) Ceiling(Sqrt(Base));
            Dictionary<Integer, Integer> s = new Dictionary<Integer, Integer>();

            // Store all exponents up to the ceiling of square root of base in a set.
            for (Integer i = 0; i < c; i++)
            {
                Integer mod = Mod(new Exponential(r, i));
                s.Add(i, mod);
            }
            // Compute r^(-c)
            Integer inverse = Mod(new Exponential(MultiplicativeInverse(r), c));
            Integer a_ = a;

            // Pair check
            for(Integer i = 0; i < c; i++)
            {
                foreach(var d in s)
                {
                    if(d.Value == a_)
                        return Mod(i * c + d.Key);
                }
                a_ = Mod(a_ * inverse);
            }
            return 0;
        }

        #endregion

        #region quadratic_residue

        /// <summary>
        /// Checks whether a is a quadratic residue or not.
        /// For gcd(a, n) = 1 takes advantage of Legendre symbols.
        /// For gcd(a, n) != 1 uses brute force.
        /// a is a quadratic residue if and only if the equation 
        /// x^2 ≡ a (mod n) is solvable.
        /// Solvability rules:
        /// [Let n = 2^(r_0) * p_1^(r_1) * p_2^(r_2) * ... (p_k)^(r_k)]
        /// (1) a ≡ 0 (mod n) or a ≡ 1 (mod n)
        /// (2) n = 1 or n = 2
        /// (3) If n is odd, then all (a / p_i) = 1 i = 1, 2, ..., r
        /// (4) If n is even, rule (2) (except even prime factor) and
        ///   (a) a ≡ 1 (mod 4) if 4 | n but 8 ∤ n
        ///   (b) a ≡ 1 (mod 8) if 8 | n
        /// </summary>
        /// <param name="a">An integer modulo n</param>
        /// <returns>Whether a is a quadratic residue or not</returns>
        public bool IsQuadraticResidue(Integer a)
        {
            a = Mod(a);

            // Trivial cases
            if (Base <= 2 || a <= 1) return true;

            // Case gcd(a, n) = 1
            if (a.IsRelativelyPrime(Base)) 
            {
                // Prime case
                if (Base.IsPrime())
                    return Legendre(a) == 1;

                // Non-prime cases
                Set primes = Base.FactorizeUnique();

                primes.Remove(2);

                foreach(Integer p in primes)
                    if (new IntegerCongruence(p).Legendre(a) != 1) return false;

                // Odd case
                if (Base % 2 == 1)
                    return true;
                else
                // Even case
                    return (Base % 2 == 0 && Base % 4 != 0 && Base % 8 != 0) || (Base % 4 == 0 && Base % 8 != 0 && a % 4 == 1) || (Base % 8 == 0 && a % 8 == 1);
            }
            else
            // Case gcd(a, n) != 1 (Worst-case complexity O(n))
            {
                for(Integer i = 2; i < Base; i++)
                {
                    if (IsCongruent(Mod(new Exponential(i, 2)), a)) 
                        return true;
                }
                return false;
            }
        }

        /// <inheritdoc cref="Legendre(Integer, Integer)"/>
        public static Integer Legendre(Integer a, Integer n)
        {
            return new IntegerCongruence(n).Legendre(a);
        }

        /// <summary>
        /// Legendre symbol is a multiplicative function defined as:
        ///  1 : if a is a quadratic residue modulo p
        /// -1 : if a is a quadratic nonresidue modulo p
        ///  0 : if a ≡ 0 modulo p
        /// Its generalization is Jacobi symbol, where it can be written the multiplication
        /// of prime factorizations of the modulo base.
        /// This function implements Jacobi symbol, hence
        /// Legendre symbol is also denoted as (a / p) where p is an odd prime.
        /// Its generalization also does not support even numbers.
        /// </summary>
        /// <exception cref="UndefinedDomainException">Legendre and Jacobi symbols are undefined for even bases. 
        /// Use IsQuadraticResidue instead.</exception>
        /// <param name="a">An integer modulo n</param>
        /// <param name="n">Base of the integer congruence</param>
        /// <returns>-1, 1, or 0 depending on the input</returns>
        public Integer Legendre(Integer a)
        {
            // Legendre symbol is not supported for even bases.
            if(Base % 2 == 0)
                throw new UndefinedDomainException("Legendre and Jacobi symbols are undefined for even bases. Use IsQuadraticResidue instead.");

            // Trivial cases
            if (Base == 1) return 1;

            // If at least for one prime factor p_i, (a / p_i) = 0 the result is 0.
            a = Mod(a);

            if (a == 0) return 0;

            // Solve base case of the recursion
            // Base case implies that n is a prime and a ≡ -1, 1, 2, or 3 (mod p)

            Integer minus_zero = -AdditiveInverse(a);

            if(Base.IsPrime()) 
            { 
                if(a == minus_zero)
                {
                    // Theorem: (-1 / p) = 1 if p ≡ 1 (mod 4) and -1 otherwise
                    if (Base % 4 == 1) return 1;
                    else return -1;
                }
                else if(a == 1)
                {
                    // Theorem: (1 / p) = 1
                    return 1;
                }
                else if (a == 2)
                {
                    // Theorem: (2 / p) = 1 if p ≡ 1 (mod 8) or p ≡ 7 (mod 8), and -1 otherwise
                    if (Base % 8 == 1 || Base % 8 == 7) return 1;
                    else return -1;
                }
                else if (a == 3)
                {
                    // Theorem: (3 / p) = 1 if p ≡ 1 or 11 (mod 12) and -1 otherwise
                    if (Base % 12 == 1 || Base % 12 == 11) return 1;
                    else return -1;
                }
                else if (a.IsPrime())
                {
                    // For (p / q), if p and q are primes, apply quadratic reciprocity law
                    // (p / q) = (q / p) if q ≡ 1 (mod 4) or p ≡ 1 (mod 4), -(q / p) otherwise.
                    if (a % 4 == 1 || Base % 4 == 1)
                        return new IntegerCongruence(a).Legendre(Base);
                    else
                        return -new IntegerCongruence(a).Legendre(Base);
                }
                else // a is composite
                {
                    Integer result = 1;
                    // Theorem: (ab / p) = (a / p)(b / p)
                    Set a_primes = a.Factorize();

                    // Factorize the base into its primes
                    foreach (Exponential qs in a_primes)
                    {
                        // Get the prime and its power
                        Integer q = (Integer)qs.Base;
                        Integer s = (Integer)qs.Index;

                        // Theorem: (q^2 / p) = 1
                        // Even powers can be ignored.
                        if (s % 2 == 1)
                        {
                            result *= Legendre(q);
                        }
                    }
                    return result;
                }
            }
            else
            // Jacobi symbol implementation starts from here
            {
                Set n_primes = Base.Factorize();

                Integer result = 1;

                // Factorize the base into its primes
                foreach (Exponential pr in n_primes)
                {
                    // Get the prime and its power
                    Integer p = (Integer)pr.Base;
                    Integer r = (Integer)pr.Index;

                    // Even powers q_i of r_i always result with (a / p_i)^(r_i) = 1.
                    // They will not affect the overall result. Then, only operate on odd powers.
                    if (r % 2 == 1)
                    {
                        result *= new IntegerCongruence(p).Legendre(a);
                    }
                }
                return result;
            }
        }

        #endregion

        #region override

        public override string ToString()
        {
            return $"a ≡ b (mod {Base})";
        }

        #endregion
    }
}

using System.Collections.Generic;
using System.Linq;

namespace Topos.Core
{
    /// <summary>
    /// A function is a relation over sets A and B,
    /// where its domain is equal to the pre-image of B,
    /// and if aRx and aRy, then x = y.
    /// </summary>
    public class Function : BinaryRelation
    {
        #region core

        // Checking the properties of a function can be computationally comprehensive.
        // Once computed, store the boolean variable.
        private bool? isInjective = null, isSurjective = null;

        /// <summary>
        /// Defines an empty function.
        /// </summary>
        public Function(): base() { }

        /// <summary>
        /// Defines a single-variable function f: A -> B with given mappings.
        /// If the given mapping is invalid, it is ignored.
        /// </summary>
        /// <param name="a">Domain set of function</param>
        /// <param name="b">Codomain set of function</param>
        /// <param name="mappings">Mappings in terms of ordered pairs</param>
        public Function(Set a, Set b, params (MathObject, MathObject)[] mappings)
        {
            Set redundantObjects = new Set();
            foreach (var mapping in mappings)
            {
                MathObject pairIn = mapping.Item1;
                MathObject pairOut = mapping.Item2;
                // By definition: If aRx and aRy, then x = y.
                // Hence, ignore later additions completely.
                if (!redundantObjects.Contains(pairIn) && pairIn.IsMemberOf(a) && pairOut.IsMemberOf(b)) 
                { 
                    elements.Add(new OrderedTuple(pairIn, pairOut));
                    redundantObjects.Add(pairIn);
                }
            }

            // By definition: Domain is equal to the pre-image of B.
            Domain = PreImageOf(b);
            Codomain = b;
        }

        /// <summary>
        /// Defines a single-variable function f: S -> S with given mappings.
        /// If the given mapping is invalid, it is ignored.
        /// </summary>
        /// <param name="s">Domain and codomain sets of function</param>
        /// <param name="mappings">Mappings in terms of ordered pairs</param>
        public Function(Set s, params (MathObject, MathObject)[] mappings)
        {
            Set redundantObjects = new Set();
            foreach (var mapping in mappings)
            {
                MathObject pairIn = mapping.Item1;
                MathObject pairOut = mapping.Item2;
                // By definition: If aRx and aRy, then x = y.
                // Hence, ignore later additions completely.
                if (!redundantObjects.Contains(pairIn) && pairIn.IsMemberOf(s) && pairOut.IsMemberOf(s))
                {
                    elements.Add(new OrderedTuple(pairIn, pairOut));
                    redundantObjects.Add(pairIn);
                }
            }

            // By definition: Domain is equal to the pre-image of B.
            Domain = PreImageOf(s);
            Codomain = s;
        }

        #endregion

        #region mapping

        /// <summary>
        /// Maps the input to the corresponding element in the range.
        /// Inputting an invalid element returns an empty set.
        /// </summary>
        /// <param name="x">Input element</param>
        /// <returns>Corresponding element from the range</returns>
        public new MathObject Map(MathObject x)
        {
            // Add related members from the set.
            foreach (OrderedTuple t in elements)
                if (t[0].Equals(x))
                    return t[1];
            return new Set();
        }

        #endregion

        #region composition_operation

        /// <summary>
        /// Computes the composition of two functions f and g.
        /// Composition of f and g is the set of all f(g(x)).
        /// </summary>
        /// <param name="f">First function</param>
        /// <param name="g">Second function</param>
        /// <returns>The function composition f o g = f(g(x))</returns>
        public static Function Composition(Function f, Function g)
        {
            List<(MathObject, MathObject)> mappings = new List<(MathObject, MathObject)>();
            // S o R
            foreach (MathObject m in g.Domain)
            {
                MathObject mapping = f.Map(g.Map(m));
                mappings.Add((m, mapping));
            }

            return new Function(g.Domain, f.Codomain, mappings.ToArray());
        }

        #endregion

        #region logical_checks
        /// <summary>
        /// Checks whether the function f is injective or not,
        /// which means if f(x) = a and f(y) = a, then x = y.
        /// If f is injective, then the pre-image of each y 
        /// in the Codomain has cardinality of at most 1.
        /// </summary>
        /// <returns>Whether the function is injective or not</returns>
        public bool IsInjective()
        {
            if(isInjective == null) 
            { 
            foreach(MathObject m in Codomain.ToList())
                {
                    if (InverseMap(m).Cardinality > 1) 
                    {
                        isInjective = false;
                        return false;
                    }
                }
            

            isInjective = true;
            return true;
            }
            return (bool)isInjective;
        }

        /// <summary>
        /// Checks whether the function f is surjective or not,
        /// which means every element in the codomain is related
        /// with some element in the domain.
        /// </summary>
        /// <returns>Whether the function is surjective or not</returns>
        public bool IsSurjective()
        {
            if (isSurjective == null)
            {
                if(Codomain.Equals(Range)) { 
                    isSurjective = true;
                    return true;
                } 
                else
                {
                    isSurjective = false;
                    return false;
                }
            }
            return (bool)isSurjective;
        }

        /// <summary>
        /// Checks whether the function f is bijective or not,
        /// which means f is both injective and surjective.
        /// </summary>
        /// <returns>Whether the function is bijective or not</returns>
        public bool IsBijective()
        {
            return IsInjective() && IsSurjective();
        }

        #endregion

        #region override
        public override string ToString()
        {
            return $"[Function]\n[Domain]: {Domain}" +
                $"\n[Codomain]: {Codomain}\n" +
                $"[Mappings]: {new Set(elements.ToArray())}";
        }

        /// <summary>
        /// Computes the composition of two functions f and g.
        /// Composition of f and g is the set of all f(g(x)).
        /// </summary>
        /// <param name="f">First function</param>
        /// <param name="g">Second function</param>
        /// <returns>The function composition f o g = f(g(x))</returns>
        public static Function operator *(Function f, Function g)
        {
            // Reversed
            return Composition(f, g);
        }

        #endregion
    }
}

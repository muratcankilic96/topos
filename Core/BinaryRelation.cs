using System;
using System.Collections.Generic;
using System.Text;

namespace Topos.Core
{
    /// <summary>
    /// Determines a special type of binary relation.
    /// </summary>
    public enum BinaryRelationType { Empty, Universal }

    /// <summary>
    /// A binary relation is an arbitrary subset of the Cartesian product A x B of sets A and B. 
    /// Binary relations hold basis for binary operations and functions.
    /// </summary>
    public class BinaryRelation : Set
    {
        #region core

        /// <summary>
        /// Domain of the relation, which is the input set of the relation.
        /// </summary>
        public Set Domain { get; }

        /// <summary>
        /// Codomain of the relation, which is the output set of the relation.
        /// </summary>
        public Set Codomain { get; }

        /// <summary>
        /// Range of the relation, which is the subset of the output set of the relation,
        /// where the elements unrelated with the domain are excluded.
        /// </summary>
        public Set Range 
        {
            get => ImageOf(Domain);
        }

        /// <summary>
        /// Pre-image of the relation, which is the subset of the input set of the relation,
        /// where the elements unrelated with the codomain are excluded.
        /// </summary>
        public Set PreImage
        {
            get => PreImageOf(Codomain);
        }
        
        // Checking the properties of a relation can be computationally comprehensive.
        // Once computed, store the boolean variable.
        private bool? isHomogeneous = null, isReflexive = null, isSymmetric = null, isTransitive = null, isAntiSymmetric = null;

        /// <summary>
        /// Defines an empty binary relation.
        /// </summary>
        /// <param name="a">Domain set of relation</param>
        /// <param name="b">Codomain set of relation</param>
        public BinaryRelation(Set a, Set b): base()
        {
            Domain = CopyFrom(a);
            Codomain = CopyFrom(b);
        }

        /// <summary>
        /// Defines a special binary relation.
        /// </summary>
        /// <param name="a">Domain set of relation</param>
        /// <param name="b">Codomain set of relation</param>
        public BinaryRelation(Set a, Set b, BinaryRelationType type)
        {
            Domain = CopyFrom(a);
            Codomain = CopyFrom(b);

            switch (type)
            {
                case BinaryRelationType.Empty:
                    elements = new HashSet<MathObject>();
                    break;
                case BinaryRelationType.Universal:
                    elements = new HashSet<MathObject>(CartesianProduct(a, b).ToList());
                    break;
            }
        }

        /// <summary>
        /// Defines a homogeneous binary relation with given mappings from Set S to S.
        /// If the given mapping is invalid, it is ignored.
        /// </summary>
        /// <param name="s">Domain and codomain sets of relation</param>
        /// <param name="mappings">Mappings in terms of ordered pairs</param>
        public BinaryRelation(Set s, params (MathObject, MathObject)[] mappings)
        {
            Domain = Codomain = CopyFrom(s);

            foreach (var mapping in mappings)
            {
                MathObject pairIn = mapping.Item1;
                MathObject pairOut = mapping.Item2;
                if (pairIn.IsMemberOf(s) && pairOut.IsMemberOf(s))
                    elements.Add(new OrderedTuple(pairIn, pairOut));
            }
        }

        /// <summary>
        /// Defines a heterogeneous binary relation with given mappings.
        /// If the given mapping is invalid, it is ignored.
        /// </summary>
        /// <param name="a">Domain set of relation</param>
        /// <param name="b">Codomain set of relation</param>
        /// <param name="mappings">Mappings in terms of ordered pairs</param>
        public BinaryRelation(Set a, Set b, params (MathObject, MathObject)[] mappings)
        {
            Domain = CopyFrom(a);
            Codomain = CopyFrom(b);

            foreach (var mapping in mappings)
            {
                MathObject pairIn = mapping.Item1;
                MathObject pairOut = mapping.Item2;
                if (pairIn.IsMemberOf(a) && pairOut.IsMemberOf(b)) 
                    elements.Add(new OrderedTuple(pairIn, pairOut));
            }
        }

        #endregion

        #region collection_operations

        /// <summary>
        /// Adds an element to the set.
        /// (Invalid for binary relations.)
        /// </summary>
        /// <param name="obj">The element to be added</param>
        public override void Add(MathObject obj)
        {
        }

        /// <summary>
        /// Removes an element from the set.
        /// (Invalid for binary relations.)
        /// </summary>
        /// <param name="obj">The element to be removed</param>
        /// <returns>Whether the deletion is successful or not</returns>
        public override bool Remove(MathObject obj)
        {
            return false;
        }

        #endregion

        #region mapping

        /// <summary>
        /// Maps the input to the corresponding elements in the range.
        /// Inputting an invalid element returns an empty set.
        /// For an equivalence relation, returns its equivalence class for the input.
        /// </summary>
        /// <param name="x">Input element</param>
        /// <returns>Set of corresponding elements from the range</returns>
        public virtual Set Map(MathObject x)
        {
            // Create an empty set.
            Set s = new Set();

            // Add related members from the set.
            foreach (OrderedTuple t in elements)
                if (t[0].Equals(x)) 
                    s.Add(t[1]);

            // Return the set.
            return s;
        }

        /// <summary>
        /// Inversely maps the input to the corresponding elements in the pre-image.
        /// Inputting an invalid element returns an empty set.
        /// For an equivalence relation, returns its equivalence class for the input.
        /// </summary>
        /// <param name="x">Input element</param>
        /// <returns>Set of corresponding elements from the pre-image</returns>
        public virtual Set InverseMap(MathObject x)
        {
            // Create an empty set.
            Set s = new Set();

            // Add related members from the set.
            foreach (OrderedTuple t in elements)
                if (t[1].Equals(x))
                    s.Add(t[0]);

            // Return the set.
            return s;
        }

        /// <summary>
        /// Determines the image of the corresponding elements in the range.
        /// Invalid elements in the set are ignored.
        /// </summary>
        /// <param name="s">Input elements as a set</param>
        /// <returns>Set of corresponding elements from the range</returns>
        public virtual Set ImageOf(Set s)
        {
            Set t = new Set();
            foreach (var element in s.ToList())
                t = Union(t, Map(element));
            return t;
        }

        /// <summary>
        /// Determines the pre-image of the corresponding elements in the range.
        /// Invalid elements in the set are ignored.
        /// </summary>
        /// <param name="s">Input elements as a set</param>
        /// <returns>Set of corresponding elements from the pre-image</returns>
        public virtual Set PreImageOf(Set s)
        {
            Set t = new Set();
            foreach (var element in s.ToList())
                t = Union(t, InverseMap(element));
            return t;
        }

        /// <summary>
        /// Converses the binary relation. 
        /// (b, a) ∈ R' for any element (a, b) ∈ R.
        /// </summary>
        /// <returns>Converse of the binary relation</returns>
        public virtual BinaryRelation Converse()
        {
            (MathObject, MathObject)[] mappings = new (MathObject, MathObject)[Cardinality];
            int i = 0;
            // Apply inverse mapping.
            foreach (OrderedTuple x in elements)
                mappings[i++] = (x[1], x[0]);

            // Create the inverse binary relation.
            return new BinaryRelation(Codomain, Domain, mappings);
        }

        #endregion

        #region logical_checks

        /// <summary>
        /// Checks whether for binary relation R, aRb is valid.
        /// </summary>
        /// <param name="a">Left-hand side of the binary relation</param>
        /// <param name="b">Right-hand side of the binary relation</param>
        /// <returns>Whether aRb is valid.</returns>
        public bool IsRelated(MathObject a, MathObject b)
        {
            return Map(a).Contains(b);
        }

        /// <summary>
        /// Checks whether the binary relation R is universal or not
        /// </summary>
        /// <returns>Whether the binary relation is universal or not</returns>
        public bool IsUniversal()
        {
            // Definition: An universal binary operation R over sets A and B holds the property R = A x B.
            // Theorem: |A x B| = |A| * |B|.
            return Cardinality == Domain.Cardinality * Codomain.Cardinality;
        }

            #region homogeneous_properties

        /// <summary>
        /// Checks whether a binary relation R is homogeneous or not.
        /// Homogeneous binary relations have important properties which hold
        /// basis for equivalence relations.
        /// </summary>
        /// <returns>Whether a binary relation is homogeneous or not</returns>
        public bool IsHomogeneous()
        {
            if (isHomogeneous == null)
                return Domain.Equals(Codomain);
            else return (bool)isHomogeneous;
        }

        /// <summary>
        /// Checks whether the homogeneous binary relation R is reflexive or not,
        /// which means xRx always hold in the relation.
        /// Returns false if the binary relation is heterogeneous.
        /// </summary>
        /// <returns>Whether the homogeneous binary relation is reflexive or not</returns>
        public bool IsReflexive()
        {
            if (isReflexive == null)
            {
                // Ignore the rest for heterogeneous relation.
                if (!IsHomogeneous()) return false;

                // Check the members that hold reflexivity property.
                foreach (MathObject x in Domain.ToList())
                    if (!IsRelated(x, x)) 
                    {
                        isReflexive = false;
                        return false;
                    }

                // If all members of the domain hold reflexivity property, return true.
                isReflexive = true;
                return true;
            }
            else return (bool)isReflexive;
        }

        /// <summary>
        /// Checks whether the homogeneous binary relation R is symmetric or not,
        /// which means if xRy then yRx in the relation. 
        /// Returns false if the binary relation is heterogeneous.
        /// </summary>
        /// <returns>Whether the homogeneous binary relation is symmetric or not</returns>
        public bool IsSymmetric()
        {
            if (isSymmetric == null)
            {
                // Ignore the rest for heterogeneous relation.
                if (!IsHomogeneous()) return false;

                // Check the members that hold symmetry property.
                foreach (OrderedTuple t in elements)
                    if (!Contains(t.Inverse()))
                    {
                        isSymmetric = false;
                        return false;
                    }

                isSymmetric = true;
                return true;
            }
            else return (bool)isSymmetric;
        }

        /// <summary>
        /// Checks whether the homogeneous binary relation R is antisymmetric or not,
        /// which means if both xRy and yRx, then x = y.
        /// Returns false if the binary relation is heterogeneous.
        /// </summary>
        /// <returns>Whether the homogeneous binary relation is antisymmetric or not</returns>
        public bool IsAntiSymmetric()
        {
            if (isAntiSymmetric == null)
            {
                // Ignore the rest for heterogeneous relation.
                if (!IsHomogeneous()) return false;

                // Check the members that hold antisymmetry property.
                foreach (OrderedTuple t in elements)
                    if (Contains(t.Inverse()) && !t.Inverse().Equals(t))
                    {
                        isAntiSymmetric = false;
                        return false;
                    }

                isAntiSymmetric = true;
                return true;
            }
            else return (bool)isAntiSymmetric;
        }

        /// <summary>
        /// Checks whether the homogeneous binary relation R is transitive or not,
        /// which means if xRy and yRz, then xRz. 
        /// Returns false if the binary relation is heterogeneous.
        /// </summary>
        /// <returns>Whether the homogeneous binary relation is transitive or not</returns>
        public bool IsTransitive()
        {
            // TO-DO: Brute force method is computationally comprehensive. Find a better solution.
            if (isTransitive == null)
            {
                // Ignore the rest for heterogeneous relation.
                if (!IsHomogeneous()) return false;

                // Check the members that hold transitivity property.
                foreach (OrderedTuple t1 in elements)
                {
                    foreach (OrderedTuple t2 in elements)
                    {
                        if (t1[1] == t2[0] && !IsRelated(t1[0], t2[1]))
                        {
                            isTransitive = false;
                            return false;
                        }
                    }
                }

                isTransitive = true;
                return true;
            }
            else return (bool)isTransitive;
        }

        /// <summary>
        /// Checks whether the homogeneous binary relation R is an equivalence relation or not.
        /// An equivalence relation is reflexive, symmetric, and transitive.
        /// Returns false if the binary relation is heterogeneous.
        /// </summary>
        /// <returns>Whether the homogeneous binary relation is an equivalence relation or not</returns>
        public bool IsEquivalenceRelation()
        {
            // Ignore the rest for heterogeneous relation.
            if (!IsHomogeneous()) return false;

            return IsReflexive() && IsSymmetric() && IsTransitive();
        }

        /// <summary>
        /// Determines the equivalence classes for an equivalence relation.
        /// Returns empty set if the binary relation is not an equivalence relation.
        /// </summary>
        /// <returns>Set of all equivalence classes for an equivalence relation.</returns>
        public Set EquivalenceClasses()
        {
            Set s = new Set();

            // Ignore the rest for non-equivalence relation.
            if (!IsEquivalenceRelation()) return s;

            // Theorem: Intersection of any different two equivalence classes is an empty set.
            // Use this fact to eliminate redundant possibilities and increase performance.
            Set domainCopy = CopyFrom(Domain);
            
            foreach (MathObject m in Domain.ToList())
            { 
                Set equivalenceClass = new Set();

                // Get the equivalence class of m, if m is an element of domainCopy.
                if (domainCopy.Contains(m)) { 
                    equivalenceClass = Map(m);
                    s.Add(equivalenceClass);
                }

                // Eliminate all members of the equivalence class from mapping.
                domainCopy = Exclusion(domainCopy, equivalenceClass);
            }

            return s;
        }
        #endregion
        #endregion
    }
}

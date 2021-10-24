using System;
using System.Linq;
using System.Collections.Generic;

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
        public Set Domain { get; protected set; }

        /// <summary>
        /// Codomain of the relation, which is the output set of the relation.
        /// </summary>
        public Set Codomain { get; protected set; }

        /// <summary>
        /// Range of the relation, which is the subset of the output set of the relation,
        /// where the elements unrelated with the domain are excluded.
        /// </summary>
        public Set Range 
        {
            get => ImageOf(Domain);
            protected set { }
        }

        /// <summary>
        /// Pre-image of the relation, which is the subset of the input set of the relation,
        /// where the elements unrelated with the codomain are excluded.
        /// </summary>
        public Set PreImage
        {
            get => PreImageOf(Codomain);
            protected set { }
        }
        
        // Checking the properties of a relation can be computationally comprehensive.
        // Once computed, store the boolean variable.
        private bool? isHomogeneous = null, isReflexive = null, isSymmetric = null, isTransitive = null, isAntiSymmetric = null;

        /// <summary>
        /// Defines an empty binary relation.
        /// </summary>
        public BinaryRelation(): base() 
        { 
            Domain = Codomain = new Set(); 
        }

        /// <summary>
        /// Defines a null binary relation.
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
        public BinaryRelation(Set s, params (MathObject, MathObject)[] mappings) : this(s, s, mappings) { }

        /// <summary>
        /// Defines a homogeneous binary relation with given mappings from Set S to S.
        /// If the given mapping is invalid, it is ignored.
        /// </summary>
        /// <param name="s">Domain and codomain sets of relation</param>
        /// <param name="mappings">Mappings in terms of ordered pairs</param>
        public BinaryRelation(Set s, params OrderedTuple[] mappings) : this(s, s, mappings) { }

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

        /// <summary>
        /// Defines a heterogeneous binary relation with given mappings.
        /// If the given mapping is invalid, it is ignored.
        /// </summary>
        /// <param name="a">Domain set of relation</param>
        /// <param name="b">Codomain set of relation</param>
        /// <param name="mappings">Mappings in terms of ordered pairs</param>
        public BinaryRelation(Set a, Set b, params OrderedTuple[] mappings)
        {
            Domain = CopyFrom(a);
            Codomain = CopyFrom(b);

            foreach (var mapping in mappings)
            {
                if(mapping.Length == 2) 
                { 
                    MathObject pairIn = mapping[0];
                    MathObject pairOut = mapping[1];
                    if (pairIn.IsMemberOf(a) && pairOut.IsMemberOf(b))
                        elements.Add(new OrderedTuple(mapping));
                }
            }
        }
        #endregion

        #region diagonal

        /// <summary>
        /// Creates a homogeneous diagonal binary relation.
        /// Let R be a homogeneous relation over set A,
        /// then for all a ∈ A, aRa holds.
        /// </summary>
        /// <param name="a">Domain and codomain sets of relation</param>
        /// <returns>The diagonal binary relation</returns>
        public static BinaryRelation Diagonal(Set a)
        {
            (MathObject, MathObject)[] mappings = new (MathObject, MathObject)[a.Cardinality];

            int i = 0;

            foreach (MathObject m in a)
                mappings[i++] = (m, m);

            return new BinaryRelation(a, mappings);
        }

        #endregion diagonal

        #region restriction

        /// <summary>
        /// Restricts a binary relation R over A x B under smaller sets S ⊆ A
        /// and T ⊆ B. If subset relations do not hold, returns an empty binary 
        /// relation.
        /// </summary>
        /// <param name="s">Restricted domain of the binary relation</param>
        /// <param name="t">Restricted codomain of the binary relation</param>
        /// <returns>The restricted binary relation</returns>
        public BinaryRelation Restriction(Set s, Set t)
        {
            if (s.IsSubsetOf(Domain) && t.IsSubsetOf(Codomain))
                return new BinaryRelation(s, t, elements.Cast<OrderedTuple>().ToArray());
            else return new BinaryRelation();
        }

        /// <summary>
        /// Restricts a binary relation R over A under a smaller set S ⊆ A.
        /// If the subset relation do not hold, returns an empty binary 
        /// relation.
        /// </summary>
        /// <param name="s">Restricted domain and codomain of the binary relation</param>
        /// <returns>The restricted binary relation</returns>
        public BinaryRelation Restriction(Set s)
        {
            return Restriction(s, s);
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
        public override bool Remove(MathObject obj) => false;

        /// <summary>
        /// Adds a mapping to the binary relation. 
        /// Invalid mappings are ignored.
        /// </summary>
        /// <param name="map">The mapping to be added</param>
        public void Add((MathObject, MathObject) map)
        {
            if (map.Item1.IsMemberOf(Domain) && map.Item2.IsMemberOf(Codomain))
            {
                base.Add(new OrderedTuple(map.Item1, map.Item2));
            }
        }

        /// <summary>
        /// Removes a mapping from the binary relation. 
        /// Invalid mappings are ignored.
        /// </summary>
        /// <param name="map">The mapping to be removed</param>
        /// <returns>Whether the deletion is successful or not</returns>
        public bool Remove((MathObject, MathObject) map)
        {
            return base.Remove(new OrderedTuple(map.Item1, map.Item2));
        }

        #endregion

        #region binary_operations

        /// <summary>
        /// Applies union operation over two binary relations.
        /// </summary>
        /// <param name="s1">First binary relation</param>
        /// <param name="s2">Second binary relation</param>
        /// <returns>The union binary relation</returns>
        public static BinaryRelation Union(BinaryRelation r, BinaryRelation s)
        {
            // Construct copies.
            Set domain = Union(r.Domain, s.Domain);
            Set codomain = Union(r.Codomain, s.Codomain);

            BinaryRelation rus = new BinaryRelation(domain, codomain);
            rus.elements = new HashSet<MathObject>(Union(new Set(r.ToArray()), new Set(s.ToArray())).ToList());
            return rus;
        }

        /// <summary>
        /// Applies generalized union operation over any number of binary relations.
        /// </summary>
        /// <param name="sets">A list of binary relations</param>
        /// <returns>The union binary relation</returns>
        public static BinaryRelation Union(params BinaryRelation[] rels)
        {
            // Throw exception for invalid number of inputs
            if (rels.Length < 2)
                throw new ArgumentException();

            // Construct original copy.
            BinaryRelation originRels = new BinaryRelation(rels[0].Domain, rels[0].Codomain);
            originRels.elements = new HashSet<MathObject>(rels[0].ToList());

            for (int i = 1; i < rels.Length; i++)
                originRels = Union(originRels, rels[i]);
            return originRels;
        }

        /// <summary>
        /// Applies intersection operation over two binary relations.
        /// </summary>
        /// <param name="s1">First binary relation</param>
        /// <param name="s2">Second binary relation</param>
        /// <returns>The intersection binary relation</returns>
        public static BinaryRelation Intersection(BinaryRelation r, BinaryRelation s)
        {
            // Construct copies.
            Set domain = Intersection(r.Domain, s.Domain);
            Set codomain = Intersection(r.Codomain, s.Codomain);

            BinaryRelation rus = new BinaryRelation(domain, codomain);
            rus.elements = new HashSet<MathObject>(Intersection(new Set(r.ToArray()), new Set(s.ToArray())).ToList());
            return rus;
        }

        /// <summary>
        /// Applies generalized intersection operation over any number of binary relations.
        /// </summary>
        /// /// <param name="sets">A list of binary relations</param>
        /// <returns>The intersection binary relation</returns>
        public static BinaryRelation Intersection(params BinaryRelation[] rels)
        {
            // Throw exception for invalid number of inputs
            if (rels.Length < 2)
                throw new ArgumentException();

            // Construct original copy.
            BinaryRelation originRels = new BinaryRelation(rels[0].Domain, rels[0].Codomain);
            originRels.elements = new HashSet<MathObject>(rels[0].ToList());

            for (int i = 1; i < rels.Length; i++)
                originRels = Intersection(originRels, rels[i]);
            return originRels;
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
            foreach (var element in s)
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
            return elements.Contains(new OrderedTuple(a, b));
        }

        /// <summary>
        /// Checks whether the binary relation R is trivial or not
        /// </summary>
        /// <returns>Whether the binary relation is trivial or not</returns>
        public bool IsTrivial()
        {
            // Definition: An trivial binary operation R over sets A and B holds the property R = A x B.
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
                foreach (MathObject x in Domain)
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
                        if (t1[1].Equals(t2[0]) && !IsRelated(t1[0], t2[1]))
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
            
            foreach (MathObject m in Domain)
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

        #region closure_operations

        /// <summary>l
        /// Generates the reflexive closure of a homogeneous binary relation.
        /// Returns a reference to itself if the binary relation is heterogeneous.
        /// </summary>
        /// <returns>The smallest reflexive relation containing R</returns>
        public BinaryRelation ReflexiveClosure()
        {
            if (!IsHomogeneous())
                return this;
            return Union(this, Diagonal(Domain));
        }

        /// <summary>
        /// Generates the symmetric closure of a homogeneous binary relation.
        /// Returns a reference to itself if the binary relation is heterogeneous.
        /// </summary>
        /// <returns>The smallest symmetric relation containing R</returns>
        public BinaryRelation SymmetricClosure()
        {
            if (!IsHomogeneous())
                return this;
            return Union(this, Converse());
        }

        /// <summary>
        /// Generates the transitive closure of a homogeneous binary relation.
        /// Returns a reference to itself if the binary relation is heterogeneous.
        /// </summary>
        /// <returns>The smallest transitive relation containing R</returns>
        public BinaryRelation TransitiveClosure()
        {
            if (!IsHomogeneous())
                return this;
            BinaryRelation r = Union(this, this * this);
            while (!(r = Union(r, r * this)).Equals(r)) { }
            return r;
        }

        /// <summary>
        /// Generates the equivalence closure of a homogeneous binary relation.
        /// Returns a reference to itself if the binary relation is heterogeneous.
        /// </summary>
        /// <returns>The equivalence closure of R</returns>
        public BinaryRelation EquivalenceClosure()
        {
            if (!IsHomogeneous())
                return this;
            BinaryRelation r = ReflexiveClosure();
            r = r.SymmetricClosure();
            r = r.TransitiveClosure();
            return r;
        }

        #endregion

        #region composition_operation

        /// <summary>
        /// Computes the composition of two relations R and S.
        /// Composition of R and S is the set of all (a, c) where aSb and bRc.
        /// </summary>
        /// <param name="s">First relation</param>
        /// <param name="r">Second relation</param>
        /// <returns>The binary relation composition S o R</returns>
        public static BinaryRelation Composition(BinaryRelation s, BinaryRelation r)
        {
            Set cartesian = new Set();
            // S o R
            foreach(MathObject m in r.Domain)
            {
                Set mapping = s.ImageOf(r.ImageOf(new Set(m)));
                cartesian = Union(cartesian, CartesianProduct(new Set(m), mapping));
            }
            // Convert OrderedTuple objects to C# pairs.

            (MathObject, MathObject)[] pairs = new (MathObject, MathObject)[cartesian.Cardinality];

            int i = 0;
            foreach(OrderedTuple t in cartesian)
                pairs[i++] = (t[0], t[1]);

            return new BinaryRelation(r.Domain, s.Codomain, pairs);
        }

        #endregion

        #region override
        public override string ToString()
        {
            return $"[Binary relation]\n[Domain]: {Domain}" +
                $"\n[Codomain]: {Codomain}\n" +
                $"[Mappings]: {base.ToString()}";
        }

        public static bool operator ==(BinaryRelation a, BinaryRelation b)
        {
            return a.Domain.Equals(b.Domain) && a.Codomain.Equals(b.Codomain) && a.elements.SetEquals(b.elements);
        }

        public static bool operator !=(BinaryRelation a, BinaryRelation b)
        {
            return !a.Equals(b);
        }

        /// <summary>
        /// Computes the composition of two relations R and S.
        /// Composition of R and S is the set of all (a, c) where aSb and bRc.
        /// </summary>
        /// <param name="s">First relation</param>
        /// <param name="r">Second relation</param>
        /// <returns>The binary relation composition S o R</returns>
        public static BinaryRelation operator *(BinaryRelation s, BinaryRelation r)
        {
            return Composition(s, r);
        }

        public override bool Equals(object obj)
        {
            if (obj is BinaryRelation)
                return this == (BinaryRelation)obj;
            else return false;
        }

        // Binary relations consist of three objects. All of them must be checked.
        public override int GetHashCode()
        {
            int hashCode = 0;

            foreach (MathObject m in Domain)
                hashCode ^= m.GetHashCode();

            foreach (MathObject m in Codomain)
                hashCode ^= m.GetHashCode();

            hashCode ^= base.GetHashCode();

            return hashCode;
        }
        #endregion
    }
}

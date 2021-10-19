using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Topos.Core
{
    /// <summary>
    /// A Set is a collection of objects that inherits MathObject class.
    /// Pure mathematical sets cannot be manipulated once defined, however
    /// in an instance of the Set object, it is possible to add or remove
    /// elements after definition.
    /// </summary>
    public class Set : MathObject
    {
        #region core
        protected HashSet<MathObject> elements;

        // Cardinality is a dummy variable. It is used to get the set cardinality.
        /// <summary>
        /// Gets the cardinality of the set
        /// </summary>
        public uint Cardinality
        { 
            get { return (uint)elements.Count; } 
        }
        
        /// <summary>
        /// Creates an empty set
        /// </summary>
        public Set()
        {
            elements = new HashSet<MathObject>();
        }

        /// <summary>
        /// Creates a set with given elements,
        /// with duplicate protection
        /// </summary>
        /// <param name="elements">List of elements</param>
        public Set(params MathObject[] elements)
        {
            this.elements = new HashSet<MathObject>(elements);
        }

        #endregion

        #region collection_operations

        /// <summary>
        /// Adds an element to the set
        /// </summary>
        /// <param name="obj">The element to be added</param>
        public void Add(MathObject obj)
        {
            elements.Add(obj);
        }

        /// <summary>
        /// Removes an element from the set
        /// </summary>
        /// <param name="obj">The element to be removed</param>
        /// <returns>Whether the deletion is successful or not</returns>
        public bool Remove(MathObject obj)
        {
            bool result = elements.Remove(obj);
            return result;
        }

        /// <summary>
        /// Converts the set to a list
        /// </summary>
        /// <returns>A list of MathObject types</returns>
        public virtual List<MathObject> ToList()
        {
            return elements.ToList();
        }

        /// <summary>
        /// Converts the set to an array
        /// </summary>
        /// <returns>An array of MathObject types</returns>
        public virtual MathObject[] ToArray()
        {
            return elements.ToArray();
        }

        /// <summary>
        /// Copies a set from another set
        /// </summary>
        /// <param name="set">The set to copy</param>
        public static Set CopyFrom(Set set)
        {
            Set setcopy = new Set
            {
                elements = new HashSet<MathObject>(set.elements)
            };
            return setcopy;
        }

        #endregion

        #region binary_operations

        /// <summary>
        /// Applies exclusion operation over two sets from HashSet implementation
        /// </summary>
        /// <param name="s1">First set</param>
        /// <param name="s2">Second set</param>
        /// <returns>The exclusion result set</returns>
        public static Set Exclusion(Set s1, Set s2)
        {
            // Construct copies.
            Set t1 = CopyFrom(s1);
            Set t2 = CopyFrom(s2);

            t1.elements.ExceptWith(t2.elements);
            return t1;
        }

        /// <summary>
        /// Applies union operation over two sets from HashSet implementation
        /// </summary>
        /// <param name="s1">First set</param>
        /// <param name="s2">Second set</param>
        /// <returns>The union set</returns>
        public static Set Union(Set s1, Set s2)
        {
            // Construct copies.
            Set t1 = CopyFrom(s1);
            Set t2 = CopyFrom(s2);

            t1.elements.UnionWith(t2.elements);
            return t1; 
        }

        /// <summary>
        /// Applies generalized union operation over any number of sets from HashSet implementation
        /// </summary>
        /// <param name="sets">A list of sets</param>
        /// <returns>The union set</returns>
        public static Set Union(params Set[] sets)
        {
            // Construct copies.
            Set[] setsCopy = new Set[sets.Length];

            for(int i = 0; i < sets.Length; i++)
                setsCopy[i] = CopyFrom(sets[i]);

            // Throw exception for invalid number of inputs
            if (setsCopy.Length < 2)
                throw new ArgumentException();

            for(int i = 1; i < sets.Length; i++)
                setsCopy[0].elements.UnionWith(setsCopy[i].elements);
            return setsCopy[0];
        }

        /// <summary>
        /// Applies intersection operation over two sets from HashSet implementation
        /// </summary>
        /// <param name="s1">First set</param>
        /// <param name="s2">Second set</param>
        /// <returns>The intersection set</returns>
        public static Set Intersection(Set s1, Set s2)
        {
            // Construct copies.
            Set t1 = CopyFrom(s1);
            Set t2 = CopyFrom(s2);

            t1.elements.IntersectWith(t2.elements);
            return t1;
        }

        /// <summary>
        /// Applies generalized intersection operation over any number of sets from HashSet implementation
        /// </summary>
        /// /// <param name="sets">A list of sets</param>
        /// <returns>The intersection set</returns>
        public static Set Intersection(params Set[] sets)
        {
            // Construct copies.
            Set[] setsCopy = new Set[sets.Length];

            for (int i = 0; i < sets.Length; i++)
                setsCopy[i] = CopyFrom(sets[i]);

            // Throw exception for invalid number of inputs
            if (setsCopy.Length < 2)
                throw new ArgumentException();

            for (int i = 1; i < setsCopy.Length; i++)
                setsCopy[0].elements.IntersectWith(setsCopy[i].elements);
            return setsCopy[0];
        }

        #endregion

        #region power_set

        /// <summary>
        /// Gets the power set of the set. Cardinality of a power set is 2^N, 
        /// where N is the cardinality of the input set.
        /// </summary>
        /// <returns>The power set of the set</returns>
        public Set PowerSet()
        {
            // Convert the collection into a list to track indices of elements
            List<MathObject> iterativeList = elements.ToList();

            // Initialize the power set
            Set powerSet = new Set();

            // It is proven that the cardinality of a power set equals to 2^n where n is
            // cardinality of the current set.
            int powerSetSize = (int) Math.Pow(2, elements.Count);
            for(int i = 0; i < powerSetSize; i++)
            {
                // Represent the current index as string
                string binaryRepresentation = Convert.ToString(i, 2);

                // Insert trailing zeroes to left
                binaryRepresentation = binaryRepresentation.PadLeft(elements.Count, '0');

                // Initialize the element of the power set
                Set powerSetElement = new Set();

                // Each digit in the binary representation shows whether that element will
                // be included or not.
                for(int j = 0; j < binaryRepresentation.Length; j++)
                    if(binaryRepresentation[j] == '1') powerSetElement.Add(iterativeList[j]);

                // Add current set into the power set
                powerSet.Add(powerSetElement);
            }

            // Return the power set
            return powerSet;
        }

        #endregion

        #region cartesian_product

        /// <summary>
        /// Computes the Cartesian product of two sets.
        /// </summary>
        /// <param name="a">First set</param>
        /// <param name="b">Second set</param>
        /// <returns>The Cartesian product set</returns>
        public static Set CartesianProduct(Set a, Set b)
        {
            Set cartesianSet = new Set();
            foreach(MathObject a_elem in a.elements)
            {
                foreach(MathObject b_elem in b.elements)
                {
                    cartesianSet.Add(new OrderedTuple(a_elem, b_elem));
                }
            }
            return cartesianSet;
        }

        /// <summary>
        /// Computes the generalized Cartesian product of given sets
        /// </summary>
        /// <param name="sets">A list of sets</param> 
        /// <returns>The Cartesian product set</returns>
        public static Set CartesianProduct(params Set[] sets)
        {
            Set cartesianSet = sets[0];
            for(int i = 1; i < sets.Length; i++)
            {
                cartesianSet = CartesianProduct(cartesianSet, sets[i]);
            }
            return cartesianSet;
        }

        #endregion

        #region logical_checks

        /// <summary>
        /// Checks whether the set is empty or not
        /// </summary>
        /// <returns>Whether the set is empty or not</returns>
        public bool IsEmpty() => elements.Count == 0;

        /// <summary>
        /// Checks whether the set is a singleton or not
        /// </summary>
        /// <returns>Whether the set is a singleton or not</returns>
        public bool IsSingleton() => elements.Count == 1;

        /// <summary>
        /// Checks whether the set contains the given element or not
        /// </summary>
        /// <param name="element">The element to check its existence</param>
        /// <returns>Whether the elements exists</returns>
        public bool Contains(MathObject element) => elements.Contains(element);

        /// <summary>
        /// Checks whether the set is a subset of the given set, including a trivial one
        /// </summary>
        /// <param name="superSet">The assumed superset of the given set</param>
        /// <returns>Whether the set is a subset of the given set or not</returns>
        public bool IsSubsetOf(Set superSet) => elements.IsSubsetOf(superSet.elements);

        /// <summary>
        /// Checks whether the set is a proper subset of the given set
        /// </summary>
        /// <param name="superSet">The assumed superset of the given set</param>
        /// <returns>Whether the set is a subset of the given set or not</returns>
        public bool IsProperSubsetOf(Set superSet) => elements.IsProperSubsetOf(superSet.elements);

        /// <summary>
        /// Checks whether the set is a superset of the given set, including a trivial one
        /// </summary>
        /// <param name="subset">The assumed subset of the given set</param>
        /// <returns>Whether the set is a superset of the given set or not</returns>
        public bool IsSupersetOf(Set subset) => elements.IsSupersetOf(subset.elements);

        /// <summary>
        /// Checks whether the set is a proper superset of the given set
        /// </summary>
        /// <param name="subset">The assumed subset of the given set</param>
        /// <returns>Whether the set is a superset of the given set or not</returns>
        public bool IsProperSupersetOf(Set subset) => elements.IsProperSupersetOf(subset.elements);

        /// <summary>
        /// Checks whether the set is a set of numbers or not.
        /// </summary>
        /// <returns>Whether the set is a set of numbers or not</returns>
        public virtual bool IsNumberCollection()
        {
            foreach(MathObject m in elements)
                if (!(m is Number)) return false;
            return true;
        }

        /// <summary>
        /// Checks whether the set is finite or not.
        /// </summary>
        /// <returns>Whether the set is finite or not</returns>
        public virtual bool IsFinite() => true;

        /// <summary>
        /// Checks whether the set is countable or not.
        /// Every finite set is countable.
        /// </summary>
        /// <returns>Whether the set is countable or not</returns>
        public virtual bool IsCountable() => true;

        #endregion

        #region override
        // Lists elements of a set. It exposes a recursive definition.
        private string ListElements()
        {
            // Check whether the set is empty or not
            if (elements.Count > 0)
            {
                string listString = "";

                string leftBracket = "{";
                string rightBracket = "}";

                int i = 0;
                foreach (MathObject element in elements)
                {
                    listString += element + ((i++ != elements.Count - 1) ? ", " : "");
                }
                return leftBracket + listString + rightBracket;
            } // Use empty set notation if the set is empty.
            else return "Ø";
        }

        public override string ToString()
        {
            return ListElements();
        }

        // Set comparison is independent on order of elements.
        public static bool operator ==(Set a, Set b)
        {
            return a.elements.SetEquals(b.elements);
        }

        public static bool operator !=(Set a, Set b)
        {
            return !a.elements.SetEquals(b.elements);
        }
        
        public override bool Equals(object obj)
        {
            if (obj is Set)
                return this == (Set)obj;
            else return false;
        }

        // Two separate collection types can contain same values but different references.
        // When that happens, their collections return different hash codes.
        // A manual implementation is required to overcome this problem.
        public override int GetHashCode()
        {
            int hashCode = 0;
            foreach (MathObject m in elements)
                hashCode ^= m.GetHashCode();
            return hashCode;
        }
        #endregion
    }
}

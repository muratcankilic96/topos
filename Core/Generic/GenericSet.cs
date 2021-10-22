using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Topos.Core.Generic
{
    /// <summary>
    /// A GenericSet is a special case of Set that can only hold one type
    /// of MathObject, which is useful on type protection in special types of
    /// applications.
    /// </summary>
    public class GenericSet<T> : MathObject, IEnumerable<T> where T: MathObject
    {
        #region core
        // Encapsulation over inheritance. Increases code count but solves casting issues.
        private Set set;

        /// <summary>
        /// Gets the cardinality of the generic set
        /// </summary>
        public uint Cardinality
        {
            get { return set.Cardinality; }
        }

        /// <summary>
        /// Creates an empty generic set
        /// </summary>
        public GenericSet()
        {
            set = new Set();
        }

        /// <summary>
        /// Creates a generic set with given elements,
        /// with duplicate protection
        /// </summary>
        /// <param name="elements">List of elements</param>
        public GenericSet(params T[] elements)
        {
            set = new Set(elements);
        }

        // This hidden constructor is used to pass sets faster.
        private GenericSet(Set s) => set = Set.CopyFrom(s);

        /// <summary>
        /// Converts the generic set into a set that can hold all kinds of
        /// MathObject types.
        /// </summary>
        /// <returns>The output set</returns>
        public Set ToSet() => set;

        #endregion

        #region collection_operations

        /// <summary>
        /// Adds an element to the generic set
        /// </summary>
        /// <param name="obj">The element to be added</param>
        public void Add(T obj)
        {
            set.Add(obj);
        }

        /// <summary>
        /// Removes an element from the generic set
        /// </summary>
        /// <param name="obj">The element to be removed</param>
        /// <returns>Whether the deletion is successful or not</returns>
        public bool Remove(T obj)
        {
            bool result = set.Remove(obj);
            return result;
        }

        /// <summary>
        /// Converts the generic set to a list
        /// </summary>
        /// <returns>A list of generic types</returns>
        public virtual List<T> ToList() => (List<T>)(object)set.ToList();

        /// <summary>
        /// Converts the generic set to an array
        /// </summary>
        /// <returns>An array of generic types</returns>
        public virtual T[] ToArray() => (T[])set.ToArray();

        /// <summary>
        /// Copies a generic set from another generic set
        /// </summary>
        /// <param name="set">The generic set to copy</param>
        public static GenericSet<T> CopyFrom(GenericSet<T> set)
        {
            GenericSet<T> setcopy = new GenericSet<T>
            {
                set = new Set(set)
            };
            return setcopy;
        }

        #endregion

        #region binary_operations

        /// <summary>
        /// Applies exclusion operation over two generic sets from HashSet implementation
        /// </summary>
        /// <param name="s1">First generic set</param>
        /// <param name="s2">Second generic set</param>
        /// <returns>The exclusion result generic set</returns>
        public static GenericSet<T> Exclusion(GenericSet<T> s1, GenericSet<T> s2) => new GenericSet<T>(Set.Exclusion(s1.set, s2.set));

        /// <summary>
        /// Applies union operation over two generic sets from HashSet implementation
        /// </summary>
        /// <param name="s1">First generic set</param>
        /// <param name="s2">Second generic set</param>
        /// <returns>The union generic set</returns>
        public static GenericSet<T> Union(GenericSet<T> s1, GenericSet<T> s2) => new GenericSet<T>(Set.Union(s1.set, s2.set));

        /// <summary>
        /// Applies generalized union operation over any number of generic sets from HashSet implementation
        /// </summary>
        /// <param name="sets">A list of generic sets</param>
        /// <returns>The union set</returns>
        public static GenericSet<T> Union(params GenericSet<T>[] sets)
        {
            // Throw exception for invalid number of inputs
            if (sets.Length < 2)
                throw new ArgumentException();

            // Arrange the sets
            Set[] _sets = new Set[sets.Length];
            for (int i = 0; i < sets.Length; i++)
                _sets[i] = sets[i].set;

            return new GenericSet<T>(Set.Union(_sets));
        }

        /// <summary>
        /// Applies intersection operation over two generic sets from HashSet implementation
        /// </summary>
        /// <param name="s1">First generic set</param>
        /// <param name="s2">Second generic set</param>
        /// <returns>The intersection generic set</returns>
        public static GenericSet<T> Intersection(GenericSet<T> s1, GenericSet<T> s2) => new GenericSet<T>(Set.Intersection(s1.set, s2.set));

        /// <summary>
        /// Applies generalized intersection operation over any number of generic sets from HashSet implementation
        /// </summary>
        /// /// <param name="sets">A list of generic sets</param>
        /// <returns>The intersection generic set</returns>
        public static GenericSet<T> Intersection(params GenericSet<T>[] sets)
        {
            // Throw exception for invalid number of inputs
            if (sets.Length < 2)
                throw new ArgumentException();

            // Arrange the sets
            Set[] _sets = new Set[sets.Length];
            for (int i = 0; i < sets.Length; i++)
                _sets[i] = sets[i].set;

            return new GenericSet<T>(Set.Intersection(_sets));
        }

        #endregion

        #region power_set
        /// <summary>
        /// Gets the power set of the generic set. Cardinality of a power set is 2^N, 
        /// where N is the cardinality of the input set.
        /// A power set cannot be generic.
        /// </summary>
        /// <returns>The power set of the set</returns>
        public Set PowerSet() => set.PowerSet();

        #endregion

        #region cartesian_product

        /// <summary>
        /// Computes the Cartesian product of two generic sets.
        /// The resulting set cannot be generic.
        /// </summary>
        /// <param name="a">First generic set</param>
        /// <param name="b">Second generic set</param>
        /// <returns>The Cartesian product generic set</returns>
        public static Set CartesianProduct(GenericSet<T> a, GenericSet<T> b)
        {
            return Set.CartesianProduct(a.set, b.set);

        }

        /// <summary>
        /// Computes the generalized Cartesian product of given generic sets.
        /// The resulting set cannot be generic.
        /// </summary>
        /// <param name="sets">A list of generic sets</param> 
        /// <returns>The Cartesian product set</returns>
        public static Set CartesianProduct(params GenericSet<T>[] sets)
        {
            // Throw exception for invalid number of inputs
            if (sets.Length < 2)
                throw new ArgumentException();

            // Arrange the sets
            Set[] _sets = new Set[sets.Length];
            for (int i = 0; i < sets.Length; i++)
                _sets[i] = sets[i].set;

            return Set.CartesianProduct(_sets);
        }

        #endregion

        #region logical_checks

        /// <summary>
        /// Checks whether the generic set is empty or not
        /// </summary>
        /// <returns>Whether the generic set is empty or not</returns>
        public bool IsEmpty() => set.Cardinality == 0;

        /// <summary>
        /// Checks whether the generic set is a singleton or not
        /// </summary>
        /// <returns>Whether the generic set is a singleton or not</returns>
        public bool IsSingleton() => set.Cardinality == 1;

        /// <summary>
        /// Checks whether the generic set contains the given element or not
        /// </summary>
        /// <param name="element">The element to check its existence</param>
        /// <returns>Whether the element exists</returns>
        public bool Contains(T element) => set.Contains(element);

        /// <summary>
        /// Checks whether the generic set is a subset of the given generic set, including a trivial one
        /// </summary>
        /// <param name="superSet">The assumed generic superset of the given generic set</param>
        /// <returns>Whether the generic set is a generic subset of the given generic set or not</returns>
        public bool IsSubsetOf(GenericSet<T> superSet) => set.IsSubsetOf(superSet.set);

        /// <summary>
        /// Checks whether the generic set is a proper generic subset of the given generic set
        /// </summary>
        /// <param name="superSet">The assumed superset of the given generic set</param>
        /// <returns>Whether the generic set is a generic subset of the given generic set or not</returns>
        public bool IsProperSubsetOf(GenericSet<T> superSet) => set.IsProperSubsetOf(superSet.set);

        /// <summary>
        /// Checks whether the set is a generic superset of the given generic set, including a trivial one
        /// </summary>
        /// <param name="subset">The assumed generic subset of the given generic set</param>
        /// <returns>Whether the generic set is a generic superset of the given generic set or not</returns>
        public bool IsSupersetOf(GenericSet<T> subset) => set.IsSupersetOf(subset.set);

        /// <summary>
        /// Checks whether the generic set is a proper generic superset of the given generic set
        /// </summary>
        /// <param name="subset">The assumed generic subset of the given generic set</param>
        /// <returns>Whether the generic set is a generic superset of the given generic set or not</returns>
        public bool IsProperSupersetOf(GenericSet<T> subset) => set.IsProperSupersetOf(subset.set);

        /// <summary>
        /// Checks whether the generic set is finite or not.
        /// </summary>
        /// <returns>Whether the generic set is finite or not</returns>
        public virtual bool IsFinite() => set.IsFinite();

        /// <summary>
        /// Checks whether the generic set is countable or not.
        /// Every finite set is countable.
        /// </summary>
        /// <returns>Whether the generic set is countable or not</returns>
        public virtual bool IsCountable() => set.IsCountable();

        #endregion

        #region override
        public override string ToString() => set.ToString();

        // Generic Set comparison is independent on order of elements.
        public static bool operator ==(GenericSet<T> a, GenericSet<T> b) => a.set == b.set;
        
        public static bool operator !=(GenericSet<T> a, GenericSet<T> b) => !(a.set == b.set);

        public override bool Equals(object obj)
        {
            if (obj is GenericSet<T>)
                return this == (GenericSet<T>)obj;
            else return false;
        }

        public override int GetHashCode() => set.GetHashCode();

        #endregion

        #region iteration
        IEnumerator IEnumerable.GetEnumerator()
        {
            return set.ToList().GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return (IEnumerator<T>)set.ToList().Cast<T>().GetEnumerator();
        }
        #endregion
    }
}

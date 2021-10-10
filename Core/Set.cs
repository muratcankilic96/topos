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
        HashSet<MathObject> elements;

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

        #endregion

        #region fundamental_operations

        /// <summary>
        /// Applies union operation over two sets from HashSet implementation
        /// </summary>
        /// <param name="s1">First set</param>
        /// <param name="s2">Second set</param>
        /// <returns></returns>
        public static Set Union(Set s1, Set s2)
        {
            s1.elements.Union(s2.elements);
            return s1;
        }

        /// <summary>
        /// Applies generalized union operation over any number of sets from HashSet implementation
        /// </summary>
        /// <returns></returns>
        public static Set Union(params Set[] sets)
        {
            // Throw exception for invalid number of inputs
            if (sets.Length < 2)
                throw new ArgumentException();

            for(int i = 1; i < sets.Length; i++)
                sets[0].elements.Union(sets[i].elements);
            return sets[0];
        }

        /// <summary>
        /// Applies intersection operation over two sets from HashSet implementation
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public static Set Intersection(Set s1, Set s2)
        {
            s1.elements.Intersect(s2.elements);
            return s1;
        }

        /// <summary>
        /// Applies generalized intersection operation over any number of sets from HashSet implementation
        /// </summary>
        /// <returns></returns>
        public static Set Intersection(params Set[] sets)
        {
            // Throw exception for invalid number of inputs
            if (sets.Length < 2)
                throw new ArgumentException();

            for (int i = 1; i < sets.Length; i++)
                sets[0].elements.Intersect(sets[i].elements);
            return sets[0];
        }

        #endregion

        #region power_set

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

        #region logical_checks

        /// <summary>
        /// Checks whether the set is empty or not
        /// </summary>
        /// <returns>Whether the set is empty or not</returns>
        public bool IsEmpty() => elements.Count == 0;
        

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
            return this == (Set)obj;
        }
        #endregion
    }
}

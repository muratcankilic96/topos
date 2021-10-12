using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Topos.Core
{
    /// <summary>
    /// Ordered tuples are collections of elements preserving order.
    /// Every ordered tuple is a Set.
    /// They are implemented according to Kuratowski's definition
    /// and represented as (a, b, ...) in syntax.
    /// </summary>
    public class OrderedTuple : Set
    {
        #region core
        private List<MathObject> tuple;

        /// <summary>
        /// Gets the length of the ordered tuple.
        /// </summary>
        public uint Length
        {
            get { return (uint)tuple.Count; }
        }

        // Constructs the set by Kuratowski's definition using recursion
        private Set KuratowskiConstruction(MathObject[] elements)
        {
            // Base case
            if(elements.Length < 3)
            {
                // Assignment for readability
                MathObject a = elements[0];
                MathObject b = elements[1];

                return new Set(new Set(a), new Set(a, b));
            } else { 
                // Divide by two parts
                int truncLength = elements.Length - 1;
                MathObject[] rest = new MathObject[truncLength];
                Array.Copy(elements, rest, truncLength);
                var last = elements[truncLength];

                // Assignment for readability
                MathObject a = KuratowskiConstruction(rest);
                MathObject b = last;

                return new Set(new Set(a), new Set(a, b));
            }
        }

        /// <summary>
        /// Creates an ordered n-tuple using recursive definition
        /// </summary>
        /// <param name="elements">Elements of the tuple</param>
        public OrderedTuple(params MathObject[] elements)
        {
            // Explode OrderedTuple into individual elements and combine to the list
            List<MathObject> elementsList = elements.ToList();
            foreach(MathObject m in elements) { 
                if(m is OrderedTuple)
                {
                    OrderedTuple orderedTuple = (OrderedTuple)m;
                    var orderedTupleList = orderedTuple.ToList();
                    elementsList.Remove(m);
                    elements = orderedTupleList.Concat(elementsList).ToArray();
                }
            }

            // Build according to Kuratowski definition of a set, recursively
            var kuratowskiList = KuratowskiConstruction(elements).ToList();
            this.elements = new HashSet<MathObject>(kuratowskiList);

            tuple = new List<MathObject>(elements);
        }

        #endregion

        #region collection_operations

        /// <summary>
        /// Gets element by its index
        /// Uses 0-indexing
        /// </summary>
        /// <param name="index">Index of the element</param>
        /// <returns></returns>
        public MathObject Project(int index)
        {
            return tuple[index];
        }

        /// <summary>
        /// Converts the ordered tuple to a list
        /// </summary>
        /// <returns>A list of MathObject types</returns>
        public override List<MathObject> ToList()
        {
            return tuple.ToList();
        }
        #endregion

        #region override
        // Represent a tuple of the form (a, b, ...).
        private string RepresentTuple()
        {
            string listString = "";

            int i = 0;
            foreach (MathObject t in tuple)
            {
                listString += t + ((i++ != tuple.Count - 1) ? ", " : "");
            }
            return "(" + listString + ")";
        }
        
        public override string ToString()
        {
            return RepresentTuple();
        }

        // Overridden operators use simplified definitions
        public static bool operator ==(OrderedTuple a, OrderedTuple b)
        {
            return a.tuple.SequenceEqual(b.tuple);
        }

        public static bool operator !=(OrderedTuple a, OrderedTuple b)
        {
            return !a.tuple.SequenceEqual(b.elements);
        }

        public override bool Equals(object obj)
        {
            return this == (Set) obj;
        }

        public override int GetHashCode()
        {
            return tuple.GetHashCode();
        }
        #endregion
    }
}

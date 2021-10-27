using System;
using System.Collections.Generic;
using System.Linq;
using Topos.Core.ToposExceptions;

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

        public MathObject this[int i]
        {
            get => Project(i); 
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
                    int id = elementsList.FindIndex(idx => idx.Equals(m));
                    elementsList.InsertRange(id, orderedTupleList);
                    elementsList.Remove(m);
                    elements = elementsList.ToArray();
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
        /// Creates a copy of the ordered tuple in reverse order.
        /// </summary>
        /// <returns>Ordered tuple in reverse order</returns>
        public OrderedTuple Inverse()
        {
            var tupleCopy = new List<MathObject>(tuple);
            tupleCopy.Reverse();

            return new OrderedTuple(tupleCopy.ToArray());
        }

        /// <summary>
        /// Converts the ordered tuple to a list
        /// </summary>
        /// <returns>A list of MathObject types</returns>
        public override List<MathObject> ToList()
        {
            return tuple.ToList();
        }

        /// <summary>
        /// Converts the ordered tuple to an array
        /// </summary>
        /// <returns>An array of MathObject types</returns>
        public override MathObject[] ToArray()
        {
            return tuple.ToArray();
        }
        #endregion

        #region logical_checks

        /// <summary>
        /// Checks whether the ordered tuple completely consists of numbers or not.
        /// </summary>
        /// <returns>Whether the ordered tuple completely consists of numbers or not</returns>
        public override bool IsNumberCollection()
        {
            foreach (MathObject m in tuple)
                if (!(m is Number)) return false;
            return true;
        }
        #endregion

        #region number_operations

        // Checks whether two ordered tuples are of the same length and contains only Real numbers.
        private static void NumericControl(OrderedTuple a, OrderedTuple b)
        {
            if (a.Length != b.Length)
                throw new DimensionMismatchException(a.Length, b.Length);

            if(!a.IsNumberCollection() || !b.IsNumberCollection())
                    throw new IndeterminateException();
        }

        // Operations between ordered tuples are overridden. 
        // Supports only Real numbers.
        // TODO: Expand the support for Complex numbers.
        public static OrderedTuple operator +(OrderedTuple a, OrderedTuple b)
        {
            // Throw exception if something is wrong.
            NumericControl(a, b);

            Real[] operatedOrderedTuple = new Real[a.Length];
            for (int i = 0; i < a.Length; i++) 
                    operatedOrderedTuple[i] = (Real)a.tuple[i] + (Real)b.tuple[i];

            return new OrderedTuple(operatedOrderedTuple.ToArray());
        }

        public static OrderedTuple operator -(OrderedTuple a, OrderedTuple b)
        {
            // Throw exception if something is wrong.
            NumericControl(a, b);

            Real[] operatedOrderedTuple = new Real[a.Length];
            for (int i = 0; i < a.Length; i++)
                operatedOrderedTuple[i] = (Real)a.tuple[i] - (Real)b.tuple[i];

            return new OrderedTuple(operatedOrderedTuple.ToArray());
        }

        public static OrderedTuple operator *(OrderedTuple a, OrderedTuple b)
        {
            // Throw exception if something is wrong.
            NumericControl(a, b);

            Real[] operatedOrderedTuple = new Real[a.Length];
            for (int i = 0; i < a.Length; i++)
                operatedOrderedTuple[i] = (Real)a.tuple[i] * (Real)b.tuple[i];

            return new OrderedTuple(operatedOrderedTuple.ToArray());
        }

        public static OrderedTuple operator /(OrderedTuple a, OrderedTuple b)
        {
            // Throw exception if something is wrong.
            NumericControl(a, b);

            Real[] operatedOrderedTuple = new Real[a.Length];
            for (int i = 0; i < a.Length; i++)
                operatedOrderedTuple[i] = (Real)a.tuple[i] / (Real)b.tuple[i];

            return new OrderedTuple(operatedOrderedTuple.ToArray());
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
            if (obj is OrderedTuple)
                return this == (OrderedTuple)obj;
            else return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion
    }
}

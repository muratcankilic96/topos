using System;
using Topos.Core.ToposExceptions;

namespace Topos.Core
{
    /// <summary>
    /// Exponential elements provide a representation of two
    /// different elements over an exponential operation.
    /// Its applications include but not limited to  
    /// computational simplifications in modular arithmetic.
    /// </summary>
    public class Exponential : Element
    {
        #region core
        /// <summary>
        /// Base of the exponential representation.
        /// </summary>
        public Element Base { get; set; }
        /// <summary>
        /// Index of the exponential representation.
        /// </summary>
        public Element Index { get; set; }

        /// <summary>
        /// Creates an exponential representation.
        /// They can also store invariants.
        /// </summary>
        /// <param name="basePart">Base part of the exponential representation</param>
        /// <param name="indexPart">Index part of the exponential representation</param>
        public Exponential(Element basePart, Element indexPart)
        {
            Base = basePart;
            Index = indexPart;
        }

        #endregion

        #region number
        /// <summary>
        /// Computes the exponential representation if they are real numbers.
        /// </summary>
        /// <returns></returns>
        public Real Compute()
        {
            // << TO-DO: REFACTOR IT! >>

            // Apply nested definitions
            if(Base is Exponential && Index is Real)
            {
                Exponential a = (Exponential)Base;
                Real b = (Real)Index;
                return new Real(Math.Pow(a.Compute(), b));
            }
            else if(Base is Real && Index is Exponential)
            {
                Real a = (Real)Base;
                Exponential b = (Exponential)Index;
                return new Real(Math.Pow(a, b.Compute()));
            }
            else if(Base is Exponential && Index is Exponential)
            {
                Exponential a = (Exponential)Base;
                Exponential b = (Exponential)Index;
                return new Real(Math.Pow(a.Compute(), b.Compute()));
            }
            // Check whether the representation is a real number or not
            else if (Base is Real && Index is Real)
            {
                Real a = (Real)Base;
                Real b = (Real)Index;
                return new Real(Math.Pow(a, b));
            }
            else if (Base is Complex || Index is Complex)
                throw new ComplexDomainException();
            else if (Base is Invariant || Index is Invariant)
                throw new InvariantException();
            else
                throw new ToposException("Encountered unknown type of element.");
        }
        #endregion 

        #region override
        public override string ToString()
        {
            // << TO-DO: REFACTOR IT! >>

            // Exponents up to the power of 1 are simplified.
            string IndexStr = Index.ToString();
            string expSymbol = "^";
            if (Index is Real && (Real)Index == 1) { 
                IndexStr = "";
                expSymbol = "";
            }

            if ((Base is Complex || Base is Exponential) && !(Index is Complex || Index is Exponential))
                return $"({Base}){expSymbol}{IndexStr}";
            else if (!(Base is Complex || Base is Exponential) && (Index is Complex || Index is Exponential))
                return $"{Base}{expSymbol}({IndexStr})";
            else if((Base is Complex || Base is Exponential) && (Index is Complex || Index is Exponential))
                return $"({Base}){expSymbol}({IndexStr})";
            else
                return $"{Base}{expSymbol}{IndexStr}";
        }

        #endregion

        #region override
        public static bool operator ==(Exponential a, Exponential b)
        {
            try
            {
                // Real expressions
                return a.Compute().Equals(b.Compute());
            } 
            catch
            {
                // Complex and invariant expressions
                return a.Base.Equals(b.Base) && a.Index.Equals(b.Index);
            }

        }

        public static bool operator !=(Exponential a, Exponential b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (obj is Exponential)
                return this == (Exponential)obj;
            else if (Base is Real && Index is Real && obj is Number)
                return Compute() == (Real)obj;
            return false;

        }

        public override int GetHashCode()
        {
            try
            {
                // Real expressions
                return Compute().GetHashCode();
            }
            catch
            {
                // Complex and invariant expressions
                return (Base, Index).GetHashCode();
            }
        }
        #endregion
    }
}

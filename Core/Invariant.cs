using System;
using System.Collections.Generic;
using System.Text;

namespace Topos.Core
{
    /// <summary>
    /// Invariant is a type of element that holds no extra properties.
    /// </summary>
    public class Invariant : Element
    {
        public string Identifier { get; set; }

        public Invariant(string name)
        {
            Identifier = name;
        }

        public override string ToString() => Identifier;

        public static implicit operator Invariant(string s) => new Invariant(s);

        public static bool operator ==(Invariant a, Invariant b)
        {
            return a.Identifier == b.Identifier;
        }

        public static bool operator !=(Invariant a, Invariant b)
        {
            return a.Identifier != b.Identifier;
        }

        public override bool Equals(object obj)
        {
            if (obj is Invariant)
                return this == (Invariant)obj;
            else return false;
        }

        public override int GetHashCode()
        {
            return Identifier.GetHashCode();
        }
    }
}

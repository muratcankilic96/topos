using System;
using System.Collections.Generic;
using System.Text;

namespace Topos.Core
{
    /// <summary>
    /// Indeterminate is a type of element that holds no extra properties.
    /// </summary>
    public class Indeterminate : Element
    {
        public string Identifier { get; set; }

        public Indeterminate(string name)
        {
            Identifier = name;
        }

        public override string ToString() => Identifier;

        public static implicit operator Indeterminate(string s) => new Indeterminate(s);

        public static bool operator ==(Indeterminate a, Indeterminate b)
        {
            return a.Identifier == b.Identifier;
        }

        public static bool operator !=(Indeterminate a, Indeterminate b)
        {
            return a.Identifier != b.Identifier;
        }

        public override bool Equals(object obj)
        {
            if (obj is Indeterminate)
                return this == (Indeterminate)obj;
            else return false;
        }

        public override int GetHashCode()
        {
            return Identifier.GetHashCode();
        }
    }
}

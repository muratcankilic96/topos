using System;

namespace Topos.Core
{
    /// <summary>
    /// A MathObject is the foundation base of sets and elements. It cannot
    /// be instantiated.
    /// </summary>
    public abstract class MathObject
    {
        public static implicit operator MathObject(string s) => new Invariant(s);
        public static implicit operator MathObject(double d) => new Real(d);
        public static implicit operator MathObject((double, double) t) => new Complex(t.Item1, t.Item2);
        public static implicit operator MathObject((int, int) t) => new Rational(t.Item1, t.Item2);
        public static implicit operator MathObject(int i) => new Integer(i);
    }
}

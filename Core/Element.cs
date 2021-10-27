namespace Topos.Core
{
    /// <summary>
    /// Elements are the atomic mathematical objects. They cannot be divided into further components.
    /// There are different types of elements.
    /// </summary>
    public abstract class Element : MathObject
    {
        public static implicit operator Element(string s) => new Indeterminate(s);
        public static implicit operator Element(double d) => new Real(d);
        public static implicit operator Element((double, double) t) => new Complex(t.Item1, t.Item2);
        public static implicit operator Element((int, int) t) => new Rational(t.Item1, t.Item2);
        public static implicit operator Element(int i) => new Integer(i);
    }
}

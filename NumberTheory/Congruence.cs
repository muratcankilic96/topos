using System;
using System.Collections.Generic;
using System.Text;
using Topos.Core;

namespace Topos.NumberTheory
{
    /// <summary>
    /// Congruence relations provide equivalence relations on an algebraic structure.
    /// </summary>
    /// <typeparam name="T">Number system to be implemented</typeparam>
    public abstract class Congruence<T> where T: Number
    {
        public abstract T Base { get; set; }
        public abstract bool IsCongruent(T a, T b);
        public abstract T Mod(T a);
    }
}

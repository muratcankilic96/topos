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
    public interface ICongruence<T> where T: Number
    {
        T Base { get; set; }
        bool IsCongruent(T a, T b);
        T Mod(T a);
    }
}

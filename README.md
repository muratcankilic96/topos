# Topos Math Library
Topos is a library for implementations of mathematical concepts for .NET Standard 2.0 environment. Based on Zermelo–Fraenkel set theory (ZFC).

A Set is an unordered container of mathematical objects, including nested definitions such as Set of Sets. 
My implementation takes .NET HashSet<T> as basis. However, Sets are not generic types, and can only hold objects of MathObject class.

Currently supported classes are:

* MathObject *(abstract)*
  * Element
    * Invariant
    * Number *(abstract)*
      * Real
        * Integer
        * Rational
      * Complex
  * Set
    * OrderedTuple
* Division
* Exception (.NET)
  * DimensionMismatchException
  * InvariantException

TO-DO:

* Primality tests
* Prime factorization
* Modular arithmetic 
  * Mod operation
  * Order
  * Primitive root
  * Index
  * Quadratic residue
  * Legendre and Jacobi symbols
* Relations and functions
* Infinite sets (Countably - Uncountably)
* ...

ISSUES:
* Complex number operations between ordered tuples are not supported.
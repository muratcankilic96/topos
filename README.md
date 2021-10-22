# Topos Math Library
Topos is a library for implementations of mathematical concepts for .NET Standard 2.0 environment. Based on Zermelo–Fraenkel set theory (ZFC). Currently only supports finite sets.

A Set is an unordered container of mathematical objects, including nested definitions such as Set of Sets. 
My implementation takes .NET HashSet<T> as basis. However, Sets are not generic types, and can only hold objects of MathObject class.

ZFC ensures that there are no atomic elements, however, to increase comprehension, I included atomic elements where Element is its base class.

Currently supported classes are:

Topos.Core
* MathObject *(abstract)*
  * Element
    * Invariant
    * Number *(abstract)*
      * Real
        * Integer
		  * Natural
        * Rational
      * Complex
	  * Exponential
  * Set
    * OrderedTuple
	* BinaryRelation
	  * Function
  
Topos.Core.Generic
* MathObject *(abstract)* (from Topos.Core)
  * GenericSet<T>
	
Topos.Core.Exceptions
* Exception *(.NET)*
  * ToposException
    * DimensionMismatchException
    * InvariantException
    * ComplexDomainException

Topos.NumberTheory
* MathObject *(Topos.Core)*
  * Congruence<T> *(abstract)*
    * IntegerCongruence
* Division *(static)*
* NumberTheoreticFunctions *(static)*
* Primality *(static)*

TO-DO:

* Exponentials will be represented as numbers, including complex number operations (will not support invariants)
* Modular arithmetic over integers
  * Order
  * Primitive root
  * Index
  * Quadratic residue
  * Legendre and Jacobi symbols
* Binary relations
  * Restrictions of binary relations
* Infinite sets (Countably - Uncountably)
* ...

ISSUES:
* Complex number operations between ordered tuples are not supported.
* Complex number operations over exponential representations are not supported.
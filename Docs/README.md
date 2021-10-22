
# Topos Math Library
Topos is a library for implementations of mathematical concepts for .NET Standard 2.0 environment. Based on Zermelo–Fraenkel set theory (ZFC). Currently only supports finite sets.
## Getting Started
Topos is easy to use. Every non-static object of Topos is derived from the abstract object MathObject. The fundamental mathematical concepts such as numbers, sets, ordered tuples, functions, and binary relations are stored in Topos.Core namespace. For a specific field of mathematics, you should call other Topos submodules, such as Topos.NumberTheory for number theory applications. (Yes, that one is only submodule right now.)

A set is a fundamental mathematical concept. Sets are unordered lists of elements. By definition, for every set S, there is another set T that contains S. This definition allows the sets to contain sets. I did not include the concept of proper classes because they are impractical since I did not implement (pseudo)infinite sets.

For example, let us create an even-odd relation over the set of first 10 natural numbers and test it is whether an equivalence relation or not (it is), then print its equivalence classes.

```C#
using System;
using System.Linq;
using Topos.Core;

public class someClass
{
	public Set SomeFunction() 
	{
	    // Step 1: Set building
	    Set firstTenNaturals = new Set();

	    for (Natural i = 0; i < amountOfNaturals; i++)
	        firstTenNaturals.Add(i);

	    // Step 2: Relation building
	    var odd  = new List<(MathObject, MathObject)>();
	    var even = new List<(MathObject, MathObject)>();

	    for(int i = 0; i < amountOfNaturals; i++)
	    {
	        for (int j = 0; j < amountOfNaturals; j++)
	        {
	            if (i % 2 == 0 && j % 2 == 0)
	                even.Add((i, j));
	            else if (i % 2 == 1 && j % 2 == 1)
	                odd.Add((i, j));
	        }
	    }
	    
	    var allMaps = odd.Concat(even).ToArray();

	    // Step 3: Creating the relation
	    BinaryRelation evenOddRelation = new BinaryRelation(firstTenNaturals, allMaps);

	    // Step 4: Checking the equivalence relation property
	    bool isEquivalence = evenOddRelation.IsEquivalenceRelation();

	    // Step 5: If it is a equivalence relation, printing the set of equivalence classes
	    Set equivalenceClasses = new Set();
	    if(isEquivalence) 
	    { 
	        Set equivalenceClasses = evenOddRelation.EquivalenceClasses();
	        Console.WriteLine(equivalenceClasses);
	    }
	    return equivalenceClasses;
    }
}
```

The output for this code will be
```
{{0, 2, 4, 6, 8}, {1, 3, 5, 7, 9}}
```
That means in the set of first 10 natural numbers, even numbers and odd numbers get their own sets, and each equivalence class is an element of the partition set.

However, instead of a brute force approach, to get this result, alternatively, we can build a small relation, then extend the relation using equivalence closure.

```C#
public class someClass
{
	public Set SomeAlternativeFunction() 
	{
         // Step 1: Set building
         Set firstTenNaturals = new Set();

         for (Natural i = 0; i < amountOfNaturals; i++)
             firstTenNaturals.Add(i);

         // Step 2: Relation building
         var odd  = new List<(MathObject, MathObject)>();
         var even = new List<(MathObject, MathObject)>();

         for (int i = 0; i < amountOfNaturals; i++)
         {
             for (int j = i + 2; j < amountOfNaturals; j++)
             {
                 if (i % 2 == 0 && j % 2 == 0)
                     even.Add((i, j));
                 else if (i % 2 == 1 && j % 2 == 1)
                     odd.Add((i, j));
             }
         }

         var allMaps = odd.Concat(even).ToArray();

         // Step 3: Creating the relation
         BinaryRelation simpleRelation = new BinaryRelation(firstTenNaturals, allMaps);

         // Step 4: Building another relation from its equivalence closure
         BinaryRelation evenOddRelation = simpleRelation.EquivalenceClosure();

         // Step 5: Printing the set of equivalence classes
         Set equivalenceClasses = evenOddRelation.EquivalenceClasses();
         Console.WriteLine(equivalenceClasses);
         return equivalenceClasses;
    }
}
```

The output for this code will be
```
{{2, 4, 6, 8, 0}, {3, 5, 7, 9, 1}}
```

This alternative code returns the same set with the first one, however the same relation is built from a smaller relation. The output is not numerically ordered, however it is not an issue, since sets are unordered by definition. We can test whether they are the same set or not.

````C#
    Set s1 = SomeFunction();
    Set s2 = SomeAlternativeFunction();

    if (s1 == s2)
        Console.WriteLine("They are equal sets.");
````

The output for this code will be
````
{{0, 2, 4, 6, 8}, {1, 3, 5, 7, 9}}
{{2, 4, 6, 8, 0}, {3, 5, 7, 9, 1}}
They are equal sets.
````

This is just one single example, and it depends upon your creativity on what kind of structures you can build. I also appreciate your contributions, of course!

I have created this HTML documentation using Doxygen for applications of specific mathematical objects.
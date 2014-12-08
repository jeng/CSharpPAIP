Chapter 2
=========

First Version
-------------
For the first example I added an extension method for OneOf and a singleton for the RNG.  The singleton keeps us from having to pass the object around and it only gets seeded once.

Things to note, new List...Concat...ToList is a lot longer than append.  I thought about adding another extension method but didn't want to spend the whole project trying to cover warts.

Another important point is we are even further from the Kleene star notation then the lisp example.  It is definitely harder to read. 

Second Version
--------------

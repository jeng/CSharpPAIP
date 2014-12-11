Chapter 2
=========

First Version
-------------

For the first example I added an extension method for OneOf and a singleton for
the RNG.  The singleton keeps us from having to pass the random object instance
around. Plus, we only need it seeded once.

Things to note, new List...Concat...ToList is a lot longer than append.  I
thought about adding another extension method but didn't want to spend the
whole project trying to cover warts.

Another important point is we are even further from the Kleene star notation
then the lisp example.  It is definitely harder to read. 

Second Version
--------------
We have a few different options when it comes to parsing the grammar:

- Regex
- Lexer 
- Dict with a string for the left side and a object that can represent the list
  of words or rule list on the right.

I went with the last choice and it came close to the code in the book.  I may
come back and add a regex to allow for the grammar to be written with an arrow.

Third Version
-------------

This version has a rewrite of the simple grammar and the bigger grammar was
added. I had to give more thought to the data structure. Trying to represent
the grammar as nested list breaks down quickly in a static language.  You run
into a situation that requires the dictionary to have a value of `List<String>`
or `List<List<string>>`.  We can get around this by treating each node as
having an optional value, which is used for terminals and non-terminals, a list
of siblings, which must be followed in order, or a list of children, which are
taken at random.

As is usually the case, once the correct data structure is in place, the rest
of the code can be simplified.

We can switch the grammar out which is one of the goals of being data driven.
The grammars are not the easiest thing to read or write though.


Chapter 2
=========

First Version
-------------
For the first example I added an extension method for OneOf and a singleton for the RNG.  The singleton keeps us from having to pass the object around and it only gets seeded once.

Things to note, new List...Concat...ToList is a lot longer than append.  I thought about adding another extension method but didn't want to spend the whole project trying to cover warts.

Another important point is we are even further from the Kleene star notation then the lisp example.  It is definitely harder to read. 

Second Version
--------------
We have a few different options when it comes to parsing the grammar:
- Regex
- Lexer 
- Dict with a string for the left side and a object that can represent the list of words or rule list on the right.

I went with the last choice and it came close to the code in the book.  I may
come back and add a regex to allow for the grammar to be written with an arrow.

Third Version
-------------

Add the extended grammar.  I had to give more thought to the data structure.  This allowed for the generate function to be written as concise as the version in the book.

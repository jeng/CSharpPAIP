Chapter 5
=========

Eliza (Pattern Matching)
------------------------

> - Read an input
> - find a pattern that matches the input 
> - transform into a response
> - Print the response

C# doesn't have a read specific for list.  I'm going to use a simple regex to parse the input.

The patterns will change slightly from 

  Pattern: (i need a X)
  Response: (what would it mean to you if you got a X ?)

to regular expressions

  Pattern: "i need a (\w+)"
  Response: "what would it mean to you if you got a \1?"


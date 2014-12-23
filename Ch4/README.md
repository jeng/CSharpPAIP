Chapter 4
=========

First Version
-------------

Linq has a match for the functions listed in figure 4.1 pg 114:

|Lisp          |C#         |
|--------------|-----------|
|find-all      |Where      |
|set-difference|Except     |
|union         |Union      |
|member        |Any        |
|every         |All        |
|some          |Any        |
|subsetp       |!Except.Any|

`Any` can be used for both `member` and `some` by wrapping the element in a
predicate when `member` is needed.

This program ported over nicely.  C# makes it more difficult to build up static
data in the programs (code as data).  In today's environments people just
deserialize json or xml.  Instead of taking a similar route, since I wanted to
stay close to the book, I added a wrapper to make list creation less verbose.


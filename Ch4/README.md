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

4.10 Debugging output
---------------------

I didn't implement the debugging output since the runtime environment provides
tracing through `System.Diagnostic`.  With the dmcs compiler, this can be
enabled by passing `-d:TRACE` as a parameter.  The make file has been updated
to use CFLAGS.  This means tracing can be turn on or off during the build
processes.

    make CFLAGS='-d:TRACE'

That will turn on the tracer.  It is off by default.

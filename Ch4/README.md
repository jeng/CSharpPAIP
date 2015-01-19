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
|remove-if     |RemoveAll  |
|append        |AddRange   |
|mappend       |SelectMany |

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

Version 2  "A More General Problem Solver"
------------------------------------------

With lisp's dynamic type system the executing state can be quickly added by
using a nested structure and then checking for a cons or an atom.  Norvig
changes this a little later in the chapter by explicitly checking for a
starting or executing action.  To paraphrase "Saying what's convenient instead
of what's really happening will lead to trouble".

In this version a new class is added for each action.  The action type will
consist of the action name and a enumerated type for its state.  The action
state could be a string, allowing for more flexibility when moving to new
domains, such as the maze domain. The enum is less error prone and can be
checked by the compiler.  When moving to a new domain it will just need to be
extended.

In the above table I have `some` and `Any` as equivalent but this isn't true
when the predicate for `some` returns a none boolean value. `some` returns the
first none nil result: 

    CL-USER> (some #'(lambda (x) (cons x '(a b c))) '(1 2 3 4 5))
    (1 A B C)
    
    CL-USER> (some #'(lambda (x) x) '(nil 2 nil 4 5))
    2

Exercises
---------

I added three versions of the permutation program: recursive, iterative and one in C.  The running time for the two C# programs is similar. Also, both of them fail at 11 items.  Not surprising since they are trying to store 39,916,800 objects in memory. 

    % time ./StackPermute.exe one two three four five six seven eight nine ten > /dev/null
    67.07s user 7.95s system 99% cpu 1:15.24 total

    % time ./Permute.exe one two three four five six seven eight nine ten > /dev/null
    53.35s user 8.26s system 99% cpu 1:01.82 total

    % time ./permute one two three four five six seven eight nine ten > /dev/null 
    15.21s user 0.01s system 99% cpu 15.242 total

The C version has some optimizations that could be added to the C# versions.  It outputs the result when found, instead of storing all of them and returning.  It also permutes the indexes of a look up table.  This keeps it from having multiple copies of the objects in memory.

    % time ./permute one two three four five six seven eight nine ten eleven > /dev/null
    189.46s user 0.10s system 99% cpu 3:09.82 total


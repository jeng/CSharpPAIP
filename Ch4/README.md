Chapter 4
=========

First Version
-------------

Linq has a match for the functions listed in figure 4.1 pg 114:

|Lisp          |C#    |
|--------------|------|
|find-all      |Where |
|set-difference|Except|
|union         |Union |
|member        |Any   |
|every         |All   |
|some          |Any   |

`Any` can be used for both `member` and `some` by wrapping the element in a
predicate when `member` is needed.

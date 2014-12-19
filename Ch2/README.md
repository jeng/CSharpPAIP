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

Fourth Version
--------------

Specifying the grammar was ugly, that was bothering me.  I didn't want to get
into parsing s-expressions the early but I did anyway.  A recursive decent
parser was added so the grammar can be specified in a format similar to the
book.

    sentence     => (noun-phrase verb-phrase);
    noun-phrase  => (Article Adj* Noun PP*) (Name) (Pronoun);
    verb-phrase  => (Verb noun-phrase PP*);
    PP*          => () (PP PP*);
    Adj*         => () (Adj Adj*);
    PP           => (Prep noun-phrase);
    Prep         => to in by with on;
    Adj          => big little blue green adiabatic;
    Article      => the a;
    Name         => Pat Kim Lee Terry Robin;
    Noun         => man ball woman table;
    Verb         => hit took saw liked;
    Pronoun      => he she it these those that;";

This the EBNF I sketched out:

    rule       = key,[space],'=>',[space],phrase,endl
    phrase     = token_list|sexp_list
    space      = '\s\t\n\r'
    endl       = ';'
    sexp_list  = sexp,[space],{sexp}
    sexp       = '(' token_list ')'
    token_list = token,[space],{token}
    token      = alphanum|special, {alphnum|special}
    alphanum   = a-z|A-Z|0-9
    special    = '*'|'_'|'-'
    key        = token


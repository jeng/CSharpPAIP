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

Issues with the regex version
-----------------------------
Using regular expressions to match the whole sentence creates a few issues.  If we require a space around the patterns, for example `(.*) no (.*)`, we end up missing matches and the conversation is boring.  If we don't have the space, partial matches trigger replacements, think `nowhere`.  We can write a more complex expression `^no$|^(.*) no$|^(.*) no (.*)$` but at that point it feels like the wrong tool for the job.  

Also, using `\2` as the replacement, and then randomly choosing a replacement string, causes replies to show up with an incomplete sentence sometimes.  It should only pull replacement sentences when the match has a value.

I'm going to implement the books pattern matcher for comparison.

It still has funny output sometimes:

    I think that I love you eliza
    Perhaps in your fantasy we love each other
    Is it wrong to love a computer
    Do computers worry you?

Bonus Program!
--------------
SEXP's with more Atoms!  I added a sexpression parser that considers every item or a list an atom. It was used to parse the definition list from [Dr. Norvig's](http://norvig.com/paip/eliza.lisp) site into the C# definitions.

This:

    (((?* ?x) I remember (?* ?y)) 
      (Do you often think of ?y)
      (Does thinking of ?y bring anything else to mind?)
      (What else do you remember) (Why do you recall ?y right now?)
      (What in the present situation reminds you of ?y)
      (What is the connection between me and ?y))

Becomes:

    new LookupPattern(
      @"(.*)I remember(.*)",
      @"Do you often think of \2",
      @"Does thinking of \2 bring anything else to mind?",
      @"What else do you remember",
      @"Why do you recall \2 right now?",
      @"What in the present situation reminds you of \2",
      @"What is the connection between me and \2"),

It performs this conversion for every element of `*eliza-rules*`.

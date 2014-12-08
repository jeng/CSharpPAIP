// Example program chapter 2 of PAIP by Peter Norvig.
//
// Ported to C# by Jeremy English Sun Dec  7 17:56:43 CST 2014
//
// Sentence => Noun-Phrase + Verb-Phrase
// Verb-Phrase => Verb + Noun-Phrase
// Article => the, a...
// Noun => man, ball, woman, table...
// Verb => hit, took, saw, liked...
// Noun-Pharse => Article + Adj* + Noun + PP*
// Adj* => 0, Adj + Adj*
// PP* => 0, PP + PP*
// PP => Prep + Noun-Phrase
// Adj => big, little, blue, green...
// Prep => to, in, by, with...
//
using System.Linq;
using System;
using System.Collections.Generic;

public static class RandomSentenceOne
{
    public static void Main(string [] args)
    {
        Console.WriteLine(String.Join(" ", new SimpleGrammar().Sentence()));
    }
}

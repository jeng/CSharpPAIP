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
    public static string Article()
    {
        return new List<string>(){"the", "a"}.OneOf();
    }

    public static string Noun()
    {
        return new List<string>(){"man", "ball", "woman", "table"}.OneOf();
    }

    public static string Verb()
    {
        return new List<string>(){"hit", "took", "saw", "liked"}.OneOf();
    }

    public static string Adj()
    {
        return new List<string>(){"big", "little", "blue", "green", "adiabatic"}.OneOf();
    }

    public static string Prep()
    {
        return new List<string>(){"to", "in", "by", "with", "on"}.OneOf();
    }

    public static List<string> NounPharse()
    {
        return new List<string>(){Article()}
                    .Concat(AdjStar())
                    .Concat(new List<string>(){Noun()})
                    .Concat(PPStar())
                    .ToList();
    }

    public static List<string> VerbPharse()
    {
        return new List<string>(){Verb()}.Concat(NounPharse()).ToList();
    }
    
    public static List<string> AdjStar()
    {
        var rng = RandomSingleton.GetInstance();
        if (rng.Random.Next(2) == 0)
            return new List<string>();
        else
            return new List<string>(){Adj()}.Concat(AdjStar()).ToList();
    }

    public static List<string> PP()
    {
        return new List<string>() {Prep()}.Concat(NounPharse()).ToList();                
    }

    public static List<string> PPStar()
    {
        var rng = RandomSingleton.GetInstance();
        if (rng.Random.Next(2) == 0)
            return new List<string>();
        else
            return PP().Concat(PPStar()).ToList();
    }

    public static List<string> Sentence()
    {
        return NounPharse().Concat(VerbPharse()).ToList();
    }

    public static void Main(string [] args)
    {
        Console.WriteLine(String.Join(" ", Sentence()));
    }
}

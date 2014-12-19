// Example program chapter 2.3 of PAIP by Peter Norvig.
//
// Ported to C# by Jeremy English Sun Dec  7 21:34:16 CST 2014

using System.Linq;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public static class RandomSentenceGrammar
{
    public static readonly string SimpleGrammar = @"
        sentence     => (noun-phrase verb-phrase);
        noun-phrase  => (Article Noun);
        verb-phrase  => (Verb noun-phrase);
        Article      => the a;
        Noun         => man ball woman table;
        Verb         => hit took saw liked;";


    public static readonly string BiggerGrammar = @"
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

    public static Dictionary<string, Rule> Grammar;

    public static void Print(object o)
    {
        Console.WriteLine(o);
    }

   public static Rule Rewrite(Rule key, Dictionary<string, Rule> grammar)
    {
        if(grammar.ContainsKey(key.Value))
            return grammar[key.Value];
        else
            return key;
    }

    public static List<string> Generate(Rule phrase) 
    {
        //Print(phrase); 
        if (phrase.Children != null)
            return Generate(Rewrite(phrase.Children.OneOf(), Grammar));
        else if (phrase.Sibling != null)
            return phrase.Sibling
                .Select(x => Generate(Rewrite(x, Grammar)))
                .SelectMany(x => x).ToList();
        else
            return new List<string>(){phrase.Value};
   }

    public static List<string> Generate(string phrase)
    {
        return Generate(Rewrite(new Rule(){Value=phrase}, Grammar));
    }

    public static void Main(string [] args)
    {
        Regex re = new Regex(@"\s+");
        GrammarParser gp = new GrammarParser(BiggerGrammar);//SimpleGrammar);
        Grammar = gp.Grammar;
        Print(re.Replace(String.Join(" ", Generate("sentence")), " "));
    }
}

// Example program chapter 2.3 of PAIP by Peter Norvig.
//
// Ported to C# by Jeremy English Sun Dec  7 21:34:16 CST 2014

using System.Linq;
using System;
using System.Collections.Generic;

public class Rule
{
    private bool _IsList = false;
    private List<string> _Values;
    public List<string> Values
    {
        get { return _Values; }
    }

    public Rule(params string [] values)
    {
       _Values = values.ToList(); 
    }

    public Rule(List<string> functions)
    {
        _IsList = true;
        _Values = functions;
    }

    public bool IsList()
    {
        return _IsList;
    }
}

public static class RandomSentenceGrammar
{
    public static readonly Dictionary<string, Rule> Grammar = new Dictionary<string, Rule>()
    {
        {"sentence",    new Rule(new List<string>(){"noun-phrase", "verb-phrase"})},                
        {"noun-phrase", new Rule(new List<string>(){"Article", "Noun"})},
        {"verb-phrase", new Rule(new List<string>(){"Verb", "noun-phrase"})},
        {"Article",     new Rule("the", "a")},
        {"Noun",        new Rule("man", "ball", "woman", "table")},
        {"Verb",        new Rule("hit", "took", "saw", "liked")}
    };

    public static List<string> Generate(Rule phrase)
    {
        var first = phrase.Values.First();
        if (phrase.IsList())
            return phrase.Values.Select(x => Generate(x)).SelectMany(x => x).ToList();
        else if (Grammar.ContainsKey(first))
        {
            var rule = Grammar[first];
            if (rule.IsList())
                return Generate(rule);
            else
                return Generate(rule.Values.OneOf());
        }
        else
            return new List<string>(){first};
    }

    public static List<string> Generate(string phrase)
    {
        return Generate(new Rule(phrase));
    }

    public static void Main(string [] args)
    {
        for (int i = 0; i < 100; i++)
            Console.WriteLine(String.Join(" ", Generate("sentence")));
    }
}

// Example program chapter 2.3 of PAIP by Peter Norvig.
//
// Ported to C# by Jeremy English Sun Dec  7 21:34:16 CST 2014

using System.Linq;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Rule
{
    public List<Rule> Children { get; set; }
    public List<Rule> Sibling { get; set; }
    public string Value { get; set;}
    public override string ToString()
    {
        string siblings = (Sibling == null)  ? "" : String.Join(" ", Sibling.Select(x => x.Value));
        string children = (Children == null) ? "" : String.Join(" ", Children.Select(x => x.Value));
        return String.Format("Name: {0}\nSiblings: {1}\nChildren: {2}\n", 
                Value, siblings, children);
    }
    public Rule()
    {
        this.Value = "";
    }
}


public static class RandomSentenceGrammar
{
    public static readonly Dictionary<string, Rule> SimpleGrammar = new Dictionary<string, Rule>()
    {
        {"sentence",    new Rule(){Sibling=RuleList("noun-phrase", "verb-phrase")}},
        {"noun-phrase", new Rule(){Sibling=RuleList("Article", "Noun")}},
        {"verb-phrase", new Rule(){Sibling=RuleList("Verb", "noun-phrase")}},
        {"Article",     new Rule(){Children=RuleList("the", "a")}},
        {"Noun",        new Rule(){Children=RuleList("man", "ball", "woman", "table")}},
        {"Verb",        new Rule(){Children=RuleList("hit", "took", "saw", "liked")}}
    };

    public static readonly Dictionary<string, Rule> BiggerGrammar = new Dictionary<string, Rule>()
    {
        {"sentence",    new Rule(){Sibling=RuleList("noun-phrase", "verb-phrase")}},

        {"noun-phrase", new Rule(){
                          Children=
                              new List<Rule>(){
                                  new Rule(){
                                      Sibling=RuleList("Article", "Adj*", "Noun", "PP*")},
                                  new Rule(){
                                      Sibling=RuleList("Name")},
                                  new Rule(){
                                      Sibling=RuleList("Pronoun")}}}},

        {"verb-phrase", new Rule(){Sibling=RuleList("Verb", "noun-phrase", "PP*")}},

        {"PP*" , new Rule(){
                    Children=
                        new List<Rule>(){
                            new Rule(),//e empty rule
                            new Rule(){Sibling=RuleList("PP", "PP*")}}}},

        {"Adj*" , new Rule(){
                    Children=
                        new List<Rule>(){
                            new Rule(),//e empty rule
                            new Rule(){Sibling=RuleList("Adj", "Adj*")}}}},

        {"PP", new Rule(){Sibling=RuleList("Prep", "noun-phrase")}},

        {"Prep", new Rule(){Children=RuleList("to", "in", "by", "with", "on")}},

        {"Adj", new Rule(){Children=RuleList("big", "little", "blue", "green", "adiabatic")}},

        {"Article", new Rule(){Children=RuleList("the", "a")}},

        {"Name", new Rule(){Children=RuleList("Pat", "Kim", "Lee", "Terry", "Robin")}},

        {"Noun", new Rule(){Children=RuleList("man", "ball", "woman", "table")}},

        {"Verb", new Rule(){Children=RuleList("hit", "took", "saw", "liked")}},

        {"Pronoun", new Rule(){Children=RuleList("he", "she", "it", "these", "those", "that")}}
    };


    public static readonly Dictionary<string, Rule> Grammar = BiggerGrammar;//SimpleGrammar;

    public static void Print(object o)
    {
        Console.WriteLine(o);
    }

    public static List<Rule> RuleList(params string [] tokens)
    {
        List<Rule> result = new List<Rule>();
        foreach(var s in tokens)
        {
            result.Add(new Rule(){Value=s});
        }
        return result;
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
        Print(re.Replace(String.Join(" ", Generate("sentence")), " "));
    }
}

// Example program chapter 5.1 of PAIP by Peter Norvig.
//
// This version of Eliza uses pattern matching, instead of regular expressions,
// similar to the version in the book.
//
// Ported to C# by Jeremy English Sat Oct  3 23:43:14 CDT 2015

using System;
using io = SimpleIO.SimpleIO;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
//using SentenceParser;

public static class ElizaPM
{

    public static bool SimpleEqual(StringCollection x, StringCollection y)
    {
        int xLen = x.Count;
        int yLen = y.Count;
        if (xLen != yLen)
            return false;
        for(int i = 0; i < xLen; i++)
        {
            if (string.Compare(x[i], y[i], true) != 0)
                return false;
        }
        return true;
    }

    public enum PatMatchResult {success, fail, nobindings};
    public class PatMatchLookup
    {
        private PatMatchResult result;
        public PatMatchResult Result { get{return result;} }
        private Dictionary<string, string> table { get; set; }

        public bool ExtendBindings(string key, string value)
        {
            if (table.ContainsKey(key))
            {
                if (table[key] != value)
                    return false;
            }
            else
                table[key] = value;

            return true;
        }
       
        public string GetBinding(string key)
        {
            return table[key];
        }

        public bool HasBinding(string key)
        {
            return table.ContainsKey(key);            
        }

        public PatMatchLookup()
        {
            table = new Dictionary<string, string>();
            result = PatMatchResult.fail;
        }

        public void markSuccess()
        {
            result = table.Count > 0 ? PatMatchResult.success : PatMatchResult.nobindings;
        }

        public override string ToString()
        {
            string s = "";
            foreach(var key in table.Keys)
            {
                s += String.Format("{0}: {1}\n", key, table[key]);
            }
            s += String.Format("Result: {0}\n", result);
            return s;
        }
    }

    //Match pattern agains input in the context of bindings
    public static PatMatchLookup PatMatch(StringCollection pattern, StringCollection input)
    {
        var lookup = new PatMatchLookup();

        if (pattern.Count != input.Count)
            return lookup;

        for(int i = 0; i < pattern.Count; i++)
        {
            if (IsVariable(pattern[i]))
            {
                //if binding doesn't match return failure
                if (!lookup.ExtendBindings(pattern[i], input[i]))
                    return lookup;
            }
            else if (string.Compare(pattern[i], input[i], true) != 0)
                return lookup;
        }
        lookup.markSuccess();
        return lookup;
    }

    public static StringCollection Substitute(PatMatchLookup lookup, StringCollection sc)
    {
        var result = new StringCollection();
        foreach(var word in sc)
        {
            if (IsVariable(word) && lookup.HasBinding(word))
                result.Add(lookup.GetBinding(word));
            else
                result.Add(word);
        }
        return result;
    }
    
    //Is x a variable (a string beginning with ?)
    public static bool IsVariable(string x)
    {
        return x.Length > 1 && x[0] == '?';
    }

    public static string Response(StringCollection sc)
    {
        return String.Join(" ", sc.Cast<string>().ToArray());
    }

    public static int Main(string [] args)
    {
       //io.Reader(ProcessLine);
       var inputParser = new SentenceParser();
       io.Print("Match: {0}", SimpleEqual(inputParser.Parse("one two three"),
                                          inputParser.Parse("one  two,   thReE")));
       io.Print("Match: {0}", SimpleEqual(inputParser.Parse("1 two three"),
                                          inputParser.Parse("one  two,   thReE")));
       io.Print("Match: {0}", SimpleEqual(inputParser.Parse("one two three"),
                                          inputParser.Parse("two thReE")));

       io.Print("IsVariable {0}", IsVariable("?"));
       io.Print("IsVariable {0}", IsVariable("?x"));
       io.Print("IsVariable {0}", IsVariable("?y"));

       io.Print("PatMatch: {0}", PatMatch(inputParser.Parse("One ?x three"),
                                          inputParser.Parse("One twO tHREE")));
       io.Print("PatMatch: {0}", PatMatch(inputParser.Parse("One ?x ?y"),
                                          inputParser.Parse("One twO tHREE")));
       io.Print("PatMatch: {0}", PatMatch(inputParser.Parse("One ?x bark"),
                                          inputParser.Parse("One twO tHREE")));
       io.Print("PatMatch: {0}", PatMatch(inputParser.Parse("One ?x ?x"),
                                          inputParser.Parse("One twO two")));
       io.Print("PatMatch: {0}", PatMatch(inputParser.Parse("One ?x ?x"),
                                          inputParser.Parse("One twO tHREE")));
       io.Print("PatMatch: {0}", PatMatch(inputParser.Parse("One two three"),
                                          inputParser.Parse("One two tHREE")));

       io.Print(Response(Substitute(
                            PatMatch(inputParser.Parse("I need a ?x"),
                                     inputParser.Parse("I need a vacation")),
                            inputParser.Parse("What would it mean if you got a vacation ?"))));

       io.Print("== Parser Test ==");
       var sp = new SentenceParser();
       var res = sp.Parse("this is a (?* ?x) words un-words, year;; " + 
                          "\"this is \"foo\" in quotes\"( foo (xs 45) bar)?x"+
                          "...Stuff after the right paren");
       foreach(var x in res)
           io.Print(x);
                                    
       return 1;
    }
}

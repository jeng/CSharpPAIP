// Example program chapter 5.1 of PAIP by Peter Norvig.
//
// Ported to C# by Jeremy English Mon May 25 19:00:00 CDT 2015

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using io = SimpleIO.SimpleIO;

public class LookupPattern
{
    public Regex Pattern { get; set; }
    public string [] Response { get; set; }
    public LookupPattern(string format, params string [] responses)
    {
        this.Pattern = new Regex(format, RegexOptions.IgnoreCase);
        this.Response = responses;
    }
}

public static class Eliza
{
    private static Regex wordRegex = new Regex(@"([a-zA-Z0-9'-]+)");
    private static Random rng = new Random();

    public static List<string> GetWords(string s)
    {
        var match = wordRegex.Matches(s);
        var words = new List<String>();
        foreach(Match m in match)
            words.Add(m.Groups[1].Value);
        return words;
    }

    public static string SwitchViewPoint(string s)
    {
        var words = GetWords(s);
        var lookup = new Dictionary<string, string>(){
            {"i", "you"}, {"you", "I"}, {"me", "you"}, {"am", "are"}}; 

        List<String> results = new List<String>();
        foreach(var w in words)
        {
            if (lookup.ContainsKey(w.ToLower()))
                results.Add(lookup[w.ToLower()]);
            else
                results.Add(w);
        }
        return String.Join(" ", results);
    }

    public static string InsertMatches(Match inputPattern, string response)
    {
        var groupNums = new Regex(@"\\([0-9]+)");
        var nums = groupNums.Matches(response);
        foreach(Match m in nums)
        {
            int idx;
            if (int.TryParse(m.Groups[1].Value, out idx))
            {
                if (idx < inputPattern.Groups.Count)
                {
                    response = response.Replace(String.Format(@"\{0}", idx), 
                            SwitchViewPoint(inputPattern.Groups[idx].Value));
                }
            }
        }
        return response;
    }


    public static string Transform(string s)
    {
        var Patterns = new LookupPattern[]{
            new LookupPattern(
                @".*hello ?(.*)",
                @"How do you do.  Please state your problem."),
            new LookupPattern(
                @".*i need a (.*)",
                @"What would it mean to you if you got a \1"),
            new LookupPattern(
                @".*i want (.*)",
                @"What would it mean to you if you got \1?",
                @"Why do you want \1?",
                @"Suppose you got \1 soon?"),
            new LookupPattern(
                @".*if (.*)",
                @"Do you really think its likely that \1",
                @"Do you wish that \1?",
                @"What do you think about \1?",
                @"Really-- if \1"),
            new LookupPattern(
                @".*no($| .*)",
                @"Why not?",
                @"You are being a bit negative",
                @"Are you saying ""NO"" just to be negative"),
            new LookupPattern(
                @".*I was (.*)",
                @"Where you really?",
                @"Perhaps I already knew you were \1.",
                @"Why do you tell me you were \1 now?"),
            new LookupPattern(
                @".*I (?:.*)feel (.*)",
                @"Do you often feel \1?"),
            new LookupPattern(
                @".*I felt (.*)",
                @"What other feelings do you have?"),
            new LookupPattern(
                @"(.*)",
                @"\1?",
                @"Interesting.",
                @"Please continue?",
                @"Can you elaborate?")
            };
        
        foreach(var p in Patterns)
        {
            if (p.Pattern.IsMatch(s))
            {
                int rIdx = rng.Next(p.Response.Length);
                return InsertMatches(p.Pattern.Match(s), p.Response[rIdx]);
            }
        }
        return s; 
    }

    public static bool ProcessLine(string s)
    {
        //clean up the input
        var words = GetWords(s);
        var cleanStr = String.Join(" ", words);
        io.Print(Transform(cleanStr));
        return true;
    }

    public static int Main(string [] args)
    {
       io.Reader(ProcessLine);
       return 1;
    }
}

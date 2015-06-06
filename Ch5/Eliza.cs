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
        var Patterns = new LookupPattern[]
        {
            new LookupPattern(
                @"(.*)hello(.*)",
                @"How do you do. Please state your problem."),
            new LookupPattern(
                @"(.*)computer(.*)",
                @"Do computers worry you?",
                @"What do you think about machines?",
                @"Why do you mention computers?",
                @"What do you think machines have to do with your problem?"),
            new LookupPattern(
                @"(.*)name(.*)",
                @"I am not interested in names"),
            new LookupPattern(
                @"(.*)sorry(.*)",
                @"Please don't apologize",
                @"Apologies are not necessary",
                @"What feelings do you have when you apologize"),
            new LookupPattern(
                @"(.*)I remember(.*)",
                @"Do you often think of \2",
                @"Does thinking of \2 bring anything else to mind?",
                @"What else do you remember",
                @"Why do you recall \2 right now?",
                @"What in the present situation reminds you of \2",
                @"What is the connection between me and \2"),
            new LookupPattern(
                @"(.*)do you remember(.*)",
                @"Did you think I would forget \2 ?",
                @"Why do you think I should recall \2 now",
                @"What about \2",
                @"You mentioned \2"),
            new LookupPattern(
                @"(.*)if(.*)",
                @"Do you really think its likely that \2",
                @"Do you wish that \2",
                @"What do you think about \2",
                @"Really-- if \2"),
            new LookupPattern(
                @"(.*)I dreamt(.*)",
                @"Really-- \2",
                @"Have you ever fantasized \2 while you were awake?",
                @"Have you dreamt \2 before?"),
            new LookupPattern(
                @"(.*)dream about(.*)",
                @"How do you feel about \2 in reality?"),
            new LookupPattern(
                @"(.*)dream(.*)",
                @"What does this dream suggest to you?",
                @"Do you dream often?",
                @"What persons appear in your dreams?",
                @"Don't you believe that dream has to do with your problem?"),
            new LookupPattern(
                @"(.*)my mother(.*)",
                @"Who else in your family \2",
                @"Tell me more about your family"),
            new LookupPattern(
                @"(.*)my father(.*)",
                @"Your father",
                @"Does he influence you strongly?",
                @"What else comes to mind when you think of your father?"),
            new LookupPattern(
                @"(.*)I want(.*)",
                @"What would it mean if you got \2",
                @"Why do you want \2",
                @"Suppose you got \2 soon"),
            new LookupPattern(
                @"(.*)I am glad(.*)",
                @"How have I helped you to be \2",
                @"What makes you happy just now",
                @"Can you explain why you are suddenly \2"),
            new LookupPattern(
                @"(.*)I am sad(.*)",
                @"I am sorry to hear you are depressed",
                @"I'm sure its not pleasant to be sad"),
            new LookupPattern(
                @"(.*)are like(.*)",
                @"What resemblance do you see between \1 and \2"),
            new LookupPattern(
                @"(.*)is like(.*)",
                @"In what way is it that \1 is like \2",
                @"What resemblance do you see?",
                @"Could there really be some connection?",
                @"How?"),
            new LookupPattern(
                @"(.*)alike(.*)",
                @"In what way?",
                @"What similarities are there?"),
            new LookupPattern(
                @"(.*)same(.*)",
                @"What other connections do you see?"),
            new LookupPattern(
                @"(.*)I was(.*)",
                @"Were you really?",
                @"Perhaps I already knew you were \2",
                @"Why do you tell me you were \2 now?"),
            new LookupPattern(
                @"(.*)was I(.*)",
                @"What if you were \2 ?",
                @"Do you thin you were \2",
                @"What would it mean if you were \2"),
            new LookupPattern(
                @"(.*)I am(.*)",
                @"In what way are you \2",
                @"Do you want to be \2 ?"),
            new LookupPattern(
                @"(.*)am I(.*)",
                @"Do you believe you are \2",
                @"Would you want to be \2",
                @"You wish I would tell you you are \2",
                @"What would it mean if you were \2"),
            new LookupPattern(
                @"(.*)am(.*)",
                @"Why do you say ""AM?""",
                @"I don't understand that"),
            new LookupPattern(
                @"(.*)are you(.*)",
                @"Why are you interested in whether I am \2 or not?",
                @"Would you prefer if I weren't \2",
                @"Perhaps I am \2 in your fantasies"),
            new LookupPattern(
                @"(.*)you are(.*)",
                @"What makes you think I am \2 ?"),
            new LookupPattern(
                @"(.*)because(.*)",
                @"Is that the real reason?",
                @"What other reasons might there be?",
                @"Does that reason seem to explain anything else?"),
            new LookupPattern(
                @"(.*)were you(.*)",
                @"Perhaps I was \2",
                @"What do you think?",
                @"What if I had been \2"),
            new LookupPattern(
                @"(.*)I can't(.*)",
                @"Maybe you could \2 now",
                @"What if you could \2 ?"),
            new LookupPattern(
                @"(.*)I feel(.*)",
                @"Do you often feel \2 ?"),
            new LookupPattern(
                @"(.*)I felt(.*)",
                @"What other feelings do you have?"),
            new LookupPattern(
                @"(.*)I(.*)you(.*)",
                @"Perhaps in your fantasy we \2 each other"),
            new LookupPattern(
                @"(.*)why don't you(.*)",
                @"Should you \2 yourself?",
                @"Do you believe I don't \2",
                @"Perhaps I will \2 in good time"),
            new LookupPattern(
                @"(.*)yes(.*)",
                @"You seem quite positive",
                @"You are sure",
                @"I understand"),
            new LookupPattern(
                @"(.*)no(.*)",
                @"Why not?",
                @"You are being a bit negative",
                @"Are you saying ""NO"" just to be negative?"),
            new LookupPattern(
                @"(.*)someone(.*)",
                @"Can you be more specific?"),
            new LookupPattern(
                @"(.*)everyone(.*)",
                @"surely not everyone",
                @"Can you think of anyone in particular?",
                @"Who for example?",
                @"You are thinking of a special person"),
            new LookupPattern(
                @"(.*)always(.*)",
                @"Can you think of a specific example",
                @"When?",
                @"What incident are you thinking of?",
                @"Really-- always"),
            new LookupPattern(
                @"(.*)what(.*)",
                @"Why do you ask?",
                @"Does that question interest you?",
                @"What is it you really want to know?",
                @"What do you think?",
                @"What comes to your mind when you ask that?"),
            new LookupPattern(
                @"(.*)perhaps(.*)",
                @"You do not seem quite certain"),
            new LookupPattern(
                @"(.*)are(.*)",
                @"Did you think they might not be \2",
                @"Possibly they are \2"),
            new LookupPattern(
                @"(.*)",
                @"Very interesting",
                @"I am not sure I understand you fully",
                @"What does that suggest to you?",
                @"Please continue",
                @"Go on",
                @"Do you feel strongly about discussing such things?"),
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

//Parse words and punctuation into separate tokens
//words between " and ( will be treated as a single token
//also tokens can be a variable place holder ?x, ?a, ?* etc.
//
// jhe@jeremyenglish.org Sun Oct  4 13:34:47 CDT 2015
using System;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

public class SentenceParser
{
    
    public StringCollection Parse(string input)
    {
        bool inQuote = false;
        int inParen = 0;
        var pattern = @"([a-z0-9\-_]+|\(|\)|""|\?[a-z,\*])";
        var result = new StringCollection();
        var token = "";
        foreach(Match match in Regex.Matches(input, pattern, RegexOptions.IgnoreCase))
        {
            var symbol = match.Groups[1].Value;
            if (symbol == "(")
            {
                inParen++;
                token = token + "(";
            }
            else if(symbol == "\"")
            {
                token = token.Trim() + "\"";
                inQuote = !inQuote;
                if (!inQuote)
                {
                    result.Add(token.Trim());
                    token = ""; 
                }
            }
            else if(symbol == ")")
            {
                token = token.Trim() + ") ";
                inParen--;
                if (inParen == 0)
                {
                    result.Add(token.Trim());
                    token = "";
                }
            }
            else
            {
                if(inParen > 0 || inQuote)
                    token = token + symbol + " ";
                else
                    result.Add(symbol);
            }
        }
        return result;
    }
}

// Example program chapter 2.3 of PAIP by Peter Norvig.
// I didn't want to get into parsing sexp this early, oh well.

// rule       = key,[space],'=>',[space],phrase,endl
// phrase     = token_list|sexp_list
// space      = '\s\t\n\r'
// endl       = ';'
// sexp_list  = sexp,[space],{sexp}
// sexp       = '(' token_list ')'
// token_list = token,[space],{token}
// token      = alphanum|special, {alphnum|special}
// alphanum   = a-z|A-Z|0-9
// special    = '*'|'_'|'-'
// key        = token

// TODO: Add a sentence type.  This will allow us to have multiple word tokens.
// Useful for things like noun, name "Jeremy English"
//
// Doesn't handle nested s-expressions.  We should never need to in this case.

using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;


public class GrammarParser
{
    private string data;
    private Regex reSpace;
    private Regex reToken;
    private Regex reRparen;
    private Regex reLparen;
    private Regex reSemi;
    private Regex reArrow;
    private enum ListType{FLAT_LIST, SEXP_LIST};
    public Dictionary<string, Rule> Grammar { get; private set;}
    public GrammarParser(string GrammarData)
    {
        data = GrammarData;

        reSpace  = new Regex(@"^(\s|\t|\n|\r|\r\n)+");
        reToken  = new Regex(@"^((?:[a-z]|[A-Z]|[0-9]|[-_*])+)");
        reRparen = new Regex(@"^\)");
        reLparen = new Regex(@"^\(");
        reSemi   = new Regex(@"^;");
        reArrow  = new Regex(@"^=>");

        Grammar = new Dictionary<string, Rule>();

        parse();
    }

    private void removeSpace()
    {
        data = reSpace.Replace(data, "", 1);
    }

    private string pop(Regex re) 
    {
        removeSpace();
        var m = re.Match(data);
        if (m.Success)
        {
            data = re.Replace(data, "", 1);
            return m.Value;
        }
        else
        {
            throw new Exception("Could not pop value at: " + 
                        data.Substring(0, Math.Min(10, data.Length)));
        }
    }

    private string token()
    {
        return pop(reToken);
    }

    private bool topIs(Regex re)
    {
        removeSpace();
        return re.IsMatch(data);
    }

    private List<string> sexp()
    {
        pop(reLparen);
        var tl = tokenList();
        pop(reRparen);
        return tl;
    }

    private List<List<string>> sexpList()
    {
        var sexpLs = new List<List<string>>();
        while(!topIs(reSemi) && data.Length > 0)
            sexpLs.Add(sexp());
        return sexpLs;
    }

    private List<string> tokenList()
    {
        var sl = new List<string>();
        while(!topIs(reSemi) && !topIs(reRparen) && data.Length > 0)
            sl.Add(token());
        return sl;
    }

    //Use tuple as a substitute for multiple value bind
    private Tuple<ListType, List<List<string>>> phrase()
    {
        if (topIs(reLparen))
            return new Tuple<ListType, List<List<string>>>
                (ListType.SEXP_LIST, sexpList());
        else
            return new Tuple<ListType, List<List<string>>> 
                (ListType.FLAT_LIST,new List<List<string>>(){tokenList()});
    }

    public static List<Rule> RuleList(List<string> tokens)
    {
        List<Rule> result = new List<Rule>();
        foreach(var s in tokens)
            result.Add(new Rule(){Value=s});
        return result;
    }

    private void parse()
    {
        while(data.Length > 0)
        {
            string key = token();

            pop(reArrow);
            var parsedValue = phrase();
            pop(reSemi);

            if (parsedValue.Item1 == ListType.FLAT_LIST)
            {
                Grammar[key] = new Rule(){ Children=RuleList(parsedValue.Item2[0]) };
            }
            else
            {
                //Do we have more than one sexp
                if(parsedValue.Item2.Count > 1)
                {
                    var rule = new Rule(){Children = new List<Rule>()};
                    Grammar[key] = rule; 
                    foreach(var sexp in parsedValue.Item2)
                    {
                        if (sexp.Count == 0)
                            rule.Children.Add(new Rule()); //empty rule
                        else
                            rule.Children.Add(new Rule(){Sibling=RuleList(sexp)});
                    }
                }
                else
                    Grammar[key] = new Rule(){ Sibling=RuleList(parsedValue.Item2[0]) };
            }
        }
    }
}

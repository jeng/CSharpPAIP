// Jeremy English jhe@jeremyenglish.org Sat Jun  6 08:24:41 CDT 2015
//
// Parse the sexp definitions and print them in our format
//
// This:
//  (((?* ?x) I remember (?* ?y)) 
//    (Do you often think of ?y)
//    (Does thinking of ?y bring anything else to mind?)
//    (What else do you remember) (Why do you recall ?y right now?)
//    (What in the present situation reminds you of ?y)
//    (What is the connection between me and ?y))
//
// Well become:
//   new LookupPattern(
//    @"(.*) I remember (.*)",
//    @"Do you often think of \2",
//    @"Does thinking of \2 bring anything else to mind?",
//    @"What else do you remember",
//    @"Why do you recall \2 right now?",
//    @"What in the present situation reminds you of \2",
//    @"What is the connection between me and \2"),
 
using System;
using System.Collections.Generic;
using SExp;
using System.IO;

public static class ConvertElizaDef
{
    public static void PPrint(List<Cell> cl, string ident)
    {
        foreach(var c in cl)
        {
            if (c.Type == SExp.CellType.atom)
                output("{0}{1}", ident,c.Atom);
            else
                PPrint(c.List, ident+"    ");
        }
    }

    public static void error(string s, params object [] args)
    {
        throw new Exception(String.Format(s, args));
    }

    public static void output(string s, params object [] args)
    {
        Console.WriteLine(String.Format(s, args));
    }

    public static List<string> GetReplacements(List<Cell> ruleList, Dictionary<string, int> lookupTable)
    {
        var result = new List<string>();
        //The first element the matching pattern move past that
        for(int i = 1; i < ruleList.Count; i++)
        {
            var replacement = "";
            if (ruleList[i].Type == CellType.list)
            {
                foreach(var atom in ruleList[i].List)
                {
                    if (atom.Type == CellType.atom)
                    {
                        var a = atom.Atom;
                        if (lookupTable.ContainsKey(a))
                            a = "\\" + lookupTable[a];
                        replacement += a;
                        replacement += " ";
                    }
                }
            }
            result.Add(replacement.Trim());
        }
        return result;
    }

    public static void ProcessRules(List<Cell> ruleList)
    {
        //First list is the pattern to look for
        if (ruleList.Count > 0 && ruleList[0].Type == CellType.list)
        {
            //Will need a lookup table to keep track of the variables
            //Collect the search pattern atoms
            string match = "";
            int n = 1;
            Dictionary<string, int> LookupTable = new Dictionary<string, int>();
            foreach(var a in ruleList[0].List)
            {
                if (a.Type == CellType.atom)
                {
                    match += a.Atom;
                    match += " ";
                }
                else
                {
                    match = match.Trim();
                    match += "(.*)";
                    var variable = a.List;                    
                    if (variable.Count == 2 && variable[0].Atom == "?*")
                        LookupTable[variable[1].Atom] = n++;
                    else
                        error("Invalid variable list {0}", variable);
                }
            }
            match = match.Trim();
            var replacements = GetReplacements(ruleList, LookupTable);            
            PrintDef(match, replacements);
        }
        else
        {
            error("Invalid rule list {0}", ruleList);
        }
    }

    public static void PrintDef(string match, List<string> replacements)
    {
        var format = "    @\"{0}\",\n";
        var def = "new LookupPattern(\n";
        def += string.Format(format, match);
        foreach(var r in replacements)
        {
            def += string.Format(format, r.Replace("\"", "\"\""));
        }
        def += string.Format("),");
        output(def.Replace(",\n)", ")"));
    }

    public static void Main(string [] argv)
    {
        StreamReader streamReader = new StreamReader("eliza-definitions.sexp");
        string filedata = streamReader.ReadToEnd();
        streamReader.Close();
        SExp.SexpParser sexp = new SExp.SexpParser();
        var res = sexp.Parse(filedata);
        //PPrint(res, "");
        if (res.Count < 3)
            error("Empty definition list {0}", res.Count);

        if (res[0].Type != CellType.atom && 
                res[0].Atom != "defparameter" &&
                res[1].Type != CellType.atom &&
                res[1].Atom != "*eliza-rules*" &&
                res[2].Type != CellType.list)
            error("Invalid Definition List: {0} {1} {2}", res[0].Type, res[1].Type, res[2].Type);

        foreach(var rule in res[2].List)
        {
            ProcessRules(rule.List);            
        }
    }
}

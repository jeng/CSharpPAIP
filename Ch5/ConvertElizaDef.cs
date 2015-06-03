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
                Console.WriteLine(ident + c.Atom);
            else
            {
                Console.WriteLine("List start");
                PPrint(c.List, ident+"    ");
            }
        }
        Console.WriteLine("List end");
    }

    public static void Main(string [] argv)
    {
        StreamReader streamReader = new StreamReader("eliza-definitions.sexp");
        string filedata = streamReader.ReadToEnd();
        streamReader.Close();
        SExp.SexpParser sexp = new SExp.SexpParser();
        var res = sexp.Parse(filedata);
        PPrint(res, "");
        System.Console.WriteLine("Hello World");
    }
}

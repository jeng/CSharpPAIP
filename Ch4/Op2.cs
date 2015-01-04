// Example program chapter 4.11 of PAIP by Peter Norvig.
//
// Ported to C# by Jeremy English Sat Jan  3 17:07:53 CST 2015

using System;
using System.Linq;
using System.Collections.Generic;

public class Op
{
    public string Action { get; set; }
    public List<OpAction> Preconds { get; set; }
    public List<OpAction> AddList { get; set; }
    public List<OpAction> DelList { get; set; }

    public override string ToString()
    {
        return String.Format("Action: {0}\nPrecond:\n{1}\nAddList:\n{2}\nDelList:\n{3}", 
                Action, String.Join("\n", Preconds), String.Join("\n", AddList), 
                String.Join("\n", DelList));
    }
}

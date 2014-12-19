// Example program chapter 4.3 of PAIP by Peter Norvig.
//
// Ported to C# by Jeremy English Fri Dec 19 05:08:57 CST 2014

using System;
using System.Linq;
using System.Collections.Generic;

public class Op
{
    public string Action { get; set; }
    public List<string> Preconds { get; set; }
    public List<string> AddList { get; set; }
    public List<string> DelList { get; set; }
}

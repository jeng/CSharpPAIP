// Example program chapter 4.12 of PAIP by Peter Norvig.
// Exercise 4.3
// Ported to C# by Jeremy English Fri Feb 20 05:24:34 CST 2015

using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

public static class GeneralProblemSolver
{
    public static void Print(Object o)
    {
        Console.WriteLine(o);
    }

    public static List<OpAction> oal(params string [] elems)
    {
        var ol = new List<OpAction>();
        foreach(var e in elems)
        {
           var oa = new OpAction(){Name = e, State = ActionState.None};
           ol.Add(oa);
        }
        return ol;
    }

    static List<Op> dessertOps = new List<Op>(){
           new Op(){
                Action   = "eat-ice-cream",
                Preconds = oal("has-ice-cream"),
                AddList  = oal("ate-dessert"),
                DelList  = oal("has-ice-cream")},
            new Op(){
                Action   = "eat-cake",
                Preconds = oal("has-cake"),
                AddList  = oal("ate-dessert", "has-ice-cream"),
                DelList  = oal("has-cake")},
            new Op(){ 
                Action   = "buy-cake",
                Preconds = oal("has-money"),
                AddList  = oal("has-cake"),
                DelList  = oal("has-money")},
   };

    private static Op ConvertOp(Op op)
    {
        if (op.DelList == null)
            op.DelList = oal();

        if (op.AddList.First().State != ActionState.Executing)
            op.AddList.Insert(0, new OpAction(){Name = op.Action, State = ActionState.Executing});

        return op;
    }

    public static void PrintSolution(List<OpAction> solution)
    {
        if (solution == null)
            Print("Not solved");
        else
            foreach(var s in solution)
                Print(s);
     }


    public static int Main(string [] args)
    {
        Trace.Listeners.Add(new ConsoleTraceListener()); 

        var gps = new GPS(dessertOps.Select(ConvertOp).ToList());
        PrintSolution(gps.Solve(
                oal("has-money"),
                oal("ate-dessert")));
        Print("");

        return 0;
    }
}

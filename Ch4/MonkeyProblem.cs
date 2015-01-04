// Example program chapter 4.12 of PAIP by Peter Norvig.
//
// Ported to C# by Jeremy English Sat Jan  3 17:05:23 CST 2015 

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

    public static void Print(string s)
    {
        Console.WriteLine(s);
    }

    //Return a new OpAction list. If executing is true, the first
    //element will have the executing state set.
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

    static List<Op> bananaOps = new List<Op>(){
            new Op(){ 
                Action   = "climb-on-chair",
                Preconds = oal("chair-at-middle-room", "at-middle-room", "on-floor"),
                AddList  = oal("at-bananas", "on-chair"),
                DelList  = oal("at-middle-room", "on-floor")},
            new Op(){ 
                Action   = "push-chair-from-door-to-middle-room",
                Preconds = oal("chair-at-door", "at-door"),
                AddList  = oal("chair-at-middle-room", "at-middle-room"),
                DelList  = oal("chair-at-door", "at-door")},
            new Op(){ 
                Action   = "walk-from-door-to-middle-room",
                Preconds = oal("at-door", "on-floor"),
                AddList  = oal("at-middle-room"),
                DelList  = oal("at-door")},
            new Op(){ 
                Action   = "grasp-bananas",
                Preconds = oal("at-bananas", "empty-handed"),
                AddList  = oal("has-bananas"),
                DelList  = oal("empty-handed")},
            new Op(){ 
                Action   = "drop-ball",
                Preconds = oal("has-ball"),
                AddList  = oal("empty-handed"),
                DelList  = oal("has-ball")},
            new Op(){ 
                Action   = "eat-bananas",
                Preconds = oal("has-bananas"),
                AddList  = oal("empty-handed", "not-hungry"),
                DelList  = oal("has-bananas", "hungry")}
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

        var gps = new GPS(bananaOps.Select(ConvertOp).ToList());
        PrintSolution(gps.Solve(
                oal("at-door", "on-floor", "has-ball", "hungry", "chair-at-door"),
                oal("not-hungry")));
        Print("");

        return 0;
    }
}

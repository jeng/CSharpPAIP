// Example program chapter 4.11 of PAIP by Peter Norvig.
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
/*
    public static List<OpAction> addList(params string [] elems)
    {
        var ol = oal(elems);
        ol[0].State = ActionState.Executing;
        return ol;
    }
*/

    static List<Op> schoolOps = new List<Op>(){
            //The not looking after you don't leap problem
            //Comment this out and turn on tracing to see the issue
            // new Op(){
            //     Action   = "taxi-son-to-school",
            //     Preconds = oal("son-at-home", "have-money"),
            //     AddList  = oal("son-at-school"),
            //     DelList  = oal("son-at-home", "have-money")},
            new Op(){ 
                Action   = "drive-son-to-school",
                Preconds = oal("son-at-home", "car-works"),
                AddList  = oal("son-at-school"),
                DelList  = oal("son-at-home")},
            new Op(){ 
                Action   = "shop-installs-battery",
                Preconds = oal("car-needs-battery", "shop-knows-problem", "shop-has-money"),
                AddList  = oal("car-works"),
                DelList  = oal()},
            new Op(){ 
                Action   = "tell-shop-problem",
                Preconds = oal("in-communication-with-shop"),
                AddList  = oal("shop-knows-problem"),
                DelList  = oal()},
            new Op(){ 
                Action   = "telephone-shop",
                Preconds = oal("know-phone-number"),
                AddList  = oal("in-communication-with-shop"),
                DelList  = oal()},
            new Op(){ 
                Action   = "look-up-number",
                Preconds = oal("have-phone-book"),
                AddList  = oal("know-phone-number"),
                DelList  = oal()},
            new Op(){ 
                Action   = "give-shop-money",
                Preconds = oal("have-money"),
                AddList  = oal("shop-has-money"),
                DelList  = oal("have-money")},
            new Op(){
                Action   = "ask-phone-number",
                Preconds = oal("in-communication-with-shop"),
                AddList  = oal("know-phone-number"),
                DelList  = oal()}
    };

    public static void PrintSolution(List<OpAction> solution)
    {
        if (solution == null)
            Print("Not solved");
        else
            foreach(var s in solution)
                Print(s);
    }

    private static Op ConvertOp(Op op)
    {
        if (op.DelList == null)
            op.DelList = oal();

        if (op.AddList.First().State != ActionState.Executing)
            op.AddList.Insert(0, new OpAction(){Name = op.Action, State = ActionState.Executing});

        return op;
    }



    public static int Main(string [] args)
    {
        Trace.Listeners.Add(new ConsoleTraceListener()); 

        var gps = new GPS(schoolOps.Select(ConvertOp).ToList());
        PrintSolution(gps.Solve(
                oal("son-at-home", "car-needs-battery", "have-money", "have-phone-book"),
                oal("son-at-school")));
        Print("");

        PrintSolution(gps.Solve(
                oal("son-at-home", "car-works"),
                oal("son-at-school")));
        Print("");

        //Clobbered sibling goal problem
        PrintSolution(gps.Solve(
                oal("son-at-home", "car-needs-battery", "have-money", "have-phone-book"),
                oal("have-money", "son-at-school")));
        Print("");
        
        //Recursive subgoal problem
        PrintSolution(gps.Solve(
                oal("son-at-home", "car-needs-battery", "have-money"),
                oal("son-at-school")));
        Print("");

        //Trivial
        PrintSolution(gps.Solve(
                oal("son-at-school"),
                oal("son-at-school")));
        Print("");


        //Test the Taxi Problem
        PrintSolution(gps.Solve(
                    oal("son-at-home", "have-money", "car-works"),
                    oal("son-at-school", "have-money")));

        return 0;
    }
}

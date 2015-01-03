// Example program chapter 4.11 of PAIP by Peter Norvig.
//
// Ported to C# by Jeremy English Sat Jan  3 17:05:23 CST 2015 

using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

public static class GeneralProblemSolver
{
    public static void Print(OpAction oa)
    {
        Console.WriteLine(String.Format("{0}: {1}", oa.State.ToString(), oa.Name));
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

    public static List<OpAction> addList(params string [] elems)
    {
        var ol = oal(elems);
        ol[0].State = ActionState.Executing;
        return ol;
    }


    static List<Op> schoolOps = new List<Op>(){
            new Op(){ 
                Action   = "drive-son-to-school",
                Preconds = oal("son-at-home", "car-works"),
                AddList  = addList("drive-son-to-school", "son-at-school"),
                DelList  = oal("son-at-home")},
            new Op(){ 
                Action   = "shop-installs-battery",
                Preconds = oal("car-needs-battery", "shop-knows-problem", "shop-has-money"),
                AddList  = addList("shop-installs-battery", "car-works"),
                DelList  = oal()},
            new Op(){ 
                Action   = "tell-shop-problem",
                Preconds = oal("in-communication-with-shop"),
                AddList  = addList("tell-shop-problem", "shop-knows-problem"),
                DelList  = oal()},
            new Op(){ 
                Action   = "telephone-shop",
                Preconds = oal("know-phone-number"),
                AddList  = addList("telephone-shop", "in-communication-with-shop"),
                DelList  = oal()},
            new Op(){ 
                Action   = "look-up-number",
                Preconds = oal("have-phone-book"),
                AddList  = addList("look-up-number", "know-phone-number"),
                DelList  = oal()},
            new Op(){ 
                Action   = "give-shop-money",
                Preconds = oal("have-money"),
                AddList  = addList("give-shop-money", "shop-has-money"),
                DelList  = oal("have-money")},
            new Op(){
                Action   = "ask-phone-number",
                Preconds = oal("in-communication-with-shop"),
                AddList  = addList("ask-phone-number", "know-phone-number"),
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


    public static int Main(string [] args)
    {
        Trace.Listeners.Add(new ConsoleTraceListener()); 

        var gps = new GPS(schoolOps);
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

        return 0;
    }
}

// Example program chapter 4.3 of PAIP by Peter Norvig.
//
// Ported to C# by Jeremy English Fri Dec 19 05:08:57 CST 2014

using System;
using System.Linq;
using System.Collections.Generic;

public static class GeneralProblemSolver
{
    public static void Print(string s)
    {
        Console.WriteLine(s);
    }

    //Return a string list with each 
    public static List<string> sl(params string [] elems)
    {
        return new List<string>(elems);
    }

    static List<Op> schoolOps = new List<Op>(){
        new Op(){ 
            Action   = "drive-son-to-school",
            Preconds = sl("son-at-home", "car-works"),
            AddList  = sl("son-at-school"),
            DelList  = sl("son-at-home")},
        new Op(){ 
            Action   = "shop-installs-battery",
            Preconds = sl("car-needs-battery", "shop-knows-problem", "shop-has-money"),
            AddList  = sl("car-works"),
            DelList  = sl()},
        new Op(){ 
            Action   = "tell-shop-problem",
            Preconds = sl("in-communication-with-shop"),
            AddList  = sl("shop-knows-problem"),
            DelList  = sl()},
        new Op(){ 
            Action   = "telephone-shop",
            Preconds = sl("know-phone-number"),
            AddList  = sl("in-communication-with-shop"),
            DelList  = sl()},
        new Op(){ 
            Action   = "look-up-number",
            Preconds = sl("have-phone-book"),
            AddList  = sl("know-phone-number"),
            DelList  = sl()},
        new Op(){ 
            Action   = "give-shop-money",
            Preconds = sl("have-money"),
            AddList  = sl("shop-has-money"),
            DelList  = sl("have-money")},
        new Op(){
            Action   = "ask-phone-number",
            Preconds = sl("in-communication-with-shop"),
            AddList  = sl("know-phone-number"),
            DelList  = sl()}

    };

    public static void IsSolved(bool b)
    {
        if (b)
            Print("Solved");
        else
            Print("NOT solved");
    }
    public static int Main(string [] args)
    {
        var gps = new GPS(
                sl("son-at-home", "car-needs-battery", "have-money", "have-phone-book"),
                sl("son-at-school"),
                schoolOps);
        IsSolved(gps.Solve());
        Print("");

        gps = new GPS(
                sl("son-at-home", "car-needs-battery", "have-money"),
                sl("son-at-school"),
                schoolOps);
        IsSolved(gps.Solve());
        Print("");

        gps = new GPS(
                sl("son-at-home", "car-works"),
                sl("son-at-school"),
                schoolOps);
        IsSolved(gps.Solve());
        Print("");

        //Clobbered sibling goal problem
        gps = new GPS(
                sl("son-at-home", "car-needs-battery", "have-money", "have-phone-book"),
                sl("have-money", "son-at-school"),
                schoolOps);
        IsSolved(gps.Solve());
        Print("");

        //Recursive subgoal problem
        gps = new GPS(
                sl("son-at-home", "car-needs-battery", "have-money"),
                sl("son-at-school"),
                schoolOps);
        IsSolved(gps.Solve());
        Print("");


        return 0;
    }
}

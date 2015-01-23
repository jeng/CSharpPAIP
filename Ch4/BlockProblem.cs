// Example program chapter 4.14 of PAIP by Peter Norvig.
//
// Ported to C# by Jeremy English Mon Jan 19 09:55:29 CST 2015

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

    public static List<List<T>> Permute<T>(params T [] args)
    {
        if (args.Length == 1)
            return new List<List<T>>(){new List<T>(){args[0]}};
        else
        {
            var res = new List<List<T>>();
            foreach(var x in args)
            {
                var tmpArgs = new List<T>(args);
                tmpArgs.Remove(x);
                var sublist = Permute(tmpArgs.ToArray());
                foreach(var element in sublist)
                {
                    var xs = new List<T>(){x};
                    xs.AddRange(element);
                    res.Add(xs);
                }
            }
            return res;
        }
    }

    public static List<Op> MakeOp(params string [] blockList)
    {
        //Should throw if block list is greater than three
        var tableBlockList = new List<string>(blockList);
        tableBlockList.Add("table");
        var resultList = Permute(tableBlockList.ToArray());
        var OpList = new List<Op>();
        foreach(var arrangement in resultList)
        {
            //The list is move a from b to c.  Since we cannot move the table
            //exclude these.
            if (arrangement.IndexOf("table") == 0)
                continue;

            var a = arrangement[0]; var b = arrangement[1]; var c = arrangement[2];
            var Op = new Op(){
                Action = String.Format("Move element {0} from {1} to {2}", a, b, c), 
                       Preconds = oal("space on " + a,
                               "space on " + c,
                               a + " on " + b),
                       AddList = MoveOns(a, b, c),
                       DelList = MoveOns(a, c, b)};

            OpList.Add(Op);
        }
        return OpList; 
    }

    static public List<OpAction> MoveOns(string a, string b, string c)
    {
        if (b == "table")
            return oal(a + " on " + c);
        else
            return oal(a + " on " + c, "space on " + b);
    }

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
        MakeOp("a", "b", "c");
        var blockWorld = MakeOp("a", "b");
        var gps = new GPS(blockWorld.Select(ConvertOp).ToList());
        PrintSolution(gps.Solve(
                    oal("a on table", "b on table", "space on a", "space on b"),
                    oal("a on b", "b on table")));
        Print("");

        PrintSolution(gps.Solve(
                    oal("a on b", "b on table", "space on a", "space on table"),
                    oal("b on a")));
        Print("");

        blockWorld = MakeOp("a", "b", "c");
        gps = new GPS(blockWorld.Select(ConvertOp).ToList());
        PrintSolution(gps.Solve(
                    oal("a on b", "b on c", "c on table", "space on a", "space on table"),
                    oal("b on a", "c on b")));

        Print("");

        blockWorld = MakeOp("a", "b", "c");
        gps = new GPS(blockWorld.Select(ConvertOp).ToList());
        PrintSolution(gps.Solve(
                    oal("a on b", "b on c", "c on table", "space on a", "space on table"),
                    oal("c on b", "b on a")));

        return 0;
    }
}

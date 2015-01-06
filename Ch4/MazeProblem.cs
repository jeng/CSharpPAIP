// Example program chapter 4.13 of PAIP by Peter Norvig.
//
// Ported to C# by Jeremy English Tue Jan  6 06:28:06 CST 2015

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

    private static List<Op> MakeMazeOps(Tuple<int, int> pair)
    {
        return new List<Op>(){
            MakeMazeOp(pair.Item1, pair.Item2),
            MakeMazeOp(pair.Item2, pair.Item1)};
    }

    private static Op MakeMazeOp(int here, int there)
    {
        return
            new Op(){ 
                Action   = string.Format("move from {0} to {1}", here, there),
                Preconds = oal(string.Format("at {0}", here)),
                AddList  = oal(string.Format("at {0}", there)),
                DelList  = oal(string.Format("at {0}", here))};
    }

    static List<Op> mazeOps = 
        new List<Tuple<int, int>>(){
            new Tuple<int, int>(1, 2),
            new Tuple<int, int>(2, 3),
            new Tuple<int, int>(3, 4),
            new Tuple<int, int>(4, 9),
            new Tuple<int, int>(9, 14),
            new Tuple<int, int>(9, 8),
            new Tuple<int, int>(8, 7),
            new Tuple<int, int>(7, 12),
            new Tuple<int, int>(12, 13),

            new Tuple<int, int>(12, 11),
            new Tuple<int, int>(11, 6),
            new Tuple<int, int>(11, 16),
            new Tuple<int, int>(16, 17),
            new Tuple<int, int>(17, 22),
            new Tuple<int, int>(21, 22),
            new Tuple<int, int>(22, 23),

            new Tuple<int, int>(23, 18),
            new Tuple<int, int>(23, 24),
            new Tuple<int, int>(24, 19),
            new Tuple<int, int>(19, 20),
            new Tuple<int, int>(20, 15),
            new Tuple<int, int>(15, 10),
            new Tuple<int, int>(10, 5),
            new Tuple<int, int>(20, 25)
        }.SelectMany(MakeMazeOps).ToList();

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

        var gps = new GPS(mazeOps.Select(ConvertOp).ToList());
        PrintSolution(gps.Solve(
                oal("at 1"),
                oal("at 25")));
        Print("");

        return 0;
    }
}

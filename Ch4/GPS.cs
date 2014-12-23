// Example program chapter 4.3 of PAIP by Peter Norvig.
//
// Ported to C# by Jeremy English Fri Dec 19 05:08:57 CST 2014

using System;
using System.Linq;
using System.Collections.Generic;

public class GPS
{
    //The current state: a list of conditions.
    private List<string> state;
    
    //A list of available operators
    private List<Op> ops;

    //The list of goals to achieve
    private List<string> goals;

    private Action<string> output;

    //Make this explicit versus using the properties when 
    //creating an instance
    public GPS(List<string> state, List<string> goals, List<Op> ops)
    {
        this.state = state;
        this.goals = goals;
        this.ops = ops;
        //TODO: Pass this in so we can handle the trace in different ways
        this.output = Console.WriteLine;
    }
    
    public bool Solve()
    {
        return AchieveAll(goals);
    }

    private bool Achieve(string goal)
    {
        return state.Any(x => x == goal) || 
               ops.Where(x => IsAppropriate(goal, x)).Any(ApplyOp);
    }

    private bool IsSubset(List<string> sub, List<string> set)
    {
        return !sub.Except(set).Any();
    }

    private bool AchieveAll(List<string> goals)
    {
        return goals.All(Achieve) && IsSubset(goals, this.state);
    }

    private bool IsAppropriate(string goal, Op op)
    {
        return op.AddList.Any(x => x == goal);
    }

    private bool ApplyOp(Op op)
    {
        if (AchieveAll(op.Preconds))
        {
            output(string.Format("Executing: {0}", op.Action));
            state = state.Except(op.DelList).ToList();
            state = state.Union(op.AddList).ToList();
            return true;
        }
        return false;
    }
}

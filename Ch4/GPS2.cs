// Example program chapter 4.11 of PAIP by Peter Norvig.
//
// Ported to C# by Jeremy English Sat Jan  3 17:04:49 CST 2015

using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

public class GPS
{
    //A list of available operators
    private List<Op> ops;

    public GPS(List<Op> ops)
    {
        this.ops = ops;
    }
   
    public List<OpAction> Solve(List<OpAction> state, List<OpAction> goals)
    {
        //Push a start onto the state to handle null list
        var StartAction = new OpAction(){Name = "", State = ActionState.Start};
        if (state == null)
            state = new List<OpAction>(){StartAction};
        else
            state.Insert(0, StartAction);
                
        var result = AchieveAll(state, goals, new List<OpAction>());
        if (result != null)
            return result
                .Where(x => x.State == ActionState.Start || x.State == ActionState.Executing)
                .ToList();
        else
            return null;
    }

    private List<OpAction> Achieve(List<OpAction> state, OpAction goal, List<OpAction> goalStack)
    {
        TraceIndent(string.Format("Trying to achieve goal: {0}", goal.Name), goalStack.Count());

        if (state.Any(x => x == goal))
            return state;
        else if (goalStack.Any(x => x == goal))
            return null;
        else
            return AppropriateOps(goal, state).Some(op => ApplyOp(state, goal, op, goalStack));
    }

    //Return a list of appropriate operations sorted by the number of unfulfilled preconditions
    private List<Op> AppropriateOps(OpAction goal, List<OpAction> state) 
    {
       return this.ops.Where(op => IsAppropriate(goal, op))
           .OrderBy(op => op.Preconds.CountIf(precond => !state.Any(s => s == precond)))
           .ToList();
    }

    private bool IsSubset(List<OpAction> sub, List<OpAction> set)
    {
        return !sub.Except(set).Any();
    }

    private List<OpAction> AchieveAll(List<OpAction> state, List<OpAction> goals, List<OpAction> goalStack)
    {
        return Orderings(goals).Some(newgoals => AchieveEach(state, newgoals, goalStack));
    }

    //Returns null when they cannot be achieved
    private List<OpAction> AchieveEach(List<OpAction> state, List<OpAction> goals, List<OpAction> goalStack)
    {
        List<OpAction> currentState = new List<OpAction>(state);
        Func<OpAction, bool> AchieveClosure = 
            (goal => 
                { 
                    Console.WriteLine(String.Format("testing: {0}", goal));
                    foreach(var s in goalStack)
                        Console.WriteLine(String.Format("testing: {0}", s));
                    currentState = Achieve(currentState, goal, goalStack); 
                    return currentState != null;
                });

        if (goals.All(AchieveClosure) && IsSubset(goals, currentState))
            return currentState;
        else
            return null;
    }

    private List<List<OpAction>> Orderings(List<OpAction> lop)
    {
        if (lop.Count > 1)
        {
            var reverse = new List<OpAction>(lop);
            reverse.Reverse();
            return new List<List<OpAction>>(){
                new List<OpAction>(lop), reverse};
        }
        else
        {
            return new List<List<OpAction>>(){
                new List<OpAction>(lop)};
        }
    }

    private bool IsAppropriate(OpAction goal, Op op)
    {
       return op.AddList.Any(x => x == goal);
    }

    private List<OpAction> ApplyOp(List<OpAction> state, OpAction goal, Op op, List<OpAction> goalStack)
    {
        TraceIndent(string.Format("Consider: {0}", op.Action), goalStack.Count());
        //If we already optained the goal then just exit
        //if (state.Exists(x => x == goal
        foreach(var s in goalStack)
            TraceIndent(string.Format("GOAL TEST: {0}", s), goalStack.Count());

        List<OpAction> tempGoalState = new List<OpAction>(goalStack);
        tempGoalState.Insert(0, goal);

        List<OpAction> state2 = AchieveAll(state, op.Preconds, tempGoalState);
        
        if (state2 != null)
        {
            TraceIndent(string.Format("Action: {0}", goal.Name), goalStack.Count());
            state2.RemoveAll(x => op.DelList.Any(y => y == x));
            state2.AddRange(op.AddList);
            return state2;
        }
        return null;
    }

    private void TraceIndent(string s, int indent)
    {
        Trace.WriteLine(string.Format("{0}{1}", "".PadLeft(indent), s));
    }
}

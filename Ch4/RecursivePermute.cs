using System;
using System.Collections.Generic;
using System.Linq;

public static class StackPermute
{
    public static List<List<T>> Permute<T>(params T [] args)
    {
        var origStack = new Stack<T>(args.ToList());
        var stack = new Stack<Stack<T>>();
        var tmpStack = new Stack<Stack<T>>();

        for(var i = origStack.Count(); i != 0; i--)
            stack.Push(new Stack<T>(origStack));

        var workingList = new List<T>();
        var resultList = new List<List<T>>();

        var popNext = false;
        var working = true;
        while(working)
        {
            while(stack.Count != 0)
            {
                var item = stack.Pop();
                T first;

                if (tmpStack.Count == 0 || popNext)
                    first = item.Pop();
                else
                    first = item.Peek();
            
                workingList.Add(first);

                popNext = (item.Count == 0);

                if (popNext)
                    tmpStack.Push(new Stack<T>(origStack));
                else
                    tmpStack.Push(item);

                working = !(popNext && stack.Count == 0);
            }

            resultList.Add(new List<T>(workingList));
            workingList.Clear();
 
            while(tmpStack.Count != 0)
                stack.Push(tmpStack.Pop());
        }

        return resultList;
    }


    public static void Dump<T>(List<List<T>> data)
    {
        foreach(var x in data)
        {
            foreach(var y in x)
                Console.Write(string.Format("{0} ", y));

            Console.WriteLine();
        }
    }

    
    public static void Main()
    {
        //Dump(Permute("abc", "123", "you", "me"));
        //Dump(Permute(1,2,3,4,5));
        Dump(Permute("John", "Rob", "Peter", "Don", "Alan", "Doug", "Ivan"));
    }
}

// Program exercise chapter 4 of PAIP by Peter Norvig.
//
// Iterative version in C# by Jeremy English Sun Jan 11 17:54:55 CST 2015

using System;
using System.Collections.Generic;
using System.Linq;

public static class StackPermute
{
    public static List<List<T>> Permute<T>(params T [] args)
    {
        var stack = new Stack<List<T>>();

        var workingList = new List<T>();
        var resultList = new List<List<T>>();

        var next = new List<T>(args);
        stack.Push(next);
        while(next.Count != 0)
        {
            var first = next.First();
            workingList.Add(first);
            next = new List<T>(args);
            foreach(var e in workingList)
                next.Remove(e);

            if (next.Count == 0)
                resultList.Add(new List<T>(workingList));

            while (next.Count == 0 && stack.Count != 0)
            {
                next = stack.Pop();
                next.RemoveAt(0);
                workingList.RemoveAt(workingList.Count -1);
            }
            stack.Push(next);
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
        //Dump(Permute("a"));
        //Dump(Permute("abc", "123", "you", "me"));
        //Dump(Permute(1,2,3,4,5));
        Dump(Permute("John", "Rob", "Peter", "Don", "Alan", "Doug", "Ivan", "Adele"));
        //Dump(Permute("a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k"));
    }
}

// Program exercise chapter 4 of PAIP by Peter Norvig.
//
// Recursive version in C# by Jeremy English Sun Jan 11 17:54:55 CST 2015

using System;
using System.Collections.Generic;
using System.Linq;

public static class StackPermute
{
    
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

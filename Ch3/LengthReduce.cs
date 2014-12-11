using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;

public static class LengthReduce
{
    public static int Length<T>(List<T> list)
    {
        return list.Select(x => 1).Aggregate((x, y) => x + y);
    }

    public static void Main()
    {
        Debug.Assert(Length(new List<string>(){"foo", "bar", "baz"}) == 3);
        Random r = new Random();
        var nl = new List<int>();
        int stop = r.Next(1, short.MaxValue);
        Console.WriteLine(String.Format("Stop: {0}", stop));
        for(int i = 0; i < stop; i++)
        {
            nl.Add(i);
        }
        Console.WriteLine(String.Format("Length: {0}", Length(nl)));
    }
}

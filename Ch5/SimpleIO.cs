using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;


namespace SimpleIO
{
    public static class SimpleIO
    {
        public static void Print(string pattern, params object [] vars)
        {
            Console.WriteLine(String.Format(pattern, vars));
        }
     
        public static void Reader(Func<string, bool> callback)
        {
            while(true)
            {
                string s = Console.ReadLine();
                if (!callback(s))
                    break;
            }
        }
    }
}

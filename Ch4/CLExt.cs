using System;
using System.Linq;
using System.Collections.Generic;

public static class CLExt
{
    public static int CountIf<T>(this List<T> elements, Func <T, bool> predicate)
    {
        //This could also be implemented as a Where and then Count
        return elements.Select(x => predicate(x) ? 1 : 0).Aggregate((x, y) => x + y);
    }

    public static T Some<T, U>(this List<U> operators, Func<U, T> predicate) 
        where U : class where T : class
    {
        foreach(var op in operators)
        {       
            var result = predicate(op);
            if (result != null)
                return result;
        }
        return null;
    }
}


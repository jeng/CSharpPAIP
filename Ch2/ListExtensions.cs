using System.Collections.Generic;

public static class ListExtensions
{
    public static T OneOf<T>(this List<T> xs)
    {
        if (xs.Count <= 0)
            return default(T);
        else
        {
            RandomSingleton rnd = RandomSingleton.GetInstance();
            return xs[rnd.Random.Next(0, xs.Count)];
        }
    }
}


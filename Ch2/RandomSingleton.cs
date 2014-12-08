using System;

public class RandomSingleton 
{
    private static RandomSingleton unique;
    private Random random;

    private RandomSingleton() 
    {
        random = new Random();
    }

    public static RandomSingleton GetInstance() 
    {
        if (unique == null)
            unique = new RandomSingleton();
        return unique;
    }

    public Random Random
    {
        get
        {
            return random;
        }
    }
}
    

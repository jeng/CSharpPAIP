using System.Linq;
using System.Collections.Generic;
public class SimpleGrammar
{
    public string Article()
    {
        return new List<string>(){"the", "a"}.OneOf();
    }

    public string Noun()
    {
        return new List<string>(){"man", "ball", "woman", "table"}.OneOf();
    }

    public string Verb()
    {
        return new List<string>(){"hit", "took", "saw", "liked"}.OneOf();
    }

    public string Adj()
    {
        return new List<string>(){"big", "little", "blue", "green", "adiabatic"}.OneOf();
    }

    public string Prep()
    {
        return new List<string>(){"to", "in", "by", "with", "on"}.OneOf();
    }

    public List<string> NounPharse()
    {
        return new List<string>(){Article()}
                    .Concat(AdjStar())
                    .Concat(new List<string>(){Noun()})
                    .Concat(PPStar())
                    .ToList();
    }

    public List<string> VerbPharse()
    {
        return new List<string>(){Verb()}.Concat(NounPharse()).ToList();
    }
    
    public List<string> AdjStar()
    {
        var rng = RandomSingleton.GetInstance();
        if (rng.Random.Next(2) == 0)
            return new List<string>();
        else
            return new List<string>(){Adj()}.Concat(AdjStar()).ToList();
    }

    public List<string> PP()
    {
        return new List<string>() {Prep()}.Concat(NounPharse()).ToList();                
    }

    public List<string> PPStar()
    {
        var rng = RandomSingleton.GetInstance();
        if (rng.Random.Next(2) == 0)
            return new List<string>();
        else
            return PP().Concat(PPStar()).ToList();
    }

    public List<string> Sentence()
    {
        return NounPharse().Concat(VerbPharse()).ToList();
    }
}

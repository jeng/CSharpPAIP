// Example program chapter 4.11 of PAIP by Peter Norvig.
//
// Ported to C# by Jeremy English Sat Jan  3 17:06:04 CST 2015

using System;
using System.Linq;
using System.Collections.Generic;

public class OpAction
{
    public string Name { get; set; }
    public ActionState State { get; set; }


    public override bool Equals(System.Object obj)
    {
        if (obj == null)
        {
            return false;
        }

        OpAction oa = obj as OpAction;
        if ((System.Object)oa == null)
        {
            return false;
        }

        return (Name == oa.Name);
    }

    public bool Equals(OpAction oa)
    {
        // If parameter is null return false:
        if ((object)oa == null)
        {
            return false;
        }

        // Return true if the fields match:
        return (Name == oa.Name);
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }

    public static bool operator ==(OpAction a, OpAction b)
    {
        // If both are null, or both are same instance, return true.
        if (System.Object.ReferenceEquals(a, b))
        {
            return true;
        }

        // If one is null, but not both, return false.
        if (((object)a == null) || ((object)b == null))
        {
            return false;
        }

        // Return true if the fields match:
        return a.Name == b.Name;
    }

    public static bool operator !=(OpAction a, OpAction b)
    {
        return !(a == b);
    }
}

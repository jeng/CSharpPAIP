using System;
using System.Linq;
using System.Collections.Generic;

public class Rule
{
    public List<Rule> Children { get; set; }
    public List<Rule> Sibling { get; set; }
    public string Value { get; set;}
    public override string ToString()
    {
        string siblings = (Sibling == null)  ? "" : String.Join(" ", Sibling.Select(x => x.Value));
        string children = (Children == null) ? "" : String.Join(" ", Children.Select(x => x.Value));
        return String.Format("Name: {0}\nSiblings: {1}\nChildren: {2}\n", 
                Value, siblings, children);
    }
    public Rule()
    {
        this.Value = "";
    }
}

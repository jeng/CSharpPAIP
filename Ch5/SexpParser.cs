//A very basic s-expression parser
using System.Collections.Generic;

namespace SExp
{
    public enum CellType
    {
        list,
        atom
    }

    public class Cell
    {
        private string atom;
        private List<Cell> list;
        public CellType Type;
        public Cell(string s)
        {
            Type = CellType.atom;
            atom = s;        
        }
        public Cell(List<Cell> cl)
        {
            Type = CellType.list;
            list = cl;
        }
        public List<Cell> List
        {
            get
            {
                return list;
            }
        }
        public string Atom
        {
            get
            {
                return atom;
            }
        }
    }

    public class SexpParser
    {
        const char LPAREN = '(';
        const char RPAREN = ')';
        const char QUOTE  = '\'';

        private bool IsSpace(char c)
        {
            return (c == ' ' || c == '\t' || c == '\r' || c == '\n');
        }

        private void SkipSpace(string s, ref int n)
        {
            while(n < s.Length && IsSpace(s[n]))
                n++;
        }

        private string ParseAtom(string s, ref int n)
        {
            SkipSpace(s, ref n);
            string atom = "";
            while(!IsSpace(s[n]) && s[n] != RPAREN && s[n] != LPAREN)
                atom += s[n++];
            SkipSpace(s, ref n);
            return atom;
        }

        private List<Cell> ParseList(string s, ref int n)
        {
            SkipSpace(s, ref n);
            var result = new List<Cell>();
            while (s[n] != RPAREN)
            {
                if (s[n] == QUOTE)
                    n++;
                if (s[n] == LPAREN)
                {
                    n++;
                    Cell c = new Cell(ParseList(s, ref n));
                    result.Add(c);
                }
                else
                {
                    Cell c = new Cell(ParseAtom(s, ref n));
                    result.Add(c);
                }
            }
            n++;//Move past RPAREN
            SkipSpace(s, ref n);
            return result;
        }

        public List<Cell> Parse(string s)
        {
            s = s.TrimStart();
            if (s.Length == 0)
                return new List<Cell>();
            int n = 0;
            if (s[n] == QUOTE)
                n++;
            if (s[n] == LPAREN)
            {
                n++;
                return ParseList(s, ref n);
            }
            else
            {
                return new List<Cell>(){new Cell(ParseAtom(s, ref n))};
            }
        }
    }
}

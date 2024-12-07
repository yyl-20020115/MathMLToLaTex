namespace MaTeX.LaTexToMathML;

public abstract class Node
{
    public class Number(string s) : Node { public readonly string s = s; }
    public class Letter(char c, Variant v) : Node { public readonly char c = c; public readonly Variant v = v; }
    public class Operator(char c) : Node { public readonly char c = c; }
    public class Function(string s, Node n) : Node { public readonly string s = s; public readonly Node n = n; }
    public class Space(float f) : Node { public readonly float f = f; }
    public class Subscript(Node n0, Node n1) : Node { public readonly Node n0 = n0; public readonly Node n1 = n1; }
    public class Superscript(Node n0, Node n1) : Node { public readonly Node n0 = n0; public readonly Node n1 = n1; }
    public class SubSup(Node target, Node sub, Node sup) : Node { public readonly Node target = target; public readonly Node sub = sub; public readonly Node sup = sup; }
    public class OverOp(char c, bool b, Node n) : Node { public readonly char c = c; public readonly bool b = b; public readonly Node n = n; }
    public class UnderOp(char c, bool b, Node n) : Node { public readonly char c = c; public readonly bool b = b; public readonly Node n = n; }
    public class Overset(Node over, Node target) : Node { public readonly Node target = target; public readonly Node over = over; }
    public class Underset(Node under, Node target) : Node { public readonly Node target = target; public readonly Node under = under; }
    public class Under(Node target, Node under) : Node { public readonly Node target = target; public readonly Node under = under; }
    public class UnderOver(Node target, Node under, Node over) : Node { public readonly Node target = target; public readonly Node under = under; public readonly Node over = over; }
    public class Sqrt(Node? deg, Node content) : Node { public readonly Node? deg = deg; public readonly Node content = content; }
    public class Frac(Node n0, Node n1, LineThickness lt) : Node { public readonly Node n0 = n0; public readonly Node n1 = n1; public readonly LineThickness lt = lt; }
    public class Row(Node[] ns) : Node { public readonly Node[] ns = ns; }
    public class Fenced(string open, string close, Node content) : Node { public readonly string open = open; public readonly string close = close; public readonly Node content = content; }
    public class StrechedOp(bool b, string s) : Node { public readonly bool b = b; public readonly string s = s; }
    public class OtherOperator(string s) : Node { public readonly string s = s; }
    public class SizedParen(string size, string paren) : Node { public readonly string size = size; public readonly string paren = paren; }
    public class Text(string s) : Node { public readonly string s = s; }
    public class Matrix(Node[] ns, ColumnAlign alignment) : Node { public readonly Node[] ns = ns; public ColumnAlign alignment = alignment; }
    public class Ampersand : Node { }
    public class NewLine : Node { }
    public class Slashed(Node n) : Node { public readonly Node n = n; }
    public class Style(DisplayStyle d, Node n) : Node { public DisplayStyle d = d; public readonly Node n = n; }
    public class Undefined(string s) : Node { public readonly string s = s; }

}

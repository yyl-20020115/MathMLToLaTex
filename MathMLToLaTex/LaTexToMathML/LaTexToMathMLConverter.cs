using MaTex.MathMLToLaTex;

namespace MaTeX.LaTexToMathML;

public static class LaTexToMathMLConverter
{
    public static MathMLElement? Convert(Node node) => node switch
    {
        Node.Number number => new MathMLElement("mn", $"{number.s}"),
        Node.Letter letter => new MathMLElement("mi", $"{letter.c}",
            letter.v == Variant.Italic ? [] : [("mathvariant", letter.v.ToString())]),
        Node.Operator @operator => @operator.c == '∂'
            ? new MathMLElement("mo", $"{@operator.c}", [("mathvariant", "italic")])
            : new MathMLElement("mo", $"{@operator.c}"),
        Node.Function function => function.n != null
            ? new MathMLElement("mrow", [new MathMLElement("mi", function.s), new MathMLElement("mo", "\u2061"), Convert(function.n)])
            : new MathMLElement("mi"),
        Node.Space space => new MathMLElement("mspace", ("width", $"{space.f}em")),
        Node.Subscript sub => new MathMLElement("msub", [Convert(sub.left), Convert(sub.right)]),
        Node.Superscript sup => new MathMLElement("msup", [Convert(sup.left), Convert(sup.right)]),
        Node.SubSup subsup => new MathMLElement("msubsup", [Convert(subsup.target), Convert(subsup.sub), Convert(subsup.sup)]),
        Node.OverOp overop => new MathMLElement("mover", [Convert(overop.n), new MathMLElement("mo", $"{overop.b}", [("accent", $"{overop.c}")])]),
        Node.UnderOp underop => new MathMLElement("munder", [Convert(underop.n), new MathMLElement("mo", $"{underop.b}", [("accent", $"{underop.c}")])]),
        Node.Overset overset => new MathMLElement("mover", [Convert(overset.target), Convert(overset.over)]),
        Node.Underset underset => new MathMLElement("munder", [Convert(underset.target), Convert(underset.under)]),
        Node.Under under => new MathMLElement("munder", [Convert(under.target), Convert(under.under)]),
        Node.UnderOver underover => new MathMLElement("munderover", [Convert(underover.target), Convert(underover.under), Convert(underover.over)]),
        Node.Sqrt sqrt => sqrt.deg != null ? new MathMLElement("mroot", [Convert(sqrt.content), Convert(sqrt.deg)]) : new MathMLElement("msqrt", [Convert(sqrt.content)]),
        Node.Frac frac => new MathMLElement("mfrac", [Convert(frac.n0), Convert(frac.n1)], ("thickness", $"{frac.lt}")),
        Node.Row row => new MathMLElement("mrow", [.. row.ns.Select(n => Convert(n))]),
        Node.Fenced fenced => new MathMLElement("mrow", [new MathMLElement("mo", $"{fenced.open}", ("stretchy", "true"), ("form", "prefix")), Convert(fenced.content), new MathMLElement("mo", $"{fenced.close}", ("stretchy", "true"), ("form", "postfix"))]),
        Node.StrechedOp op => new MathMLElement("mo", $"{op.s}", ("stretchy", $"{op.b}")),
        Node.OtherOperator op => new MathMLElement("mo", $"{op.s}"),
        Node.SizedParen sp => new MathMLElement("mrow", [new MathMLElement("mo", $"{sp.paren}", ("maxsize", $"{sp.size}"), ("minsize", $"{sp.size}"))]),
        Node.Slashed slash => slash.n is Node.Letter letter ? new MathMLElement("mi", $"{letter.c}&#x0338", ("mathvariant", $"{letter.v}")) : slash.n is Node.Operator o ? new MathMLElement("mo", $"{o.c}&#x0338") : Convert(slash.n),
        Node.Matrix matrix => new MathMLElement("mtable", [.. matrix.ns.Select(n => new MathMLElement("mtr", [new MathMLElement("mtd", [Convert(n)])]))], ("columnalign", $"{matrix.alignment}")),
        Node.Text text => new MathMLElement("mtext", text.s),
        Node.Style style => style.d switch
        {
            DisplayStyle.Block => new MathMLElement("mstyle",("displaystyle", "true")),
            DisplayStyle.Inline => new MathMLElement("mstyle", ("displaystyle", "false")),
            _ => new MathMLElement("mstyle")
        },
        //for Error
        _ => new MathMLElement("mtext", $"[PARSE ERROR:{node}]")
    };
    public static string Convert(string latex)
    {
        using var reader = new StringReader(latex);
        var lexer = new Lexer(reader);
        var paser = new Parser(lexer);
        var (ns, e) = paser.Parse();
        return e is null
            ? string.Join("",
                ns.Select(n => Convert(n)).Select(m => MathMLToElementsAdapter.Convert(m!)))
            : MathMLToElementsAdapter.Convert(
            Convert(new Node.Errror($"{e}"))!).ToString();
    }
}

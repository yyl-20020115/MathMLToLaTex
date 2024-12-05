using MaTex.MathMLToLaTex;

namespace MaTeX.LaTexToMathML;

public static class LaTexToMathMLConverter
{
    public static MathMLElement? Convert(Node node) => node switch
    {
        Node.Number number => new MathMLElement("mn", number.s),
        Node.Letter letter => new MathMLElement("mi", letter.c.ToString(), 
            letter.v== Variant.Italic ?[] : [("mathvariant", letter.v.ToString())]),
        Node.Operator @operator => @operator.c == '∂'
            ? new MathMLElement("mo", @operator.c.ToString(), [("mathvariant", "italic")])
            : new MathMLElement("mo",@operator.c.ToString()),

        _ => new MathMLElement("mtext", $"[PARSE ERROR:{node}]")
    };
    public static string Convert(string latex)
    {
        MathMLElement mathML = Convert(default(Node));

        return MathMLToElementsAdapter.Convert(mathML).ToString();
    }
}

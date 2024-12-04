namespace MathMLToLaTex.ToLatexConverters;

public class MSqrt(MathMLElement Element) : ToLatexConverter(Element)
{
    public override string Convert() => $"\\sqrt{{{base.Convert()}}}";
}

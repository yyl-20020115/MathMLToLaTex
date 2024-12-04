namespace MathMLToLaTex.ToLatexConverters;

public class Void(MathMLElement Element) : ToLatexConverter(Element)
{
    public override string Convert() => "";
}

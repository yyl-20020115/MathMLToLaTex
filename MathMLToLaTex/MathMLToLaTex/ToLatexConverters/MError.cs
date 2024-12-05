namespace MaTex.MathMLToLaTex.ToLatexConverters;

public class MError(MathMLElement Element) : ToLatexConverter(Element)
{
    public override string Convert()
        => $"\\color{{red}}{{{base.Convert()}}}";
}

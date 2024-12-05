using MaTex.MathMLToLaTex.Helpers;
namespace MaTex.MathMLToLaTex.ToLatexConverters;

public class Math(MathMLElement Element) : ToLatexConverter(Element)
{
    public override string Convert()
        => Utilities.NormalizeWhiteSpaces(base.Convert());
}

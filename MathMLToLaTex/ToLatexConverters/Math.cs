using MathMLToLaTex.Helpers;
namespace MathMLToLaTex.ToLatexConverters;

public class Math(MathMLElement Element) : ToLatexConverter(Element)
{
    public override string Convert()
        => Utilities.NormalizeWhiteSpaces(base.Convert());
}

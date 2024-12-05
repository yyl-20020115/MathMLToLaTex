using MaTex.MathMLToLaTex.Helpers;

namespace MaTex.MathMLToLaTex.ToLatexConverters;

public class MN(MathMLElement Element) : ToLatexConverter(Element)
{
    public override string Convert()
    {
        var normalizedValue = Utilities.NormalizeWhiteSpaces(this.Element.Value).Trim();
        return Dicts.NumberByGlyph.TryGetValue(normalizedValue, out var v) ? v : normalizedValue;
    }
}

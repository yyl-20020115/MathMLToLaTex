using MathMLToLaTex.Helpers;

namespace MathMLToLaTex.ToLatexConverters;

public class MO(MathMLElement Element) : ToLatexConverter(Element)
{
    public override string Convert()
    {
        var value = Utilities.NormalizeWhiteSpaces(this.Element.Value).Trim();
        return Dicts.MathOperatorsByChar.TryGetValue(value, out var c0) ? c0 :
            Dicts.MathOperatorsByGlyph.TryGetValue(value, out var c1) ? c1 :
            Dicts.NumberByGlyph.TryGetValue(value, out var c2) ? c2 :
            UTF8ToLtXConverter.Convert(value)
            ;
    }
}


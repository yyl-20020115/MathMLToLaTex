using MathMLToLaTex.Helpers;

namespace MathMLToLaTex.ToLatexConverters;

public class GenericUnderOver(MathMLElement Element) : ToLatexConverter(Element)
{
    protected static string GetDefaultCmd(bool under) => under ? "\\underset" : "\\overset";
    public override string Convert()
    {
        if (this.Element.Children.Length != 2) throw new InvalidDataException(nameof(Element.Children));

        var content = MathMLElementToLatexConverterAdapter.Convert(this.Element.Children[0]).Convert();
        var accent = MathMLElementToLatexConverterAdapter.Convert(this.Element.Children[1]).Convert();

        return (Dicts.LatexAccents.Contains(accent)) ?
            $"{accent}{{{content}}}" : $"{GetDefaultCmd(this.Element.Name.Equals("under", StringComparison.OrdinalIgnoreCase))}{{{accent}}}{{{content}}}";
    }
}

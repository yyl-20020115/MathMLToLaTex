using MathMLToLaTex.Helpers.Wrappers;

namespace MathMLToLaTex.ToLatexConverters;

public class MFrac(MathMLElement Element) : ToLatexConverter(Element)
{
    public bool IsBevelled => this.Element.Attributes.TryGetValue("bevelled", out var v) && bool.TryParse(v, out var b) && b;
    public override string Convert()
    {
        if (this.Element.Children.Length != 2) throw new InvalidDataException(nameof(Element.Children));

        var upper = MathMLElementToLatexConverterAdapter.Convert(this.Element.Children[0]).Convert();
        var lower = MathMLElementToLatexConverterAdapter.Convert(this.Element.Children[1]).Convert();

        return this.IsBevelled ? $"{new ParenthesisWrapper().WrapIfMore(upper)}/{new ParenthesisWrapper().WrapIfMore(lower)}" : $"\\frac{upper}{lower}";
    }
}


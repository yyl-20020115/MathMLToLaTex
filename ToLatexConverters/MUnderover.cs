namespace MathMLToLaTex.ToLatexConverters;

public class MUnderover(MathMLElement Element) : ToLatexConverter(Element)
{
    public override string Convert()
    {
        if (this.Element.Children.Length != 3) throw new InvalidDataException(nameof(Element.Children));

        var _base = MathMLElementToLatexConverterAdapter.Convert(this.Element.Children[0]).Convert();
        var under = MathMLElementToLatexConverterAdapter.Convert(this.Element.Children[1]).Convert();
        var over = MathMLElementToLatexConverterAdapter.Convert(this.Element.Children[2]).Convert();

        return $"{_base}_{under}^{over}";
    }
}

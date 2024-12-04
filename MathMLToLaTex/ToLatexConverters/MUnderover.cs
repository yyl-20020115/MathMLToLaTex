namespace MathMLToLaTex.ToLatexConverters;

public class MUnderover(MathMLElement Element) : ToLatexConverter(Element)
{
    public override string Convert()
    {
        if (this.Element.Children.Length != 3) throw new InvalidDataException(nameof(Element.Children));

        var baseChild = MathMLElementToLatexConverterAdapter.Convert(this.Element.Children[0]).Convert();
        var underChild = MathMLElementToLatexConverterAdapter.Convert(this.Element.Children[1]).Convert();
        var overChild = MathMLElementToLatexConverterAdapter.Convert(this.Element.Children[2]).Convert();

        return $"{baseChild}_{{{underChild}}}^{{{overChild}}}";
    }
}

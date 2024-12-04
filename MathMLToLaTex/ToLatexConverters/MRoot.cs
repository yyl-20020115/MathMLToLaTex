namespace MathMLToLaTex.ToLatexConverters;

public class MRoot(MathMLElement Element) : ToLatexConverter(Element)
{
    public override string Convert()
    {
        if (this.Element.Children.Length != 2) throw new InvalidDataException(nameof(Element.Children));

        var content = MathMLElementToLatexConverterAdapter.Convert(this.Element.Children[0]).Convert();
        var rootIndex = MathMLElementToLatexConverterAdapter.Convert(this.Element.Children[1]).Convert();

        return $"\\sqrt[{rootIndex}]{{{content}}}";
    }
}


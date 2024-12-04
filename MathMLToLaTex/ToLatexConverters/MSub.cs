namespace MathMLToLaTex.ToLatexConverters;
public class MSub(MathMLElement Element) : MScript(Element)
{
    public override string Convert()
    {
        if (this.Element.Children.Length != 2) throw new InvalidDataException(nameof(Element.Children));

        var baseChild = this.Element.Children[0];
        var subChild = this.Element.Children[1];

        return $"{ConvertBase(baseChild)}_{ConvertScript(subChild)}";
    }
}


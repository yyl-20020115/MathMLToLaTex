namespace MathMLToLaTex.ToLatexConverters;
public class MSub(MathMLElement Element) : MScript(Element)
{
    public override string Convert()
    {
        if (this.Element.Children.Length != 2) throw new InvalidDataException(nameof(Element.Children));

        var _base = this.Element.Children[0];
        var sub = this.Element.Children[1];

        return $"{ConvertBase(_base)}_{ConvertScript(sub)}";
    }
}


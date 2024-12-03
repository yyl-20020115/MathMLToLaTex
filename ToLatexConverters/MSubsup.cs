using MathMLToLaTex.Helpers.Wrappers;

namespace MathMLToLaTex.ToLatexConverters;

public class MSubsup(MathMLElement Element) : MScript(Element)
{
    public override string Convert()
    {
        if (this.Element.Children.Length != 3) throw new InvalidDataException(nameof(Element.Children));

        var _base = this.Element.Children[0];
        var sub = this.Element.Children[1];
        var sup = this.Element.Children[2];

        return $"{ConvertBase(_base)}_{ConvertScript(sub)}^{ConvertScript(sup)}";
    }
}

using MathMLToLaTex.Helpers.Wrappers;

namespace MathMLToLaTex.ToLatexConverters;

public class MSubsup(MathMLElement Element) : MScript(Element)
{
    public override string Convert()
    {
        if (this.Element.Children.Length != 3) throw new InvalidDataException(nameof(Element.Children));

        var baseChild = this.Element.Children[0];
        var subChild = this.Element.Children[1];
        var supChild = this.Element.Children[2];

        return $"{ConvertBase(baseChild)}_{ConvertScript(subChild)}^{ConvertScript(supChild)}";
    }
}

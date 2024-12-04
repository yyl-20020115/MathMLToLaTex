namespace MathMLToLaTex.ToLatexConverters;

public class MAction(MathMLElement Element) : ToLatexConverter(Element)
{
    protected bool IsToggle 
        => Element.Attributes.TryGetValue("actiontype", out var ac) && ac == "toggle"
        || !Element.Attributes.ContainsKey("actiontype");

    public override string Convert() => IsToggle
            ? string.Join(" \\Longrightarrow ", this.Element.Children.Select(
                child => MathMLElementToLatexConverterAdapter.Convert(child)).Select(c => c.Convert()))
            : MathMLElementToLatexConverterAdapter.Convert(this.Element.Children[0]).Convert();
}

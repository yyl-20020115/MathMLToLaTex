namespace MathMLToLaTex.ToLatexConverters;

public class MTr(MathMLElement Element) : ToLatexConverter(Element)
{
    public override string Convert()
         => string.Join(" & ",
            this.Element.Children.Select(
                child => MathMLElementToLatexConverterAdapter.Convert(child)).Select(c => c.Convert()));
}

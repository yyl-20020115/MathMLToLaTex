namespace MathMLToLaTex;

public class ToLatexConverter(MathMLElement Element)
{
    public readonly MathMLElement Element = Element;
    public virtual string Convert()
         => string.Join(' ',
            this.Element.Children.Select(
                child => MathMLElementToLatexConverterAdapter.Convert(child)).Select(c => c.Convert()));

}

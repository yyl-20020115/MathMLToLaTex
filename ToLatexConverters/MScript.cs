using MathMLToLaTex.Helpers.Wrappers;

namespace MathMLToLaTex.ToLatexConverters;

public class MScript(MathMLElement Element) : ToLatexConverter(Element)
{
    protected static string ConvertBase(MathMLElement mathMLElement)
    {
        var str = MathMLElementToLatexConverterAdapter.Convert(mathMLElement).Convert();
        return mathMLElement.Children.Length <= 1 ? str : new ParenthesisWrapper().WrapIfMore(str);
    }
    protected static string ConvertScript(MathMLElement mathMLElement)
    {
        var str = MathMLElementToLatexConverterAdapter.Convert(mathMLElement).Convert();
        return new BracketWrapper().Wrap(str);
    }

}


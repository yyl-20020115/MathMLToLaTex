using MathMLToLaTex.Helpers.Wrappers;
using System.Text.RegularExpressions;

namespace MathMLToLaTex.ToLatexConverters;

public class MMultiscripts(MathMLElement Element) : ToLatexConverter(Element)
{
    public static Regex Spaces = new("\\s+");
    public static string WrapIfSpaces(string text)
        => !Spaces.IsMatch(text) ? text : new ParenthesisWrapper().Wrap(text);

    public static bool IsPrescript(MathMLElement Element) => Element.Name.Equals("mprescripts", StringComparison.CurrentCultureIgnoreCase);

    public override string Convert()
    {
        if (this.Element.Children.Length < 3) throw new InvalidDataException(nameof(Element.Children));

        var baseText = MathMLElementToLatexConverterAdapter.Convert(this.Element.Children[0]).Convert();

        return this.ConvertPreScrpt() + WrapIfSpaces(baseText) + ConvertPostScript();

    }
    private string ConvertPreScrpt()
    {
        var children = this.Element.Children;
        MathMLElement sub, sup;
        if (children.Length >= 4 && IsPrescript(children[1]))
        {
            sub = children[2];
            sup = children[3];
        }
        else if (children.Length >= 6 && IsPrescript(children[3]))
        {
            sub = children[4];
            sup = children[5];
        }
        else
            return "";

        var subLatex = MathMLElementToLatexConverterAdapter.Convert(sub).Convert();
        var supLatex = MathMLElementToLatexConverterAdapter.Convert(sup).Convert();

        return $"\\_{{{subLatex}}}^{{{supLatex}}}";

    }

    private string ConvertPostScript()
    {
        var children = this.Element.Children;

        if (IsPrescript(children[1]) || children.Length < 3) return "";

        var sub = children[1];
        var sup = children[2];

        var subLatex = MathMLElementToLatexConverterAdapter.Convert(sub).Convert();
        var supLatex = MathMLElementToLatexConverterAdapter.Convert(sup).Convert();

        return $"_{{{subLatex}}}^{{{supLatex}}}";
    }

}

using MaTex.MathMLToLaTex.Helpers.Wrappers;
using System.Text.RegularExpressions;

namespace MaTex.MathMLToLaTex.ToLatexConverters;

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
        MathMLElement? sub, sup;
        if (children.Length >= 3 && IsPrescript(children[1]))
        {
            sub = children[2];
            sup = children.Length> 3 ? children[3]:null;
        }
        else if (children.Length >= 5 && IsPrescript(children[3]))
        {
            sub = children[4];
            sup = children.Length>5 ? children[5]:null;
        }
        else
            return "";

        if (sup != null)
        {
            var subLatex = MathMLElementToLatexConverterAdapter.Convert(sub).Convert();
            var supLatex = MathMLElementToLatexConverterAdapter.Convert(sup).Convert();

            return $"\\_{{{subLatex}}}^{{{supLatex}}}";
        }
        else
        {
            var subLatex = MathMLElementToLatexConverterAdapter.Convert(sub).Convert();
            return $"\\_{{{subLatex}}}^{{}}";

        }

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

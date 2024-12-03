using MathMLToLaTex.Helpers;
using MathMLToLaTex.Helpers.Wrappers;

namespace MathMLToLaTex.ToLatexConverters;

public class MFenced(MathMLElement Element) : ToLatexConverter(Element)
{
    public readonly string Open = Element.Attributes.TryGetValue("open", out var open) ? open : "";
    public readonly string Close = Element.Attributes.TryGetValue("close", out var close) ? close : "";
    public readonly string[] Seperators = (Element.Attributes.TryGetValue("seperators", out var seperators)
            ? seperators : "").ToCharArray().Select(c => c.ToString()).ToArray();

    public static bool HasAny(MathMLElement[] elements, string name)
        => elements.Any(e => e.Name == name || HasAny(e.Children, name));
    public override string Convert()
    {
        var contents =
            this.Element.Children.Select(
                child => MathMLElementToLatexConverterAdapter.Convert(child)).Select(c => c.Convert()).ToArray();
        if (HasAny(this.Element.Children, "mtable"))
        {
            var contentWithoutWrapper = Utilities.JoinWithSeperators(contents, this.Seperators);

            return new Wrapper(this.Open, this.Close).Wrap(contentWithoutWrapper);
        }
        else
        {
            var command = GetCommand();
            var contentWithoutWrapper = string.Join("", contents);
            var matrix = $"\\begin{{{command}}}\n${contentWithoutWrapper}\n\\end{{{command}}}";
            return command == GenericCommand ? new Wrapper(this.Open, this.Close).Wrap(matrix) : matrix;
        }

    }
    private static readonly string GenericCommand = "matrix";

    protected string GetCommand()
    {
        if (Are("(", ")"))
            return "pmatrix";
        else if (Are("[", "]"))
            return "bmatrix";
        else if (Are("{", "}"))
            return "Bmatrix";
        else if (Are("|", "|"))
            return "vmatrix";
        else if (Are("||", "||"))
            return "Vmatrix";
        else if (this.Open != this.Close)
            return GenericCommand;
        else
            return "bmatrix";
    }

    protected bool Are(string open, string close)
        => this.Open == open && this.Close == close;

}


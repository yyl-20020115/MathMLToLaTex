using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace MathMLToLaTex;

public static class MathMLToLaTeX
{
    private static readonly Regex LineBreaks = new ("\\n|\\r\\n|\\r");
    private static readonly Regex MsWordPrefix = new("mml:");
    public static string Convert(string mathml)
        => string.Join("", ElementsToMathMLAdapter.Convert(Parse(mathml)).Select(
                mathMLElement => MathMLElementToLatexConverterAdapter.Convert(mathMLElement))
                .Select(toLatexConverters=> toLatexConverters.Convert())).Trim()
        ;

    public static XElement[] Parse(string mathml)
    {
        var element = XElement.Parse(MsWordPrefix.Replace(LineBreaks.Replace(mathml, ""), ""));
        return (element.Name.LocalName.ToLower() == "math") ? [element]
        : element.Elements().Where(e => e.Name.LocalName.ToLower() == "math").ToArray();
    }
}

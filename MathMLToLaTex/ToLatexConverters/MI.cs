using MathMLToLaTex.Helpers;

namespace MathMLToLaTex.ToLatexConverters;

public class MI(MathMLElement Element) : ToLatexConverter(Element)
{
    public override string Convert()
    {
        var normalizedValue = Utilities.NormalizeWhiteSpaces(this.Element.Value);
        if (normalizedValue == " ") return Apply(normalizedValue);
        var trimmedValue = normalizedValue.Trim();
        var convertedChar = Apply(trimmedValue);
        var parsedChar = UTF8ToLtXConverter.Convert(convertedChar);
        if (parsedChar != convertedChar) return parsedChar;

        return WrapInMathVariant(convertedChar, 
            this.Element.Attributes.TryGetValue("mathvariant", out var v) ? v : "");
    }

    public static string Apply(string value) 
        => Dicts.MathSymbolsByChar.TryGetValue(value, out var c0) ? c0 :
        Dicts.MathSymbolsByGlyph.TryGetValue(value, out var c1) ? c1 :
        Dicts.NumberByGlyph.TryGetValue(value, out var c2) ? c2 :
        UTF8ToLtXConverter.Convert(value)
        ;

    public static string WrapInMathVariant(string value, string variant) => variant switch
    {
        "bold" => $"\\mathbf{{{value}}}",
        "italic" => $"\\mathit{{{value}}}",
        "bold-italic" => $"\\mathbf{{\\mathit{{${value}}}}}",
        "double-struck" => $"\\mathbb{{{value}}}",
        "bold-fraktur" => $"\\mathbf{{\\mathfrak{{{value}}}}}",
        "script" => $"\\mathcal{{{value}}}",
        "bold-script" => $"\\mathbf{{\\mathcal{{{value}}}}}",
        "fraktur" => $"\\mathfrak{{{value}}}",
        "sans-serif" => $"\\mathsf{{{value}}}",
        "bold-sans-serif" => $"\\mathbf{{\\mathsf{{{value}}}}}",
        "sans-serif-italic" => $"\\mathsf{{\\mathit{{{value}}}}}",
        "sans-serif-bold-italic" => $"\\mathbf{{\\mathsf{{\\mathit{{{value}}} }} }}",
        "monospace" => $"\\mathtt{{{value}}}",
        _ => value,
    };
}

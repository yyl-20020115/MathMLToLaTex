namespace MathMLToLaTex.ToLatexConverters;

public class MI(MathMLElement Element) : ToLatexConverter(Element)
{
    public override string Convert() => "";

    public static string WrapInMathVariant(string value, string mathVariant) => mathVariant switch
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

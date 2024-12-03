namespace MathMLToLaTex.ToLatexConverters;

public class MText(MathMLElement Element) : ToLatexConverter(Element)
{
    public static string[] GetCommand(string _mathvariant) 
        => (string.IsNullOrEmpty(_mathvariant) ? "normal" : _mathvariant) switch
    {
        "bold" => ["\\textbf"],
        "italic" => ["\\textit"],
        "bold-italic" => ["\\textit", "\\textbf"],
        "double-struck" => ["\\mathbb"],
        "monospace" => ["\\mathtt"],
        "bold-fraktur" or "fraktur" => ["\\mathfrak"],
        _ => ["\\text"],
    };

    public override string Convert() => "";


}

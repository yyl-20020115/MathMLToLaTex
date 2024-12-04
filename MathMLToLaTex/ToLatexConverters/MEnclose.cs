namespace MathMLToLaTex.ToLatexConverters;

public class MEnclose(MathMLElement Element) : ToLatexConverter(Element)
{
    public string Notation => this.Element.Attributes.TryGetValue("notation", out var v) ? v : "longdiv";

    public static readonly string[] Shapes = ["box", "roundedbox", "circle"];
    public static readonly string[] Strikes = ["verticalstrike", "horizontalstrike"];
    public override string Convert()
    {
        var content = base.Convert();
        var notation = this.Notation;
        if (Shapes.Contains(notation)) 
            return $"\\boxed{{{content}}}";
        else if (Strikes.Contains(notation)) 
            return $"\\hcancel{{{content}}}";
        else 
            return notation switch
        {
            "actuarial" => $"\\overline{{\\left.{content}\\right |}}",
            "radical" => $"\\sqrt{{{content}}}",
            "left" => $"\\left |{content}",
            "right" => $"{content}\\right |",
            "top" => $"\\overline {{{content}}}",
            "bottom" => $"\\underline {{{content}}}",
            "updiagonalstrike" => $"\\cancel{{{content}}}",
            "downdiagonalstrike" => $"\\bcancel{{{content}}}",
            "updiagonalarrow" => $"\\cancelto{{}}{{{content}}}",
            "madruwb" => $"\\underline{{{content}\\right |}}",
            "phasorangle" => $"{{\\angle \\underline{{{content}}}}}",
            _ => $"\\overline{{\\left.\\right){content}}}",
        };
    }
}

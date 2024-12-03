using System.Text;

namespace MathMLToLaTex.ToLatexConverters;

public class MText(MathMLElement Element) : ToLatexConverter(Element)
{
    public static string[] GetCommands(string variant)
        => string.IsNullOrEmpty(variant) ? ["normal"] : (variant switch
        {
            "bold" => ["\\textbf"],
            "italic" => ["\\textit"],
            "bold-italic" => ["\\textit", "\\textbf"],
            "double-struck" => ["\\mathbb"],
            "monospace" => ["\\mathtt"],
            "bold-fraktur" or "fraktur" => ["\\mathfrak"],
            _ => ["\\text"],
        });

    public static string Apply(string value, string variant)
    {
        var builder = new StringBuilder();
        var cmds = GetCommands(variant);
        for( var i = 0;i<cmds.Length;i++)
        {
            builder.Append(i == 0 ? $"{cmds[i]}{{{value}}}" : $"{cmds[i]}{{{builder}}}");
        }
        return builder.ToString();
    }


    private class Block
    {
        public string Text = "";
        public bool IsAlpha = true;
    }
    public override string Convert()
    {
        var variant = this.Element.Attributes.TryGetValue("mathvariant", out var v) ? v : "";
        var builder = new StringBuilder();
        var value = this.Element.Value;

        var blocks = value.ToCharArray().Select(c => new Block { Text = c.ToString(), IsAlpha = char.IsLetterOrDigit(c) || c == ' ' }).ToArray();
        List<Block> results = [];
        if (blocks.Length > 0)
        {
            var last = blocks[0];
            for (var i = 1; i < blocks.Length; i++)
            {
                var current = blocks[i];
                if(current.IsAlpha && last.IsAlpha)
                {
                    last.Text += current.Text;
                }
                else
                {
                    results.Add(last);
                    last = current;
                }
            }
        }
        results.ForEach(block=>
        {
            builder.Append(Apply(block.IsAlpha?block.Text: new MI(new MathMLElement("", block.Text)).Convert(), variant));
        });

        return builder.ToString();
    }
}

namespace MathMLToLaTex.ToLatexConverters;

public class MTable : ToLatexConverter
{
    public MTable(MathMLElement Element) : base(Element)
    {
        AddFlagsRecursively(Element.Children, "mtable", "innerTable");
    }
    public static void AddFlagsRecursively(MathMLElement[] Elements, string name, string flag)
    {
        foreach (var item in Elements)
        {
            if (item.Name == name)
                item.Attributes[flag] = flag;
            AddFlagsRecursively(item.Children, name, flag);
        }
    }
    public bool HasFlag(string flag) => this.Element.Attributes.ContainsKey(flag);
    public static string Wrap(string latex) => $"\\begin{{matrix}}{latex}\\end{{matrix}}";
    public override string Convert()
    {
        var content = string.Join(" \\\\\n",
            this.Element.Children.Select(
                child => MathMLElementToLatexConverterAdapter.Convert(child)).Select(c => c.Convert()));

        return this.HasFlag("innerTable") ? Wrap(content) : content;
    }

}

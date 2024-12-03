namespace MathMLToLaTex;

public class MathMLElement(string Name = "Void", string Value = "")
{
    public static readonly MathMLElement Default = new();
    public string Name = Name;
    public string Value = Value;
    public MathMLElement[] Children = [];
    public Dictionary<string, string> Attributes = [];
}

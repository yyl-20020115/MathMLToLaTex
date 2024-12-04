namespace MathMLToLaTex.Helpers.Wrappers;

public class ParenthesisWrapper : Wrapper
{
    public ParenthesisWrapper() : base("\\left(", "\\right)") { }
    public string WrapIfMore(string text) => text.Length <= 1 ? text : Wrap(text);
}

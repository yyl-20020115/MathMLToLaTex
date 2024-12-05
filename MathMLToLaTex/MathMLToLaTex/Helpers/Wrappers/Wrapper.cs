namespace MaTex.MathMLToLaTex.Helpers.Wrappers;

public class Wrapper(string Open, string Close)
{
    public readonly string Open = Open;
    public readonly string Close = Close;
    public virtual string Wrap(string text) => Open + text + Close;
}

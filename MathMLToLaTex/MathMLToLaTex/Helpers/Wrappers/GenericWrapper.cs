namespace MaTex.MathMLToLaTex.Helpers.Wrappers;

public class GenericWrapper(string Open, string Close) : Wrapper("\\left"+ Open, "\\right"+ Close)
{
}

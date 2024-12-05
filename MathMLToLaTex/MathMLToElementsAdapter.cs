using System.Xml.Linq;

namespace MaTex.MathMLToLaTex;

public static class MathMLToElementsAdapter
{
    public static XElement Convert(MathMLElement e)
    {
        var x = new XElement(e.Name)
        {
            Value = e.Children.Length > 0 ? "" : e.Value,
        };
        foreach (var attribute in e.Attributes)
        {
            x.SetAttributeValue(attribute.Key, attribute.Value);
        }
        x.Add(Convert(e.Children));
        return x;
    }
    public static XElement[] Convert(MathMLElement[] elements)
        => [.. elements.Where(e => !string.IsNullOrEmpty(e.Name)).Select(e => Convert(e))];

}

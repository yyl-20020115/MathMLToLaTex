using System.Xml.Linq;

namespace MaTex.MathMLToLaTex;

public static class ElementsToMathMLAdapter
{
    public static Dictionary<string,string> GetAttributes(XElement element)
    {
        var ats = element.Attributes().Select(a => new KeyValuePair<string, string>(a.Name.LocalName, a.Value));
        var dict = new Dictionary<string, string>();
        foreach(var a in ats)
        {
            dict.Add(a.Key, a.Value);
        }
        return dict;
    }
    public static MathMLElement[] Convert(XElement[] elements)
        => [..elements.Where(e=>!string.IsNullOrEmpty(e.Name.LocalName)).Select(e=>new MathMLElement()
        {
            Name = e.Name.LocalName,
            Value = e.HasElements?"":e.Value,
            Attributes = GetAttributes(e),
            Children = Convert([..e.Elements()])

        })];
}

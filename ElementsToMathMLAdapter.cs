using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MathMLToLaTex;

public static class ElementsToMathMLAdapter
{
    public static MathMLElement[] Convert(XElement[] elements)
        => [..elements.Where(e=>!string.IsNullOrEmpty(e.Name.LocalName)).Select(e=>new MathMLElement()
        {
            Name = e.Name.LocalName,
            Value = e.HasElements?"":e.Value,
            Attributes = e.Attributes().Select(a => new KeyValuePair<string,string>(a.Name.LocalName, a.Value)).ToDictionary(),
            Children = Convert([..e.Elements()])

        })];
}

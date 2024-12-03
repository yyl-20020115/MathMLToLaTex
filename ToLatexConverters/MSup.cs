using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathMLToLaTex.ToLatexConverters;

public class MSup(MathMLElement Element) : ToLatexConverter(Element)
{
    public override string Convert() => "";
}

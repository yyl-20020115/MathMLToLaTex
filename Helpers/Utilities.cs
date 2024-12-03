using System.Text;
using System.Text.RegularExpressions;

namespace MathMLToLaTex.Helpers;

public static class Utilities
{
    public static string JoinWithSeperators(string[] parts, string[] seperators)
    {
        var builder = new StringBuilder();
        for (int i = 0; i < parts.Length; i++)
        {
            builder.Append(parts[i]);
            if(i == parts.Length - 1)
            {
                builder.Append(string.Empty);
            }
            else if (i < seperators.Length)
            {
                builder.Append(seperators[i]);
            }else if(seperators.Length > 0)
            {
                builder.Append(seperators[^1]);
            }
            else
            {
                builder.Append(',');
            }
        }
        return builder.ToString();
    }

    private static readonly Regex SpacesRegex = new Regex("\\s+");
    public static string NormalizeWhiteSpaces(string text)
    {
        if (string.IsNullOrEmpty(text)) return text;
        text = SpacesRegex.Replace(text, " ");
        return text;
    }

    //<img alt="\frac{1}{2}" data-cke-saved-src="https://latex.csdn.net/eq?%5Cfrac%7B1%7D%7B2%7D" src="https://latex.csdn.net/eq?%5Cfrac%7B1%7D%7B2%7D" class="mathcode">
    public static string UrlEncode(string str)
    {
        var builder = new StringBuilder();
        var bytes = Encoding.UTF8.GetBytes(str);
        for (int i = 0; i < bytes.Length; i++)
        {
            builder.Append($"%{Convert.ToString(bytes[i], 16)}");
        }

        return builder.ToString();
    }
    public static string MakeCsdnURL(string equation) => $"https://latex.csdn.net/eq?" + UrlEncode(equation);
    public static string MakeCsdnImageTag(string latex, string equation)
    {
        var url = MakeCsdnURL(equation);
        return $"<img alt=\"{latex}\" data-cke-saved-src=\"{url}\" src=\"{url}\" class=\"mathcode\">";
    }
}

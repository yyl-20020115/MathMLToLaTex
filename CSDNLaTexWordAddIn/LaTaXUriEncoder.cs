using System.Text;

namespace CSDNLaTexWordAddIn;

public static class LaTaXUriEncoder
{
    public static string Encode(string latex)
    {
        var builder = new StringBuilder();
        for (int i = 0; i < latex.Length; i++)
        {
            char c = latex[i];
            if (char.IsLetterOrDigit(c))
            {
                builder.Append(c);
            }
            else
            {
                switch (c)
                {
                    case '_':
                    case '/':
                        builder.Append(c);
                        break;
                    //case '+':
                    //    builder.Append("&plus;");
                    //    break;
                    case < (char)0xff:
                        builder.Append($"%{(int)c:X2}");
                        break;
                    case >= (char)0x100:
                        builder.Append($"\\u{(int)c:X4}");
                        break;
                    default:
                        builder.Append(c);
                        break;
                }
            }
        }
        return builder.ToString();
    }
}

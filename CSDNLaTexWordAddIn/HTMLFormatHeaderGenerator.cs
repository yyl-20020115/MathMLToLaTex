using System;
using System.Text;

namespace CSDNLaTexWordAddIn
{
    public class HTMLFormatHeaderGenerator
    {
        public static readonly byte[] StartFragmentTagBytes
            = Encoding.UTF8.GetBytes(HTMLFormatHeaderParser.StartFragmentTag)
            ;
        public static readonly byte[] EndFragmentTagBytes
            = Encoding.UTF8.GetBytes(HTMLFormatHeaderParser.EndFragmentTag)
            ;

        public HTMLFormatHeaderGenerator() { }
        public static int IndexOf(byte[] array, byte[] toFind)
        {
            if (array.Length == 0 || toFind.Length == 0 || array.Length < toFind.Length) return -1;

            int p = 0;
            for (int i = 0; i < array.Length - toFind.Length + 1; i++)
            {
                p = Array.IndexOf(array, toFind[0], p);
                if (p >= 0)
                {
                    var failed = false;
                    for (int j = p, k = 0; j < array.Length && k < toFind.Length; j++, k++)
                    {
                        if (array[j] != toFind[k])
                        {
                            failed = true;
                            break;
                        }
                    }
                    if (!failed)
                        return p;
                    p++;
                }
                else
                {
                    break;
                }
            }
            return -1;
        }
        public string Generate(HTMLFormatInfo info, int? start_selection = null, int? end_selection = null)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"{HTMLFormatInfo.VersionTitle}{info.Version}");
            builder.AppendLine($"{HTMLFormatInfo.StartHTMLTitle}{info.StartHTML:D10}");
            builder.AppendLine($"{HTMLFormatInfo.EndHTMLTitle}{info.EndHTML:D10}");
            builder.AppendLine($"{HTMLFormatInfo.StartFragmentTitle}{info.StartFragment:D10}");
            builder.AppendLine($"{HTMLFormatInfo.EndFragmentTitle}{info.EndFragment:D10}");

            var epi = builder.Length;
            if (start_selection != null && end_selection != null)
            {
                builder.AppendLine($"{HTMLFormatInfo.StartSelectionTitle}{info.StartSelection = 0:D10}");
                builder.AppendLine($"{HTMLFormatInfo.EndSelectionTitle}{info.EndSelection = 0:D10}");
            }
            var current = Encoding.UTF8.GetBytes(builder.ToString());
            var pre = current.Length;
            builder.AppendLine(info.HTML);
            current = Encoding.UTF8.GetBytes(builder.ToString());
            var post = current.Length - 1;

            var start_pos = IndexOf(current, StartFragmentTagBytes) 
                + HTMLFormatHeaderParser.StartFragmentTag.Length;
            var end_pos = IndexOf(current, EndFragmentTagBytes) - 1;

            builder.Remove(0, epi);

            var header = new StringBuilder();
            header.AppendLine($"{HTMLFormatInfo.VersionTitle}{info.Version}");
            header.AppendLine($"{HTMLFormatInfo.StartHTMLTitle}{pre:D10}");
            header.AppendLine($"{HTMLFormatInfo.EndHTMLTitle}{post:D10}");
            header.AppendLine($"{HTMLFormatInfo.StartFragmentTitle}{start_pos:D10}");
            header.AppendLine($"{HTMLFormatInfo.EndFragmentTitle}{end_pos:D10}");
            if (start_selection != null && end_selection != null)
            {
                header.AppendLine($"{HTMLFormatInfo.StartSelectionTitle}{info.StartSelection = start_selection + epi:D10}");
                header.AppendLine($"{HTMLFormatInfo.EndSelectionTitle}{info.EndSelection = end_selection + epi:D10}");
            }
            builder.Insert(0, header.ToString());
            return builder.ToString();
        }

        public byte[] GenerateBytes(HTMLFormatInfo info, int? start_selection = null, int? end_selection = null)
            => Encoding.UTF8.GetBytes(Generate(info, start_selection, end_selection));
    }
}

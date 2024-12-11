#nullable enable
using System;
using System.IO;
using System.Text;

namespace CSDNLaTexWordAddIn
{
    /// <summary>
    ///<cf-html>                ::= <description-header> <context>
    ///<context>                ::= [<preceding-context>] <fragment> [<trailing-context>]
    ///<description-header>     ::= "Version:" <version> <br> ( <header-offset-keyword> ":" <header-offset-value> <br> )*
    ///<header-offset-keyword>  ::= "StartHTML" | "EndHTML" | "StartFragment" | "EndFragment" | "StartSelection" | "EndSelection"
    ///<header-offset-value>    ::= { Base 10 (decimal) integer string with optional _multiple_ leading zero digits(see "Offset syntax" below) }
    ///< version >                ::= "0.9" | "1.0"
    ///< fragment >               ::= < fragment - start - comment > < fragment - text > < fragment - end - comment >
    ///< fragment - start - comment > ::= "<!--StartFragment -->"
    ///< fragment - end - comment >   ::= "<!--EndFragment -->"
    ///< preceding - context >      ::= { Arbitrary HTML }
    ///< trailing - context >       ::= { Arbitrary HTML }
    ///< fragment - text >          ::= { Arbitrary HTML }
    ///< br >                     ::= "\r" | "\n" | "\r\n"
    /// </summary>
    public class HTMLFormatInfo
    {
        public const string VersionTitle = "Version:";
        public const string StartHTMLTitle = "StartHTML:";
        public const string EndHTMLTitle = "EndHTML:";
        public const string StartFragmentTitle = "StartFragment:";
        public const string EndFragmentTitle = "EndFragment:";
        public const string StartSelectionTitle = "StartSelection:";
        public const string EndSelectionTitle = "EndSelection:";
        public const string SourceURLTitle = "SourceURL:";

        public string Version = "";
        public long StartHTML = -1;
        public long EndHTML = -1;
        public long StartFragment = 0;
        public long EndFragment = 0;
        public long? StartSelection = null;
        public long? EndSelection = null;
        public string? SourceURL = null;
        public string? HTML = null;
        public string? Fragment = null;
        public string? Selection = null;
        public HTMLFormatInfo() { }
    }
    public class HTMLFormatHeaderParser()
    {
        public const string StartFragmentTag = "<!--StartFragment-->";
        public const string EndFragmentTag = "<!--EndFragment-->";

        public HTMLFormatInfo Parse(byte[] bytes)
        {
            var text = Encoding.UTF8.GetString(bytes);
            using var reader = new StringReader(text);
            var info = new HTMLFormatInfo();
            string line;
            while (null != (line = reader.ReadLine()))
            {
                line = line.Trim();
                if (line.Length == 0)
                {
                    break;
                }
                else if (line.StartsWith(HTMLFormatInfo.VersionTitle))
                {
                    info.Version = line.Substring(HTMLFormatInfo.VersionTitle.Length);
                }
                else if (line.StartsWith(HTMLFormatInfo.StartHTMLTitle))
                {
                    info.StartHTML = long.TryParse(line.Substring(HTMLFormatInfo.StartHTMLTitle.Length), out var v) ? v : -1;
                }
                else if (line.StartsWith(HTMLFormatInfo.EndHTMLTitle))
                {
                    info.EndHTML = long.TryParse(line.Substring(HTMLFormatInfo.EndHTMLTitle.Length), out var v) ? v : -1;
                }
                else if (line.StartsWith(HTMLFormatInfo.StartFragmentTitle))
                {
                    info.StartFragment = long.TryParse(line.Substring(HTMLFormatInfo.StartFragmentTitle.Length), out var v) ? v : -1;
                }
                else if (line.StartsWith(HTMLFormatInfo.EndFragmentTitle))
                {
                    info.EndFragment = long.TryParse(line.Substring(HTMLFormatInfo.EndFragmentTitle.Length), out var v) ? v : -1;
                }
                else if (line.StartsWith(HTMLFormatInfo.StartSelectionTitle))
                {
                    info.StartSelection = long.TryParse(line.Substring(HTMLFormatInfo.StartSelectionTitle.Length), out var v) ? v : -1;
                }
                else if (line.StartsWith(HTMLFormatInfo.EndSelectionTitle))
                {
                    info.EndSelection = long.TryParse(line.Substring(HTMLFormatInfo.EndSelectionTitle.Length), out var v) ? v : -1;
                }
                else if (line.StartsWith(HTMLFormatInfo.SourceURLTitle))
                {
                    info.SourceURL = line.Substring(HTMLFormatInfo.SourceURLTitle.Length);
                }
            }

            if (info.StartHTML >= 0 && info.EndHTML >= 0 && info.StartHTML <= info.EndHTML && info.EndHTML <= bytes.Length)
            {
                var length = info.EndHTML - info.StartHTML;
                var buffer = new byte[length];
                Array.Copy(bytes, info.StartHTML, buffer, 0, length);
                info.HTML = Encoding.UTF8.GetString(buffer);
            }
            if (info.StartFragment >= 0 && info.EndFragment >= 0 && info.StartFragment <= info.EndFragment && info.EndFragment <= bytes.Length)
            {
                var sp = info.StartFragment - StartFragmentTag.Length;
                var ep = info.EndFragment;
                if (sp >= 0 && sp <= ep && ep + EndFragmentTag.Length <= bytes.Length)
                {
                    var buffer = new byte[StartFragmentTag.Length];
                    Array.Copy(bytes, sp, buffer, 0, StartFragmentTag.Length);
                    var sf = Encoding.UTF8.GetString(buffer).Trim();

                    buffer = new byte[EndFragmentTag.Length];
                    Array.Copy(bytes, ep, buffer, 0, EndFragmentTag.Length);
                    var ef = Encoding.UTF8.GetString(buffer).Trim();

                    if (sf.Equals(StartFragmentTag, StringComparison.OrdinalIgnoreCase) 
                     && ef.Equals(EndFragmentTag, StringComparison.OrdinalIgnoreCase))
                    {
                        var length = info.EndFragment - info.StartFragment;
                        buffer = new byte[length];
                        Array.Copy(bytes, info.StartFragment, buffer, 0, length);
                        info.Fragment = Encoding.UTF8.GetString(buffer);
                    }
                }
            }
            if (info.StartSelection != null
                && info.EndSelection != null
                && info.StartSelection >= 0
                && info.EndSelection >= 0
                && info.StartSelection.Value <= info.EndSelection.Value
                && info.EndSelection.Value <= bytes.Length)
            {
                var length = info.EndSelection.Value - info.StartSelection.Value;
                var buffer = new byte[length];
                Array.Copy(bytes, info.StartSelection.Value, buffer, 0, length);
                info.Selection = Encoding.UTF8.GetString(buffer);
            }

            return info;
        }
    }
}

using Microsoft.Office.Tools.Ribbon;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;

namespace CSDNLaTexWordAddIn
{
    public partial class CSDNRibbon
    {
        public class XCommentEx(string value) : XComment(value)
        {
            public override void WriteTo(XmlWriter writer)
                => writer.WriteRaw(Value);
        }
        protected XslCompiledTransform xslOfficeMathMLToMathML = new();
        protected XslCompiledTransform xslMathMLToOfficeMathML = new();

        protected const string m_ns = "http://schemas.openxmlformats.org/officeDocument/2006/math";
        protected const string mml_ns = "http://www.w3.org/1998/Math/MathML";
        protected const string Entities =
            """
            <!DOCTYPE inline_dtd [
            <!ENTITY nbsp " ">
            <!ENTITY copy "©">
            <!ENTITY reg "®">
            <!ENTITY trade "™">
            <!ENTITY mdash "—">
            <!ENTITY ldquo "“">
            <!ENTITY rdquo "”"> 
            <!ENTITY pound "£">
            <!ENTITY yen "¥">
            <!ENTITY euro "€">
            ]>
            """;
        protected string csdn_url_prefix = "https://latex.csdn.net/eq?";

        protected string xmlns_omml = "http://schemas.microsoft.com/office/2004/12/omml";
        protected string xmlns_mml = "http://schemas.openxmlformats.org/officeDocument/2006/math";

        protected string TransformOfficeMathMLToMathML(string text)
        {
            using var textreader = new StringReader(text);
            using var textwriter = new StringWriter();
            using var reader = new XmlTextReader(textreader);
            using var writer = new XmlTextWriter(textwriter);
            xslOfficeMathMLToMathML.Transform(reader, writer);
            return textwriter.ToString();
        }
        protected string TransformMathMLToOfficeMathML(string text)
        {
            using var textreader = new StringReader(text);
            using var textwriter = new StringWriter();
            using var reader = new XmlTextReader(textreader);
            using var writer = new XmlTextWriter(textwriter);
            xslMathMLToOfficeMathML.Transform(reader, writer);
            return textwriter.ToString();
        }

        protected bool IsOMath(XElement element)
            => element.Name.Namespace.NamespaceName
            == "http://schemas.openxmlformats.org/officeDocument/2006/math"
            ;

        protected XElement PurifyOMath(XElement element)
        {
            List<XElement> selected = [];
            var name = element.Name;
            if (!IsOMath(element))
            {
                selected.AddRange(element.Elements());
            }
            else if (element.HasElements)
            {
                foreach (var child in element.Elements().ToArray())
                {
                    if (IsOMath(child))
                    {
                        selected.Add(child);
                    }
                    else
                    {
                        selected.AddRange(child.Elements());
                    }
                }
            }
            else
            {
                //if <m:r> without <m:t>, add it
                if (element.Name.LocalName == "r")
                {
                    var v = element.Value.Trim();
                    element.RemoveAll();
                    element.Add(new XElement(XName.Get("t", name.NamespaceName), v));
                }
            }
            List<XElement> results = [];
            foreach (var other in selected)
            {
                results.Add(this.PurifyOMath(other));
            }

            if (results.Count == 1 && !IsOMath(element))
            {
                element = results[0];
            }
            else if (results.Count > 0)
            {
                var others = element.Elements().ToArray();
                element.Add([.. results]);
                foreach (var p in others)
                    p.Remove();
            }
            return element;
        }

        protected virtual string TranslateToImageTag(string url_prefix, string latex)
            => $"<img alt=\"{latex}\" src=\"{url_prefix}{LaTaXUriEncoder.Encode(latex)}\" class=\"mathcode\"/>";
        private void CSDNRibbon_Load(object sender, RibbonUIEventArgs e)
        {
            xslOfficeMathMLToMathML.Load("OMML2MML.XSL");
            xslMathMLToOfficeMathML.Load("MML2OMML.XSL");
        }
        private void ButtonCopy_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                var selection = Globals.ThisAddIn?.Application?.Selection;
                if (selection != null)
                {
                    Clipboard.Clear();
                    selection.Copy();

                    var bytes = ClipboardData.GetHtmlBytes();
                    if (bytes == null) return;
                    Clipboard.Clear();

                    var info = new HTMLFormatHeaderParser().Parse(bytes);

                    HTMLFormatHeaderGenerator generator = new();

                    var doc_fragment = NSoup.NSoupClient.Parse(info.HTML);

                    var document_text = doc_fragment.ToString();

                    document_text = Entities + document_text;
                    using var reader = new StringReader(document_text);
                    var html = XDocument.Load(reader);
                    var processing = false;
                    var vml_img = false;
                    foreach (var node in html.DescendantNodes().ToArray())
                    {
                        var text = node.ToString();
                        var sc = text.Trim();
                        var is_comment = node.NodeType == XmlNodeType.Comment;
                        if (is_comment)
                        {
                            if (sc == HTMLFormatHeaderParser.StartFragmentTag)
                                processing = true;
                        }
                        if (processing)
                        {
                            if (node is XComment comment)
                            {
                                if (text.StartsWith("<!--[if gte msEquation") && text.EndsWith("<![endif]-->"))
                                {
                                    //find open bracket ending
                                    var p = text.IndexOf('>');
                                    if (p >= 0)
                                    {
                                        try
                                        {
                                            var oml = text.Substring(p + 1, text.Length - "<![endif]-->".Length - p - 1);
                                            
                                            var h = NSoup.NSoupClient.ParseBodyFragment(oml);
                                            var o = h?.Body?.GetChildNode(0);

                                            o?.Attributes?.Add("xmlns:m", m_ns);
                                            o?.Attributes?.Add("xmlns:mml", mml_ns);

                                            oml = PurifyOMath(XElement.Parse(o?.ToString() ?? ""))?.ToString() ?? "";

                                            var mathml = TransformOfficeMathMLToMathML(oml);

                                            var latex = MaTex.MathMLToLaTex.MathMLToLaTeXConverter.Convert(mathml);

                                            var tag = TranslateToImageTag(csdn_url_prefix, latex);

                                            node.ReplaceWith(new XCommentEx(tag));
                                        }
                                        catch (System.Exception ex)
                                        {

                                        }
                                    }
                                }
                                else if (text.StartsWith("<!--[if gte vml") && text.EndsWith("<![endif]-->"))
                                {
                                    node.Remove();
                                }
                                else if (text == "<!--[if !vml]-->")
                                    vml_img = true;
                                else if (vml_img && text.EndsWith("<!--[endif]-->"))
                                    vml_img = false;
                            }
                            else if (vml_img)
                            {
                                node.Remove();
                            }
                        }
                        if (is_comment) //comment switch
                        {
                            if (sc == HTMLFormatHeaderParser.EndFragmentTag)
                                processing = false;
                        }
                    }
                    var result = html.ToString();
                    int start_pos = result.IndexOf(HTMLFormatHeaderParser.StartFragmentTag);
                    int end_pos = result.IndexOf(HTMLFormatHeaderParser.EndFragmentTag);
                    int ws_length = 0;
                    while (end_pos >= 1 && char.IsWhiteSpace(result[end_pos - 1]))
                    {
                        ws_length++;
                        end_pos--;
                    }
                    if (ws_length > 0)
                    {
                        var builder = new StringBuilder(result);
                        builder.Remove(end_pos, ws_length);
                        result = builder.ToString();
                    }
                    int sp = result.IndexOf("]>");
                    if (sp >= 0)
                    {
                        info.HTML = result.Substring(sp + "]>".Length).Trim();
                        info.Fragment = "";
                        ClipboardData.SetHTMLFormatTextToClipboard(
                             generator.GenerateBytes(info)
                             );
                    }
                }
            }
            catch (System.Exception ex)
            {

            }
        }
    }
}

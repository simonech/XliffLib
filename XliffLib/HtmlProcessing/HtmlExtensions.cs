using System;
using System.Linq;
using HtmlAgilityPack;

namespace XliffLib.HtmlProcessing
{
    public static class HtmlExtensions
    {
        #pragma warning disable IDE1006 // Naming Styles
        private static string[] INLINECODE = {
            "cp","ph","pc","sc","ec","mrk","sm","em"
        };

        public static bool IsHtml(this string text)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(text);
            return !doc.DocumentNode.ChildNodes.All(n => IsTextOrXliff(n));
        }

        private static bool IsTextOrXliff(HtmlNode n)
        {
            if (n.NodeType == HtmlNodeType.Text) return true;
            if (n.NodeType == HtmlNodeType.Element)
            {
                if (INLINECODE.Contains(n.Name))
                {
                    // Unfortunately both HTML and Xliff have an <em> element
                    // so if the <em> tag also has an attribute startRef it's an Xliff
                    if (n.Name.Equals("em") && n.Attributes["startRef"] == null)
                    {
                        return false;
                    }
                    return true;
                }
            }
            return false;
        }
    }
}

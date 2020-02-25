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

        private static string[] HTML_INLINE_ELEMENTS = {
            "a","abbr","acronym","audio","b","bdi","bdo","big","br",
            "button","canvas","cite","code","data","datalist","del",
            "dfn","em","embed","i","iframe","img","input","ins","kbd",
            "label","map","mark","meter","noscript","object","output",
            "picture","progress","q","ruby","s","samp","script","select",
            "slot","small","span","strong","sub","sup","svg","template",
            "textarea","time","u","tt","var","video","wbr"
        };

        public static bool IsHtml(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return false;
            var doc = new HtmlDocument();
            doc.LoadHtml(text);
            return !doc.DocumentNode.ChildNodes.All(n => IsTextOrXliff(n));
        }

        private static bool IsInlineElement(HtmlNode node)
        {
            return HTML_INLINE_ELEMENTS.Contains(node.Name.ToLowerInvariant());
        }

        public static bool IsTextOrInline(this HtmlNode node)
        {
            return node.NodeType == HtmlNodeType.Element && IsInlineElement(node)
                    || node.NodeType == HtmlNodeType.Text && !string.IsNullOrWhiteSpace(node.InnerText);
        }

        public static bool IsBlock(this HtmlNode node)
        {
            return node.NodeType == HtmlNodeType.Element && !IsInlineElement(node);
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

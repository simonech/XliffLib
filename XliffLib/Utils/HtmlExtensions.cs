using System;
using System.Linq;
using HtmlAgilityPack;

namespace XliffLib.Utils
{
    public static class HtmlExtensions
    {
#pragma warning disable IDE1006 // Naming Styles
        private static string[] INLINECODE = {
            "cp","ph","pc","sc","ec","mrk","sm","em"
        };
#pragma warning restore IDE1006 // Naming Styles

        public static bool IsHtml(this string text)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(text);
            return !doc.DocumentNode.ChildNodes.All(n=>IsTextOrXliff(n));
        }

        public static String[] SplitByParagraphs(this string htmlText){
            return htmlText.SplitByTags("p");
        }

        public static String[] SplitByTags(this string htmlText, params string[] tags)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlText);
            return doc.DocumentNode.ChildNodes.Where(e => tags.Contains(e.Name)).Select(e => e.OuterHtml).ToArray();
        }

        public static string RemoveContainingTag(this string htmlText)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlText);
            return doc.DocumentNode.ChildNodes[0].InnerHtml;
        }

        private static bool IsTextOrXliff(HtmlNode n)
        {
            if (n.NodeType == HtmlNodeType.Text) return true;
            if(n.NodeType==HtmlNodeType.Element)
            {
                if (INLINECODE.Contains(n.Name))
                    return true;
            }
            return false;
        }
    }
}

using System;
using System.Linq;
using HtmlAgilityPack;

namespace XliffLib.Utils
{
    public static class HtmlExtensions
    {
        private static string[] INLINECODE = {
            "cp","ph","pc","sc","ec","mrk","sm","em"
        };

        public static bool IsHtml(this string text)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(text);
            return !doc.DocumentNode.ChildNodes.All(n=>IsTextOrXliff(n));
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

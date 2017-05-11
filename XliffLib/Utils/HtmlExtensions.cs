using System;
using System.Linq;
using HtmlAgilityPack;

namespace XliffLib.Utils
{
    public static class HtmlExtensions
    {
        public static bool IsHtml(this string text)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(text);
            return !doc.DocumentNode.ChildNodes.All(n=>n.NodeType==HtmlNodeType.Text);
        }
    }
}

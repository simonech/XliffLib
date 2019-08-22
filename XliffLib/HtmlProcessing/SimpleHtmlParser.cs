using System;
using System.Linq;
using HtmlAgilityPack;

namespace XliffLib.HtmlProcessing
{
    public class SimpleHtmlParser: BaseHtmlParser
    {
        private static string[] HTMLTAGSTOSPLIT = {
            "p","ul","ol","li","h1","h2","h3","h4","blockquote"
        };

        public override SimplifiedHtmlContentItem[] SplitHtml(string text)
        {
            return SplitByTags(text, HTMLTAGSTOSPLIT);
        }

        public SimplifiedHtmlContentItem[] SplitByParagraphs(string text)
        {
            if (text.IsHtml())
                return SplitByTags(text,"p");
            else
                return SplitPlainText(text);
        }

        public SimplifiedHtmlContentItem[] SplitByTags(string htmlText, params string[] tags)
        {
            if (!htmlText.IsHtml())
                throw new InvalidOperationException(@"The text supplied is not HTML: {htmlText}");
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlText);
            return doc.DocumentNode.ChildNodes.Where(e => tags.Contains(e.Name)).Select(e => new SimplifiedHtmlContentItem() { Name = e.Name, Content = e.InnerHtml }).ToArray();
        }
    }
}

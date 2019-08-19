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

        public override String[] SplitByDefaultTags(string text)
        {
            return SplitByTags(text, HTMLTAGSTOSPLIT);
        }

        public override String[] SplitPlainText(string text)
        {
            if (text.IsHtml())
                throw new InvalidOperationException(@"The text supplied is not plain text: {text}");
            else
                return text.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        }

        public String[] SplitByParagraphs(string text)
        {
            if (text.IsHtml())
                return SplitByTags(text,"p");
            else
                return SplitPlainText(text);
        }

        public String[] SplitByTags(string htmlText, params string[] tags)
        {
            if (!htmlText.IsHtml())
                throw new InvalidOperationException(@"The text supplied is not HTML: {htmlText}");
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlText);
            return doc.DocumentNode.ChildNodes.Where(e => tags.Contains(e.Name)).Select(e => e.OuterHtml).ToArray();
        }





    }
}

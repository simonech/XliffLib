using HtmlAgilityPack;

namespace XliffLib.HtmlProcessing
{
    public abstract class BaseHtmlParser: IHtmlParser
    {
        public abstract string[] SplitByDefaultTags(string text);
        public abstract string[] SplitPlainText(string text);

        public string RemoveContainingTag(string htmlText)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlText);
            return doc.DocumentNode.ChildNodes[0].InnerHtml;
        }

        public string GetContainingTag(string htmlText)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlText);
            return doc.DocumentNode.ChildNodes[0].Name;
        }

        public string ToXliffHtmlType(string htmlTag)
        {
            return "html:" + htmlTag;
        }

        public string FromXliffHtmlType(string xliffType)
        {
            return xliffType.Substring(5);
        }


    }
}
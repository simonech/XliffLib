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

        private static string[] HTMLTAGSTOSPLIT = {
            "p","ul","ol","li","h1","h2","h3","h4","blockquote"
        };


        public static bool IsHtml(this string text)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(text);
            return !doc.DocumentNode.ChildNodes.All(n => IsTextOrXliff(n));
        }

        public static String[] SplitByParagraphs(this string text)
        {
            if (IsHtml(text))
                return text.SplitByTags("p");
            else
                return text.SplitPlainText();
        }

        public static String[] SplitPlainText(this string text)
        {
            if (IsHtml(text))
                throw new InvalidOperationException(@"The text supplied is not plain text: {text}");
            else
                return text.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static String[] SplitByDefaultTags(this string text)
        {
            return text.SplitByTags(HTMLTAGSTOSPLIT);
        }

        public static String[] SplitByTags(this string htmlText, params string[] tags)
        {
            if(!IsHtml(htmlText))
                throw new InvalidOperationException(@"The text supplied is not HTML: {htmlText}");
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

        public static string GetContainingTag(this string htmlText)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlText);
            return doc.DocumentNode.ChildNodes[0].Name;
        }

        public static string ToXliffHtmlType(this string htmlTag)
        {
            return "html:" + htmlTag;
        }

        public static string FromXliffHtmlType(this string xliffType)
        {
            return xliffType.Substring(5);
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

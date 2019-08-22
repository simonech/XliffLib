using System;
using HtmlAgilityPack;
using System.Linq;

namespace XliffLib.HtmlProcessing
{
    public abstract class BaseHtmlParser: IHtmlParser
    {
        public abstract SimplifiedHtmlContentItem[] SplitHtml(string text);

        public SimplifiedHtmlContentItem[] SplitPlainText(string text)
        {
            if (text.IsHtml())
                throw new InvalidOperationException(@"The text supplied is not plain text: {text}");
            else
            {
                var list = text.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                return list.Select(e => new SimplifiedHtmlContentItem() { Content = e }).ToArray();
            }
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
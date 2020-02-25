using System;

namespace XliffLib.HtmlProcessing
{
    public interface IHtmlParser
    {
        SimplifiedHtmlContentItem[] SplitHtml(string text);
        SimplifiedHtmlContentItem[] SplitPlainText(string text);
        string ToXliffHtmlType(string htmlTag);
        string FromXliffHtmlType(string xliffType);

        bool SupportsAttributes { get; }

    }
}
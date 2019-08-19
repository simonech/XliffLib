using System;

namespace XliffLib.HtmlProcessing
{
    public interface IHtmlParser
    {
        String[] SplitByDefaultTags(string text);
        String[] SplitPlainText(string text);
        string GetContainingTag(string htmlText);
        string RemoveContainingTag(string htmlText);
        string ToXliffHtmlType(string htmlTag);
        string FromXliffHtmlType(string xliffType);

    }
}
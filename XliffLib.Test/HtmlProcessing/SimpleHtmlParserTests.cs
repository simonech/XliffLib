using NUnit.Framework;
using System;
using XliffLib.HtmlProcessing;
using System.Linq;

namespace XliffLib.Test.HtmlProcessing
{
    [TestFixture()]
    public class SimpleHtmlParserTests
    {
        SimpleHtmlParser _htmlParser;

        [OneTimeSetUp()]
        public void Init()
        {
            _htmlParser = new SimpleHtmlParser();
        }

        [Test(), TestCaseSource(typeof(HtmlExtrationDataSamples), "SplitParagraphsCount")]
        public int ParagraphsAreRightCount(string text)
        {
            return _htmlParser.SplitByParagraphs(text).Length;
        }

        [Test(), TestCaseSource(typeof(HtmlExtrationDataSamples), "SplitParagraphsValues")]
        public string[] SplitParagraphsValuesAreCorrect(string text)
        {
            return _htmlParser.SplitByParagraphs(text).Select(e => e.ToHtmlElement()).ToArray();
        }

        [Test(), TestCaseSource(typeof(HtmlExtrationDataSamples), "SplitLIValues")]
        public string[] SplitLIValuesAreCorrect(string text)
        {
            return _htmlParser.SplitByTags(text,"ul","li","#text").Select(e => e.ToHtmlElement()).ToArray();
        }

        [Test(), TestCaseSource(typeof(HtmlExtrationDataSamples), "SplitMultipleTags")]
        public string[] SplitMultipleTagsAreCorrect(string text)
        {
            return _htmlParser.SplitHtml(text).Select(e => e.ToHtmlElement()).ToArray();
        }

    }
}

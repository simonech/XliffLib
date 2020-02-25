using NUnit.Framework;
using System;
using XliffLib.HtmlProcessing;
using System.Linq;

namespace XliffLib.Test.HtmlProcessing
{
    [TestFixture()]
    public class HierarchicHtmlParserTests
    {
        HierarchicHtmlParser _htmlParser;

        [OneTimeSetUp()]
        public void Init()
        {
            _htmlParser = new HierarchicHtmlParser();
        }

        [Test(), TestCaseSource(typeof(HtmlExtrationDataSamples), "SplitMultipleTags")]
        public string[] SplitMultipleTagsAreCorrect(string text)
        {
            return _htmlParser.SplitHtml(text).Select(e => e.ToHtmlElement(true)).ToArray();
        }
    }
}

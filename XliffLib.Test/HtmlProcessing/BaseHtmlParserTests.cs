using NUnit.Framework;
using System;
using XliffLib.HtmlProcessing;
using System.Linq;

namespace XliffLib.Test
{
    [TestFixture()]
    public class SimpleHtmlExtractorTests
    {
        SimpleHtmlParser _htmlParser;

        [OneTimeSetUp()]
        public void Init()
        {
            _htmlParser = new SimpleHtmlParser();
        }

        [Test(), TestCaseSource(typeof(HtmlExtrationDataSamples), "SplitParagraphsValues")]
        public string[] SplitParagraphsValuesAreCorrect(string text)
        {
            return _htmlParser.SplitByParagraphs(text).Select(e => e.ToHtmlElement()).ToArray();
        }



    }
}

using NUnit.Framework;
using System;
using XliffLib.HtmlProcessing;
using System.Linq;

namespace XliffLib.Test.HtmlProcessing
{
    [TestFixture()]
    public class HtmlExtensionsTest
    {

        [Test(), TestCaseSource(typeof(HtmlExtrationDataSamples), "HtmlIdentification")]
        public bool CanIdentifyHtmlInStrings(string text)
        {
            return text.IsHtml();
        }
    }
}

using NUnit.Framework;
using System;
using XliffLib.Utils;
using System.Collections;

namespace XliffLib.Test
{
    [TestFixture()]
    public class HtmlExtractionTests
    {
        [Test(),TestCaseSource(typeof(MyDataClass), "TestCases")]
        public bool CanIdentifyHtmlInStrings(string text)
        {
            return text.IsHtml();
        }
    }

	public class MyDataClass
	{
		public static IEnumerable TestCases
		{
			get
			{
				yield return new TestCaseData("Hello <b>World</b>!").Returns(true);
				yield return new TestCaseData("Hello world!").Returns(false);
				yield return new TestCaseData("<li>Item</li>").Returns(true);
			}
		}
	}
}

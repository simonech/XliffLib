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
                yield return new TestCaseData("<p>Item</p><p>Item</p><p>Item</p>").Returns(true);
                yield return new TestCaseData("Hello &lt;world!").Returns(false);
                yield return new TestCaseData("<pc id=\"1\" dataRefStart=\"1\" dataRefEnd=\"2\">Important</pc> text").Returns(false);
                yield return new TestCaseData("Ctrl+C=<cp hex=\"0003\"/>").Returns(false);
                yield return new TestCaseData("Number of entries: <ph id=\"1\" dataRef=\"d1\" /><ph id=\"2\"\r        dataRef=\"d2\"/>(These entries are only the ones matching the\r        current filter settings)").Returns(false);
                yield return new TestCaseData("<sc id=\"1\" type=\"fmt\" subType=\"xlf:b\"/>\r        First sentence. ").Returns(false);
                yield return new TestCaseData("Text in <sc id=\"1\" dataRef=\"d1\"/>bold <sc id=\"2\"\r        dataRef=\"d2\"/> and<ec startRef=\"1\" dataRef=\"d3\"/>\r         italics<ec startRef=\"2\" dataRef=\"d4\"/>.").Returns(false);
                yield return new TestCaseData("He saw his <mrk id=\"m1\" translate=\"no\">doppelgänger</mrk>").Returns(false);
                yield return new TestCaseData("<sm id=\"m1\" type=\"comment\" value=\"Comment for B and C\"/>\r        Sentence B.>").Returns(false);
                yield return new TestCaseData("Sentence C.<em startRef=\"m1\"/>>").Returns(false);
			}
		}
	}
}

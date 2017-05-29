using NUnit.Framework;
using System;
using XliffLib.Utils;
using System.Collections;

namespace XliffLib.Test
{
    [TestFixture()]
    public class HtmlExtractionTests
    {
        [Test(), TestCaseSource(typeof(DataSamples), "HtmlIdentification")]
        public bool CanIdentifyHtmlInStrings(string text)
        {
            return text.IsHtml();
        }

        [Test(), TestCaseSource(typeof(DataSamples), "SplitParagraphsCount")]
        public int ParagraphsAreRightCount(string text)
        {
            return text.SplitByParagraphs().Length;
        }

        [Test(), TestCaseSource(typeof(DataSamples), "SplitParagraphsValues")]
        public string[] SplitParagraphsValuesAreCorrect(string text)
        {
            return text.SplitByParagraphs();
        }

        [Test(), TestCaseSource(typeof(DataSamples), "SplitLIValues")]
        public string[] SplitLIValuesAreCorrect(string text)
        {
            return text.SplitByTags("ul");
        }

        [Test(), TestCaseSource(typeof(DataSamples), "SplitMultipleTags")]
        public string[] SplitMultipleTagsAreCorrect(string text)
        {
            return text.SplitByTags("ul", "p");
        }

        [Test(), TestCaseSource(typeof(DataSamples), "RemoveContainingHtmlTag")]
        public string RemoveContainingHtmlTagAreCorrect(string text)
        {
            return text.RemoveContainingTag();
        }
    }

    public class DataSamples
    {
        public static IEnumerable HtmlIdentification
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


        public static IEnumerable SplitParagraphsCount
        {
            get
            {
                yield return new TestCaseData("Hello <b>World</b>!").Returns(0);
                yield return new TestCaseData("<p>Item</p>").Returns(1);
                yield return new TestCaseData("<p>Item</p><p>Item</p><p>Item</p>").Returns(3);
                yield return new TestCaseData("<p>Item</p><ul><li>Item</li><li>Item</li><li>Item</li></ul>").Returns(1);
            }
        }

        public static IEnumerable SplitParagraphsValues
        {
            get
            {
                yield return new TestCaseData("<p>Item</p>").Returns(new string[] { "<p>Item</p>" });
                yield return new TestCaseData("<p>Item1</p><p>Item2</p><p>Item3</p>").Returns(new string[] { "<p>Item1</p>", "<p>Item2</p>", "<p>Item3</p>" });
                yield return new TestCaseData("<p>Para1</p><ul><li>Item1</li><li>Item2</li><li>Item3</li></ul><p>Para2</p>").Returns(new string[] { "<p>Para1</p>", "<p>Para2</p>" });
            }
        }

        public static IEnumerable SplitLIValues
        {
            get
            {
                yield return new TestCaseData("<p>Para1</p><ul><li>Item1</li><li>Item2</li></ul><p>Para2</p>").Returns(new string[] { "<ul><li>Item1</li><li>Item2</li></ul>" });
            }
        }

        public static IEnumerable SplitMultipleTags
        {
            get
            {
                yield return new TestCaseData("<p>Para1</p><ul><li>Item1</li><li>Item2</li></ul><p>Para2</p>").Returns(new string[] { "<p>Para1</p>", "<ul><li>Item1</li><li>Item2</li></ul>", "<p>Para2</p>" });
            }
        }

        public static IEnumerable RemoveContainingHtmlTag
        {
            get
            {
                yield return new TestCaseData("<p>Item</p>").Returns("Item");
                yield return new TestCaseData("<li>Item1</li>").Returns("Item1");
                yield return new TestCaseData("<p>This is text with some <b>formatting</b> inside</p>").Returns("This is text with some <b>formatting</b> inside");
            }
        }
    }
}

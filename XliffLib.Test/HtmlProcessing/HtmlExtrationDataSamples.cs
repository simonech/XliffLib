using NUnit.Framework;
using System.Collections;

namespace XliffLib.Test.HtmlProcessing
{
    public class HtmlExtrationDataSamples
    {
        public static IEnumerable HtmlIdentification
        {
            get
            {
                yield return new TestCaseData("Hello <b>World</b>!").Returns(true);
                yield return new TestCaseData("Hello <strong>World</strong>!").Returns(true);
                yield return new TestCaseData("Hello <i>World</i>!").Returns(true);
                yield return new TestCaseData("Hello <em>World</em>!").Returns(true);
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
                yield return new TestCaseData("<li>Item1</li><li>Item2</li>").Returns(new string[] { "<li>Item1</li>","<li>Item2</li>" });
                yield return new TestCaseData("Text<ul><li>Item1</li><li>Item2</li></ul>").Returns(new string[] { "Text", "<ul><li>Item1</li><li>Item2</li></ul>" });
            }
        }

        public static IEnumerable SplitMultipleTags
        {
            get
            {
                yield return new TestCaseData("<p>Para1</p><ul><li>Item1</li><li>Item2</li></ul><p>Para2</p>").Returns(new string[] { "<p>Para1</p>", "<ul><li>Item1</li><li>Item2</li></ul>", "<p>Para2</p>" });
                yield return new TestCaseData("<h1>Para1</h1><ul><li>Item1</li><li>Item2</li></ul><p>Para2</p>").Returns(new string[] { "<h1>Para1</h1>", "<ul><li>Item1</li><li>Item2</li></ul>", "<p>Para2</p>" });
                yield return new TestCaseData("<h1>Para1</h1><ul><li class=\"odd\">Item1</li><li>Item2</li></ul><p>Para2</p>").Returns(new string[] { "<h1>Para1</h1>", "<ul><li class=\"odd\">Item1</li><li>Item2</li></ul>", "<p>Para2</p>" });
            }
        }
    }
}

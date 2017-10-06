using System;
using Localization.Xliff.OM.Core;
using NUnit.Framework;
using XliffLib.Utils;
using System.Collections.Generic;

namespace XliffLib.Test
{
    [TestFixture()]
    public class InlineCodeTests
    {
        [Test()]
        public void PlainTextIsNotConveredToInlineCodes()
        {

            string original = "This is a simple text";

            var result = original.ConvertHtmlTagsInInLineCodes();

            Assert.Zero(result.OriginalData.Count);

            Assert.AreEqual(1,result.Text.Count);
            var plainText = result.Text[0] as PlainText;
            Assert.IsNotNull(plainText);
            Assert.AreEqual(original,plainText.Text);
        }

        [Test()]
        public void TextWithOneTagIsConvertedIntoOnePcAndTwoData()
        {

            string original = "This is a <b>formatted</b> text";

            var result = original.ConvertHtmlTagsInInLineCodes();

            Assert.AreEqual(2, result.OriginalData.Count);
            Assert.AreEqual("d1", result.OriginalData["<b>"], "Original data not stored");
            Assert.AreEqual("d2", result.OriginalData["</b>"], "Original data not stored");

            Assert.AreEqual(3, result.Text.Count);
            var plainText = result.Text[0] as PlainText;
            var pc = result.Text[1] as SpanningCode;
            var plainText2 = result.Text[2] as PlainText;
            Assert.IsNotNull(plainText);
            Assert.IsNotNull(pc);
            Assert.IsNotNull(plainText2);
            Assert.AreEqual("This is a ", plainText.Text);
            Assert.AreEqual("formatted",(pc.Text[0] as PlainText).Text);
            Assert.AreEqual("1",pc.Id);
            Assert.AreEqual("d1", pc.DataReferenceStart);
            Assert.AreEqual("d2", pc.DataReferenceEnd);
            Assert.AreEqual(" text", plainText2.Text);
        }

        [Test()]
        public void TextWithTwoDifferentTagsIsConvertedIntoTwoPcAndFourData()
        {
            string original = "This is a <b>formatted</b> <i>text</i>";

            var result = original.ConvertHtmlTagsInInLineCodes();

            Assert.AreEqual(4, result.OriginalData.Count);
            Assert.AreEqual("d1", result.OriginalData["<b>"], "Original data not stored");
            Assert.AreEqual("d2", result.OriginalData["</b>"], "Original data not stored");
            Assert.AreEqual("d3", result.OriginalData["<i>"], "Original data not stored");
            Assert.AreEqual("d4", result.OriginalData["</i>"], "Original data not stored");

            Assert.AreEqual(4,result.Text.Count);
        }

        [Test()]
        public void TextWithTwoEqualTagsIsConvertedIntoTwoPcAndTwoData()
        {

            string original = "This is a <b>formatted</b> <b>text</b>";

            var result = original.ConvertHtmlTagsInInLineCodes();

            Assert.AreEqual(2, result.OriginalData.Count);
            Assert.AreEqual("d1", result.OriginalData["<b>"], "Original data not stored");
            Assert.AreEqual("d2", result.OriginalData["</b>"], "Original data not stored");

            Assert.AreEqual(4, result.Text.Count);
        }

        [Test()]
        public void StandaloneTagIsConvertedIntoOnePhAndOneData()
        {

            string original = "<br/>";

            var result = original.ConvertHtmlTagsInInLineCodes();

            Assert.AreEqual(1, result.OriginalData.Count);
            Assert.AreEqual("d1", result.OriginalData["<br/>"], "Original data not stored");

            var ph = result.Text[0] as StandaloneCode;
            Assert.IsNotNull(ph);
            Assert.AreEqual("d1",ph.DataReference);
            Assert.AreEqual(CodeType.Formatting,ph.Type);
            Assert.AreEqual("xlf:lb", ph.SubType);
        }

        [Test()]
        public void BoldCodeGetsRightTypeSubType()
        {

            string original = "<b>formatted</b>";

            var result = original.ConvertHtmlTagsInInLineCodes();

            Assert.AreEqual(1, result.Text.Count);
            var pc = result.Text[0] as SpanningCode;
            Assert.IsNotNull(pc);
            Assert.AreEqual(CodeType.Formatting,pc.Type);
            Assert.AreEqual("xlf:b", pc.SubType);
        }

        [Test()]
        public void ItalicsCodeGetsRightTypeSubType()
        {

            string original = "<i>formatted</i>";

            var result = original.ConvertHtmlTagsInInLineCodes();

            Assert.AreEqual(1, result.Text.Count);
            var pc = result.Text[0] as SpanningCode;
            Assert.IsNotNull(pc);
            Assert.AreEqual(CodeType.Formatting, pc.Type);
            Assert.AreEqual("xlf:i", pc.SubType);
        }

        [Test()]
        public void UnderlineCodeGetsRightTypeSubType()
        {

            string original = "<u>formatted</u>";

            var result = original.ConvertHtmlTagsInInLineCodes();

            Assert.AreEqual(1, result.Text.Count);
            var pc = result.Text[0] as SpanningCode;
            Assert.IsNotNull(pc);
            Assert.AreEqual(CodeType.Formatting, pc.Type);
            Assert.AreEqual("xlf:u", pc.SubType);
        }


        [Test]
        public void PlainTextInXliffIsNotChanged()
        {
            var xliffCode = new List<ResourceStringContent>();

            var text = new PlainText("text");
            xliffCode.Add(text);

            var htmlString = xliffCode.ConvertToHtml();

            Assert.AreEqual("text", htmlString);
        }


        [Test]
        public void PcWithBoldTypeGetsBackToHtmlBTag()
        {
            var xliffCode = new List<ResourceStringContent>();
            
            var pc = new SpanningCode("1", "bold");
            pc.Type = CodeType.Formatting;
            pc.SubType = "xlf:b";
            xliffCode.Add(pc);

            var htmlString = xliffCode.ConvertToHtml();

            Assert.AreEqual("<b>bold</b>", htmlString);
        }

    }
}

using System;
using Localization.Xliff.OM.Core;
using NUnit.Framework;
using XliffLib.Utils;
using System.Collections.Generic;
using System.Linq;

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
            Assert.AreEqual("d1", result.OriginalData["<br />"], "Original data not stored");

            Assert.AreEqual(1, result.Text.Count);
        }

        [Test()]
        public void StandaloneTagWithAttributeIsConvertedIntoOnePhWithAttributeInOriginalData()
        {

            string original = "<img src=\"media\\12345\\image.jpg\"/>";

            var result = original.ConvertHtmlTagsInInLineCodes();

            Assert.AreEqual(1, result.OriginalData.Count);
            Assert.AreEqual("d1", result.OriginalData["<img src=\"media\\12345\\image.jpg\" />"], "Original data not stored");

            Assert.AreEqual(1, result.Text.Count);
        }

        [Test()]
        public void TextWithOneTagWithAttributeIsConvertedIntoOnePcAndTwoOriginalDataWithAttribute()
        {

            string original = "This is a <a href=\"http://council.eu\">link</a>";

            var result = original.ConvertHtmlTagsInInLineCodes();

            Assert.AreEqual(2, result.OriginalData.Count);
            Assert.AreEqual("d1", result.OriginalData["<a href=\"http://council.eu\">"], "Original data not stored");
            Assert.AreEqual("d2", result.OriginalData["</a>"], "Original data not stored");

            Assert.AreEqual(2, result.Text.Count);
            var plainText = result.Text[0] as PlainText;
            var pc = result.Text[1] as SpanningCode;
            Assert.IsNotNull(plainText);
            Assert.IsNotNull(pc);
            Assert.AreEqual("This is a ", plainText.Text);
            Assert.AreEqual("link", (pc.Text[0] as PlainText).Text);
            Assert.AreEqual("1", pc.Id);
            Assert.AreEqual("d1", pc.DataReferenceStart);
            Assert.AreEqual("d2", pc.DataReferenceEnd);
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
        public void LineBreakCodeGetsRightTypeSubType()
        {
            string original = "<br/>";

            var result = original.ConvertHtmlTagsInInLineCodes();

            var ph = result.Text[0] as StandaloneCode;
            Assert.IsNotNull(ph);
            Assert.AreEqual(CodeType.Formatting, ph.Type);
            Assert.AreEqual("xlf:lb", ph.SubType);
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

        [Test()]
        public void SuperscriptCodeGetsRightTypeSubType()
        {
            string original = "<sup>formatted</sup>";

            var result = original.ConvertHtmlTagsInInLineCodes();

            Assert.AreEqual(1, result.Text.Count);
            var pc = result.Text[0] as SpanningCode;
            Assert.IsNotNull(pc);
            Assert.AreEqual(CodeType.Formatting, pc.Type);
            Assert.AreEqual("html:sup", pc.SubType);
        }

        [Test()]
        public void AnchorCodeGetsRightTypeSubType()
        {
            string original = "<a href=\"http://council.eu\">Link</a>";

            var result = original.ConvertHtmlTagsInInLineCodes();

            Assert.AreEqual(1, result.Text.Count);
            var pc = result.Text[0] as SpanningCode;
            Assert.IsNotNull(pc);
            Assert.AreEqual(CodeType.Link, pc.Type);
            Assert.IsNull(pc.SubType);
        }

        [Test()]
        public void AnchorWithOneAttributeGetsSubflowWithAttributes()
        {
            string original = "<a href=\"http://council.eu\">Link</a>";

            var result = original.ConvertHtmlTagsInInLineCodes();

            Assert.AreEqual(1, result.SubFlow.Count);
            Assert.AreEqual("http://council.eu", result.SubFlow["1-href"]);

            Assert.AreEqual(1, result.Text.Count);
            var pc = result.Text[0] as SpanningCode;
            Assert.IsNotNull(pc);
            Assert.AreEqual("1-href", pc.SubFlowsStart);
        }

        [Test()]
        public void AnchorWithMultipleAttributesGetsSubflowWithAttributes()
        {
            string original = "<a href=\"http://council.eu\" title=\"Link title\">Link</a>";

            var result = original.ConvertHtmlTagsInInLineCodes();

            Assert.AreEqual(2, result.SubFlow.Count);
            Assert.AreEqual("http://council.eu", result.SubFlow["1-href"]);
            Assert.AreEqual("Link title", result.SubFlow["1-title"]);

            Assert.AreEqual(1, result.Text.Count);
            var pc = result.Text[0] as SpanningCode;
            Assert.IsNotNull(pc);
            Assert.AreEqual("1-href 1-title", pc.SubFlowsStart);
        }

        [Test()]
        public void MultipleAnchorsGetsSubflowWithAttributes()
        {
            string original = "<a href=\"http://council.eu\">Council</a> and <a href=\"http://microsoft.com\">Microsoft</a>";

            var result = original.ConvertHtmlTagsInInLineCodes();

            Assert.AreEqual(2, result.SubFlow.Count);
            Assert.AreEqual("http://council.eu", result.SubFlow["1-href"]);
            Assert.AreEqual("http://microsoft.com", result.SubFlow["2-href"]);

            Assert.AreEqual(3, result.Text.Count);
            var pc = result.Text[0] as SpanningCode;
            Assert.IsNotNull(pc);
            Assert.AreEqual("1-href", pc.SubFlowsStart);

            var pc1 = result.Text[2] as SpanningCode;
            Assert.IsNotNull(pc1);
            Assert.AreEqual("2-href", pc1.SubFlowsStart);
        }

        [Test()]
        public void InlineImageGetsRightTypeSubType()
        {
            string original = "<img src=\"media\\12345\\image.jpg\" />";

            var result = original.ConvertHtmlTagsInInLineCodes();

            Assert.AreEqual(1, result.Text.Count);
            var ph = result.Text[0] as StandaloneCode;
            Assert.IsNotNull(ph);
            Assert.AreEqual(CodeType.Image, ph.Type);
            Assert.IsNull(ph.SubType);
        }

        [Test()]
        public void InlineImageWithOneAttributeGetsSubflowWithAttributes()
        {
            string original = "<img src=\"media\\12345\\image.jpg\" />";

            var result = original.ConvertHtmlTagsInInLineCodes();

            Assert.AreEqual(1, result.SubFlow.Count);
            Assert.AreEqual("media\\12345\\image.jpg", result.SubFlow["1-src"]);

            Assert.AreEqual(1, result.Text.Count);
            var ph = result.Text[0] as StandaloneCode;
            Assert.IsNotNull(ph);
            Assert.AreEqual("1-src", ph.SubFlows);
        }

        [Test()]
        public void InlineImageWithMultipleAttributesGetsSubflowWithAttributes()
        {
            string original = "<img src=\"media\\12345\\image.jpg\" title=\"title\" />";

            var result = original.ConvertHtmlTagsInInLineCodes();

            Assert.AreEqual(2, result.SubFlow.Count);
            Assert.AreEqual("media\\12345\\image.jpg", result.SubFlow["1-src"]);
            Assert.AreEqual("title", result.SubFlow["1-title"]);

            Assert.AreEqual(1, result.Text.Count);
            var ph = result.Text[0] as StandaloneCode;
            Assert.IsNotNull(ph);
            Assert.AreEqual("1-src 1-title", ph.SubFlows);
        }

        [Test()]
        public void MultipleInlineImagesWithGetsSubflowWithAttributes()
        {
            string original = "<img src=\"media\\12345\\image.jpg\" /> and <img src=\"media\\12345\\image2.jpg\" />";

            var result = original.ConvertHtmlTagsInInLineCodes();

            Assert.AreEqual(2, result.SubFlow.Count);
            Assert.AreEqual("media\\12345\\image.jpg", result.SubFlow["1-src"]);
            Assert.AreEqual("media\\12345\\image2.jpg", result.SubFlow["2-src"]);

            Assert.AreEqual(3, result.Text.Count);
            var ph = result.Text[0] as StandaloneCode;
            Assert.IsNotNull(ph);
            Assert.AreEqual("1-src", ph.SubFlows);

            var ph1 = result.Text[2] as StandaloneCode;
            Assert.IsNotNull(ph1);
            Assert.AreEqual("2-src", ph1.SubFlows);
        }

        [Test()]
        public void SubscriptCodeGetsRightTypeSubType()
        {
            string original = "<sub>formatted</sub>";

            var result = original.ConvertHtmlTagsInInLineCodes();

            Assert.AreEqual(1, result.Text.Count);
            var pc = result.Text[0] as SpanningCode;
            Assert.IsNotNull(pc);
            Assert.AreEqual(CodeType.Formatting, pc.Type);
            Assert.AreEqual("html:sub", pc.SubType);
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
        public void PcWithBoldSubtypeGetsBackToHtmlStrongTag()
        {
            var xliffCode = new List<ResourceStringContent>();
            
            var pc = new SpanningCode("1", "text");
            pc.Type = CodeType.Formatting;
            pc.SubType = "xlf:b";
            xliffCode.Add(pc);

            var htmlString = xliffCode.ConvertToHtml();

            Assert.AreEqual("<strong>text</strong>", htmlString);
        }

        [Test]
        public void PcWithItalicSubtypeGetsBackToHtmlEmTag()
        {
            var xliffCode = new List<ResourceStringContent>();

            var pc = new SpanningCode("1", "text");
            pc.Type = CodeType.Formatting;
            pc.SubType = "xlf:i";
            xliffCode.Add(pc);

            var htmlString = xliffCode.ConvertToHtml();

            Assert.AreEqual("<em>text</em>", htmlString);
        }

        [Test]
        public void PcWithUnderlineSubtypeGetsBackToHtmlUTag()
        {
            var xliffCode = new List<ResourceStringContent>();

            var pc = new SpanningCode("1", "text");
            pc.Type = CodeType.Formatting;
            pc.SubType = "xlf:u";
            xliffCode.Add(pc);

            var htmlString = xliffCode.ConvertToHtml();

            Assert.AreEqual("<u>text</u>", htmlString);
        }

        [Test]
        public void PcWithCustomSubscriptSubtypeGetsBackToHtmlUTag()
        {
            var xliffCode = new List<ResourceStringContent>();

            var pc = new SpanningCode("1", "text");
            pc.Type = CodeType.Formatting;
            pc.SubType = "html:sub";
            xliffCode.Add(pc);

            var htmlString = xliffCode.ConvertToHtml();

            Assert.AreEqual("<sub>text</sub>", htmlString);
        }

        [Test]
        public void PcWithCustomSuperscriptSubtypeGetsBackToHtmlUTag()
        {
            var xliffCode = new List<ResourceStringContent>();

            var pc = new SpanningCode("1", "text");
            pc.Type = CodeType.Formatting;
            pc.SubType = "html:sup";
            xliffCode.Add(pc);

            var htmlString = xliffCode.ConvertToHtml();

            Assert.AreEqual("<sup>text</sup>", htmlString);
        }

        [Test]
        public void PcWithLinkTypeGetsBackToHtmlAnchorTag()
        {
            var xliffCode = new List<ResourceStringContent>();

            var pc = new SpanningCode("1", "text");
            pc.Type = CodeType.Link;
            xliffCode.Add(pc);

            var htmlString = xliffCode.ConvertToHtml();

            Assert.AreEqual("<a>text</a>", htmlString);
        }

        [Test]
        public void PcWithOneSubFlowGetsBackToHtmlWithAttributes()
        {
            var xliffCode = new List<ResourceStringContent>();

            var pc = new SpanningCode("1", "text");
            pc.Type = CodeType.Link;
            pc.SubFlowsStart = "u1-5-1-href";
            xliffCode.Add(pc);

            var subflows = new Dictionary<string, string>()
            {
                { "u1-5-1-href", "http://council.eu" }
            };

            var htmlString = xliffCode.ConvertToHtml(s => SearchSubflowsInDictionary(s,subflows));

            Assert.AreEqual("<a href=\"http://council.eu\">text</a>", htmlString);
        }

        [Test]
        public void PcWithMultipleSubFlowsGetsBackToHtmlWithAttributes()
        {
            var xliffCode = new List<ResourceStringContent>();

            var pc = new SpanningCode("1", "text");
            pc.Type = CodeType.Link;
            pc.SubFlowsStart = "u1-5-1-href u1-5-1-title";
            xliffCode.Add(pc);

            var subflows = new Dictionary<string, string>()
            {
                { "u1-5-1-href", "http://council.eu" },
                { "u1-5-1-title", "title" }
            };

            var htmlString = xliffCode.ConvertToHtml(s => SearchSubflowsInDictionary(s, subflows));

            Assert.AreEqual("<a href=\"http://council.eu\" title=\"title\">text</a>", htmlString);
        }

        private IEnumerable<KeyValuePair<string, string>> SearchSubflowsInDictionary(string subflowsStringList, Dictionary<string, string> subflows)
        {
            return subflows.Where(s => subflowsStringList.Split(' ').Contains(s.Key));
        }

        [Test]
        public void PcsWithUnrelatedSubFlowsGetsBackToHtmlWithAttributes()
        {
            var xliffCode = new List<ResourceStringContent>();

            var pc = new SpanningCode("1", "text");
            pc.Type = CodeType.Link;
            pc.SubFlowsStart = "u1-5-1-href";
            xliffCode.Add(pc);

            var subflows = new Dictionary<string, string>()
            {
                { "u1-5-1-href", "http://council.eu" },
                { "u1-5-2-href", "http://microsoft.com" }
            };

            var htmlString = xliffCode.ConvertToHtml(s => SearchSubflowsInDictionary(s, subflows));

            Assert.AreEqual("<a href=\"http://council.eu\">text</a>", htmlString);
        }

        [Test]
        public void PhWithLinebreakSubtypeGetsBackToHtmlBrTag()
        {
            var xliffCode = new List<ResourceStringContent>();

            var ph = new StandaloneCode("1");
            ph.Type = CodeType.Formatting;
            ph.SubType = "xlf:lb";
            xliffCode.Add(ph);

            var htmlString = xliffCode.ConvertToHtml();

            Assert.AreEqual("<br/>", htmlString);
        }

        [Test]
        public void PhWithImageTypeGetsBackToHtmlImgTag()
        {
            var xliffCode = new List<ResourceStringContent>();

            var ph = new StandaloneCode("1");
            ph.Type = CodeType.Image;
            xliffCode.Add(ph);

            var htmlString = xliffCode.ConvertToHtml();

            Assert.AreEqual("<img/>", htmlString);
        }

        [Test]
        public void PhWithOneSubFlowGetsBackToHtmlWithAttributes()
        {
            var xliffCode = new List<ResourceStringContent>();

            var ph = new StandaloneCode("1");
            ph.Type = CodeType.Image;
            ph.SubFlows = "u1-5-1-src";
            xliffCode.Add(ph);


            var subflows = new Dictionary<string, string>()
            {
                { "u1-5-1-src", "media\\12345\\image.jpg" }
            };

            var htmlString = xliffCode.ConvertToHtml(s => SearchSubflowsInDictionary(s, subflows));

            Assert.AreEqual("<img src=\"media\\12345\\image.jpg\"/>", htmlString);
        }

        [Test]
        public void PhWithMultipleSubFlowGetsBackToHtmlWithAttributes()
        {
            var xliffCode = new List<ResourceStringContent>();

            var ph = new StandaloneCode("1");
            ph.Type = CodeType.Image;
            ph.SubFlows = "u1-5-1-src u1-5-1-title";
            xliffCode.Add(ph);


            var subflows = new Dictionary<string, string>()
            {
                { "u1-5-1-src", "media\\12345\\image.jpg" },
                { "u1-5-1-title", "title" },
            };

            var htmlString = xliffCode.ConvertToHtml(s => SearchSubflowsInDictionary(s, subflows));

            Assert.AreEqual("<img src=\"media\\12345\\image.jpg\" title=\"title\"/>", htmlString);
        }

        [Test]
        public void PhWithUnrelatedSubFlowGetsBackToHtmlWithAttributes()
        {
            var xliffCode = new List<ResourceStringContent>();

            var ph = new StandaloneCode("1");
            ph.Type = CodeType.Image;
            ph.SubFlows = "u1-5-1-src";
            xliffCode.Add(ph);


            var subflows = new Dictionary<string, string>()
            {
                { "u1-5-1-src", "media\\12345\\image.jpg" },
                { "u1-5-2-src", "media\\12345\\image2.jpg" }
            };

            var htmlString = xliffCode.ConvertToHtml(s => SearchSubflowsInDictionary(s, subflows));

            Assert.AreEqual("<img src=\"media\\12345\\image.jpg\"/>", htmlString);
        }

    }
}

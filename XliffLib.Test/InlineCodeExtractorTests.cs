﻿using System;
using System.IO;
using System.Linq;
using Localization.Xliff.OM.Core;
using Localization.Xliff.OM.Serialization;
using NUnit.Framework;

namespace XliffLib.Test
{
    [TestFixture()]
    public class InLineCodeExtractorTests
    {
        [Test()]
        public void SingleParagraphPlainTextUnitIsLeftUntouched()
        {
            var xliff = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xliff srcLang=""en-GB"" version=""2.0"" xmlns=""urn:oasis:names:tc:xliff:document:2.0"">
    <file id=""f1"">
        <unit id=""u1"">
            <segment>
                <source>Hello World!</source>
            </segment>
        </unit>
    </file>
</xliff>";

            XliffDocument document = LoadXliff(xliff);
            var inlineprocessing = new InlineCodeProcessing();

            var newDocument = inlineprocessing.ExecuteExtraction(document);

            Assert.AreEqual(1, newDocument.Files[0].Containers.Count);
            var unit = newDocument.Files[0].Containers[0] as Unit;
            Assert.IsNull(unit.OriginalData);

            Assert.AreEqual(1,unit.CollapseChildren<Source>()[0].Text.Count);
            var segment = unit.CollapseChildren<Source>()[0].Text[0] as PlainText;
            Assert.IsNotNull(segment);
            Assert.AreEqual("Hello World!",segment.Text);
        }

        [Test()]
        public void SingleParagraphCDataWithoutFormattigIsLeftUntouched()
        {
            var xliff = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xliff srcLang=""en-GB"" version=""2.0"" xmlns=""urn:oasis:names:tc:xliff:document:2.0"">
    <file id=""f1"">
        <unit id=""u1"">
            <segment>
                <source><![CDATA[Hello World!]]></source>
            </segment>
        </unit>
    </file>
</xliff>";

            XliffDocument document = LoadXliff(xliff);
            var inlineprocessing = new InlineCodeProcessing();

            var newDocument = inlineprocessing.ExecuteExtraction(document);

            Assert.AreEqual(1, newDocument.Files[0].Containers.Count);
            var unit = newDocument.Files[0].Containers[0] as Unit;
            Assert.IsNull(unit.OriginalData);

            Assert.AreEqual(1, unit.CollapseChildren<Source>()[0].Text.Count);
            var segment = unit.CollapseChildren<Source>()[0].Text[0] as PlainText;
            Assert.IsNotNull(segment);
            Assert.AreEqual("Hello World!", segment.Text);
        }

        [Test()]
        public void TextWithBoldUnitCreatesUnitWithOriginalDataAndInlineCode()
        {
            var xliff = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xliff srcLang=""en-GB"" version=""2.0"" xmlns=""urn:oasis:names:tc:xliff:document:2.0"">
    <file id=""f1"">
        <unit id=""u1"">
            <segment>
                <source><![CDATA[Hello <b>World</b>!]]></source>
            </segment>
        </unit>
    </file>
</xliff>";

            XliffDocument document = LoadXliff(xliff);
            var inlineprocessing = new InlineCodeProcessing();

            var newDocument = inlineprocessing.ExecuteExtraction(document);

            Assert.AreEqual(1, newDocument.Files[0].Containers.Count);
            var unit = newDocument.Files[0].Containers[0] as Unit;
            Assert.IsNotNull(unit.OriginalData);
            Assert.AreEqual(2,unit.OriginalData.DataElements.Count);
            var dataItem1 = unit.OriginalData.DataElements.Single(d => d.Id.Equals("d1")).Text[0].ToString();
            var dataItem2 = unit.OriginalData.DataElements.Single(d => d.Id.Equals("d2")).Text[0].ToString();
            Assert.AreEqual("<b>", dataItem1);
            Assert.AreEqual("</b>", dataItem2);


            var segment = unit.CollapseChildren<Source>()[0];
            Assert.AreEqual(3, segment.Text.Count);
            var text1 = segment.Text[0] as PlainText;
            var pc = segment.Text[1] as SpanningCode;
            var text2 = segment.Text[2] as PlainText;
            Assert.IsNotNull(text1);
            Assert.IsNotNull(pc);
            Assert.IsNotNull(text2);
            Assert.AreEqual("Hello ", text1.Text);
            Assert.AreEqual("World",(pc.Text[0] as PlainText).Text);
            Assert.AreEqual("d1",pc.DataReferenceStart);
            Assert.AreEqual("d2", pc.DataReferenceEnd);
            Assert.AreEqual("!", text2.Text);
        }

        [Test()]
        public void TextWithHrefUnitCreatesNewGroupWithSubflow()
        {
            var xliff = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xliff srcLang=""en-GB"" version=""2.0"" xmlns=""urn:oasis:names:tc:xliff:document:2.0"">
    <file id=""f1"">
        <unit id=""u1"">
            <segment>
                <source><![CDATA[Link to <a href=""http://consilium.eu"">Council site</a>!]]></source>
            </segment>
        </unit>
    </file>
</xliff>";

            XliffDocument document = LoadXliff(xliff);
            var inlineprocessing = new InlineCodeProcessing();

            var newDocument = inlineprocessing.ExecuteExtraction(document);

            Assert.AreEqual(1, newDocument.Files[0].Containers.Count);
            var group = newDocument.Files[0].Containers[0] as Group;
            Assert.IsNotNull(group);

            Assert.AreEqual("u1-g", group.Id);
            Assert.AreEqual(2, group.Containers.Count);

            var unitSubflow = group.Containers[0] as Unit;
            Assert.AreEqual("u1-1-href", unitSubflow.Id);
            Assert.AreEqual(1, unitSubflow.CollapseChildren<Source>()[0].Text.Count);
            var subFlowSegment = unitSubflow.CollapseChildren<Source>()[0].Text[0] as PlainText;
            Assert.IsNotNull(subFlowSegment);
            Assert.AreEqual("http://consilium.eu", subFlowSegment.Text);

            var unit = group.Containers[1] as Unit;

            Assert.IsNotNull(unit.OriginalData);
            Assert.AreEqual(2, unit.OriginalData.DataElements.Count);
            var dataItem1 = unit.OriginalData.DataElements.Single(d => d.Id.Equals("d1")).Text[0].ToString();
            var dataItem2 = unit.OriginalData.DataElements.Single(d => d.Id.Equals("d2")).Text[0].ToString();
            Assert.AreEqual("<a href=\"http://consilium.eu\">", dataItem1);
            Assert.AreEqual("</a>", dataItem2);

            var segment = unit.CollapseChildren<Source>()[0];
            Assert.AreEqual(3, segment.Text.Count);
            var text1 = segment.Text[0] as PlainText;
            var pc = segment.Text[1] as SpanningCode;
            var text2 = segment.Text[2] as PlainText;
            Assert.IsNotNull(text1);
            Assert.IsNotNull(pc);
            Assert.IsNotNull(text2);
            Assert.AreEqual("Link to ", text1.Text);
            Assert.AreEqual("Council site", (pc.Text[0] as PlainText).Text);
            Assert.AreEqual("d1", pc.DataReferenceStart);
            Assert.AreEqual("d2", pc.DataReferenceEnd);
            Assert.AreEqual("u1-1-href", pc.SubFlowsStart);
            Assert.AreEqual("!", text2.Text);
        }

        //Move to actual XliffReader
        private static XliffDocument LoadXliff(string xliff)
        {
            XliffDocument document = null;
            using (Stream stream = GenerateStreamFromString(xliff))
            {
                var reader = new XliffReader();
                document = reader.Deserialize(stream);
            }

            return document;
        }


        private static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}

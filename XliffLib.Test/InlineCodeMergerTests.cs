using System;
using System.IO;
using Localization.Xliff.OM.Core;
using Localization.Xliff.OM.Serialization;
using NUnit.Framework;

namespace XliffLib.Test
{
    [TestFixture()]
    public class InlineCodeMergerTests
    {

        [Test()]
        public void SingleParagraphPlainTextUnitIsLeftUntouched()
        {
            var xliff = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xliff srcLang=""en-GB"" version=""2.0"" trgLang=""it-IT"" xmlns=""urn:oasis:names:tc:xliff:document:2.0"">
    <file id=""f1"">
        <unit id=""u1"">
            <segment>
                <source>Hello World!</source>
                <target>Hello World!</target>
            </segment>
        </unit>
    </file>
</xliff>";

            XliffDocument document = LoadXliff(xliff);
            var inlineprocessing = new InlineCodeProcessing();

            var newDocument = inlineprocessing.ExecuteMerge(document);

            Assert.AreEqual(1, newDocument.Files[0].Containers.Count);
            var unit = newDocument.Files[0].Containers[0] as Unit;

            Assert.AreEqual(1, unit.CollapseChildren<Target>()[0].Text.Count);
            var segment = unit.CollapseChildren<Target>()[0].Text[0] as PlainText;
            Assert.IsNotNull(segment);
            Assert.AreEqual("Hello World!", segment.Text);
        }

        [Test()]
        public void TargetWithPcGetConvertedToCDataWithBHtmlTag()
        {
            var xliff = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xliff srcLang=""en-GB"" version=""2.0"" trgLang=""it-IT"" xmlns=""urn:oasis:names:tc:xliff:document:2.0"">
    <file id=""f1"">
        <unit id=""u1"">
            <segment>
                <source>Hello <pc id=""1"" subType=""xlf:b"" type=""fmt"">World</pc>!</source>
                <target>Hello <pc id=""1"" subType=""xlf:b"" type=""fmt"">World</pc>!</target>
            </segment>
        </unit>
    </file>
</xliff>";

            XliffDocument document = LoadXliff(xliff);
            var inlineprocessing = new InlineCodeProcessing();

            var newDocument = inlineprocessing.ExecuteMerge(document);

            Assert.AreEqual(1, newDocument.Files[0].Containers.Count);
            var unit = newDocument.Files[0].Containers[0] as Unit;

            Assert.AreEqual(1, unit.CollapseChildren<Target>()[0].Text.Count);
            var segment = unit.CollapseChildren<Target>()[0].Text[0] as CDataTag;
            Assert.IsNotNull(segment);
            Assert.AreEqual("<p>Hello <strong>World</strong>!</p>", segment.Text);
        }

        [Test()]
        public void TargetWithPhGetConvertedToCDataWithBrHtmlTag()
        {
            var xliff = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xliff srcLang=""en-GB"" version=""2.0"" trgLang=""it-IT"" xmlns=""urn:oasis:names:tc:xliff:document:2.0"">
    <file id=""f1"">
        <unit id=""u1"">
            <segment>
                <source>Hello <ph id=""1"" subType=""xlf:lb"" type=""fmt""/>World!</source>
                <target>Hello <ph id=""1"" subType=""xlf:lb"" type=""fmt""/>World!</target>
            </segment>
        </unit>
    </file>
</xliff>";

            XliffDocument document = LoadXliff(xliff);
            var inlineprocessing = new InlineCodeProcessing();

            var newDocument = inlineprocessing.ExecuteMerge(document);

            Assert.AreEqual(1, newDocument.Files[0].Containers.Count);
            var unit = newDocument.Files[0].Containers[0] as Unit;

            Assert.AreEqual(1, unit.CollapseChildren<Target>()[0].Text.Count);
            var segment = unit.CollapseChildren<Target>()[0].Text[0] as CDataTag;
            Assert.IsNotNull(segment);
            Assert.AreEqual("<p>Hello <br/>World!</p>", segment.Text);
        }

        [Test()]
        public void TargetWithTwoUnitsWithPcGetConvertedToCDataWithBHtmlTag()
        {
            var xliff = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xliff srcLang=""en-GB"" version=""2.0"" trgLang=""it-IT"" xmlns=""urn:oasis:names:tc:xliff:document:2.0"">
    <file id=""f1"">
        <unit id=""u1"">
            <segment>
                <source>Hello <pc id=""1"" subType=""xlf:b"" type=""fmt"">World 1</pc>!</source>
                <target>Hello <pc id=""1"" subType=""xlf:b"" type=""fmt"">World 1</pc>!</target>
            </segment>
        </unit>
        <unit id=""u2"">
            <segment>
                <source>Hello <pc id=""1"" subType=""xlf:b"" type=""fmt"">World 2</pc>!</source>
                <target>Hello <pc id=""1"" subType=""xlf:b"" type=""fmt"">World 2</pc>!</target>
            </segment>
        </unit>
    </file>
</xliff>";

            XliffDocument document = LoadXliff(xliff);
            var inlineprocessing = new InlineCodeProcessing();

            var newDocument = inlineprocessing.ExecuteMerge(document);

            Assert.AreEqual(2, newDocument.Files[0].Containers.Count);
            var unit1 = newDocument.Files[0].Containers[0] as Unit;
            var unit2 = newDocument.Files[0].Containers[1] as Unit;

            Assert.AreEqual(1, unit1.CollapseChildren<Target>()[0].Text.Count);
            var segment1 = unit1.CollapseChildren<Target>()[0].Text[0] as CDataTag;
            Assert.IsNotNull(segment1);
            Assert.AreEqual("<p>Hello <strong>World 1</strong>!</p>", segment1.Text);

            Assert.AreEqual(1, unit2.CollapseChildren<Target>()[0].Text.Count);
            var segment2 = unit2.CollapseChildren<Target>()[0].Text[0] as CDataTag;
            Assert.IsNotNull(segment2);
            Assert.AreEqual("<p>Hello <strong>World 2</strong>!</p>", segment2.Text);
        }

        [Test()]
        public void TargetWithNestedUnitsWithPcGetConvertedToCDataWithBHtmlTag()
        {
            var xliff = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xliff srcLang=""en-GB"" version=""2.0"" trgLang=""it-IT"" xmlns=""urn:oasis:names:tc:xliff:document:2.0"">
    <file id=""f1"">
        <unit id=""u1"">
            <segment>
                <source>Hello <pc id=""1"" subType=""xlf:b"" type=""fmt"">World 1</pc>!</source>
                <target>Hello <pc id=""1"" subType=""xlf:b"" type=""fmt"">World 1</pc>!</target>
            </segment>
        </unit>
        <group id=""g1"">
            <unit id=""u2"">
                <segment>
                    <source>Hello <pc id=""1"" subType=""xlf:b"" type=""fmt"">World 2</pc>!</source>
                    <target>Hello <pc id=""1"" subType=""xlf:b"" type=""fmt"">World 2</pc>!</target>
                </segment>
            </unit>
        </group>
    </file>
</xliff>";

            XliffDocument document = LoadXliff(xliff);
            var inlineprocessing = new InlineCodeProcessing();

            var newDocument = inlineprocessing.ExecuteMerge(document);

            Assert.AreEqual(2, newDocument.Files[0].Containers.Count);
            var unit1 = newDocument.Files[0].Containers[0] as Unit;
            var group = newDocument.Files[0].Containers[1] as Group;
            var unit2 = group.Containers[0] as Unit;

            Assert.AreEqual(1, unit1.CollapseChildren<Target>()[0].Text.Count);
            var segment1 = unit1.CollapseChildren<Target>()[0].Text[0] as CDataTag;
            Assert.IsNotNull(segment1);
            Assert.AreEqual("<p>Hello <strong>World 1</strong>!</p>", segment1.Text);

            Assert.AreEqual(1, unit2.CollapseChildren<Target>()[0].Text.Count);
            var segment2 = unit2.CollapseChildren<Target>()[0].Text[0] as CDataTag;
            Assert.IsNotNull(segment2);
            Assert.AreEqual("<p>Hello <strong>World 2</strong>!</p>", segment2.Text);
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

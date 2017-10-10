using NUnit.Framework;
using System;
using Localization.Xliff.OM.Serialization;
using System.IO;
using Localization.Xliff.OM.Core;

namespace XliffLib.Test
{
    [TestFixture()]
    public class ParagraphSplitterMergerTests
    {

        [Test()]
        public void SingleParagraphPlainTextUnitIsLeftUntouched()
        {
            var xliff = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xliff srcLang=""en-GB"" trgLang=""it-IT"" version=""2.0"" xmlns=""urn:oasis:names:tc:xliff:document:2.0"">
    <file id=""f1"">
        <unit id=""u1"">
            <segment>
                <source>Hello Word!</source>
                <target>Hello Word!</target>
            </segment>
        </unit>
    </file>
</xliff>";

            XliffDocument document = LoadXliff(xliff);
            var splitter = new ParagraphSplitter();

            var newDocument = splitter.ExecuteMerge(document);

            Assert.AreEqual(1, newDocument.Files[0].Containers.Count);
            var unit = newDocument.Files[0].Containers[0] as Unit;
            Assert.IsNotNull(unit);
        }

        [Test()]
        public void MultipleParagraphPlainTextAreMergedBackIntoOneUnit()
        {
            var xliff = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xliff srcLang=""en-GB"" trgLang=""it-IT"" version=""2.0"" xmlns=""urn:oasis:names:tc:xliff:document:2.0"">
    <file id=""f1"">
        <group id=""u1-g"" name=""original"">
            <unit id=""u1-1"">
                <segment>
                    <source>Hello Word1!</source>
                    <target>Hello Word1!</target>
                </segment>
            </unit>
            <unit id=""u1-2"">
                <segment>
                    <source>Hello Word2!</source>
                    <target>Hello Word2!</target>
                </segment>
            </unit>
            <unit id=""u1-3"">
                <segment>
                    <source>Hello Word3!</source>
                    <target>Hello Word3!</target>
                </segment>
            </unit>
        </group>
    </file>
</xliff>";

            XliffDocument document = LoadXliff(xliff);
            var splitter = new ParagraphSplitter();

            var newDocument = splitter.ExecuteMerge(document);

            Assert.AreEqual(1, newDocument.Files[0].Containers.Count);
            var unit = newDocument.Files[0].Containers[0] as Unit;
            Assert.IsNotNull(unit);
            Assert.AreEqual("u1",unit.Id);
            Assert.AreEqual("original", unit.Name);
            Assert.AreEqual(1,unit.Resources[0].Target.Text.Count);
            Assert.AreEqual("Hello Word1!\r\nHello Word2!\r\nHello Word3!", unit.Resources[0].Target.Text[0].ToString());
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

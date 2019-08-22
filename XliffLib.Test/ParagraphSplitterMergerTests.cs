using NUnit.Framework;
using System;
using Localization.Xliff.OM.Serialization;
using System.IO;
using Localization.Xliff.OM.Core;
using XliffLib.HtmlProcessing;

namespace XliffLib.Test
{
    [TestFixture(typeof(SimpleHtmlParser))]
    [TestFixture(typeof(HierarchicHtmlParser))]
    public class ParagraphSplitterMergerTests<TParser> where TParser : IHtmlParser, new()
    {
        IHtmlParser _htmlParser;

        [SetUp()]
        public void Init()
        {
            _htmlParser = new TParser();
        }

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
            var splitter = new ParagraphSplitter(_htmlParser);

            var newDocument = splitter.ExecuteMerge(document);

            Assert.AreEqual(1, newDocument.Files[0].Containers.Count);
            var unit = newDocument.Files[0].Containers[0] as Unit;
            Assert.IsNotNull(unit);
        }

        [Test()]
        public void SingleParagraphHtmlUnitIsWrappedInP()
        {
            var xliff = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xliff srcLang=""en-GB"" trgLang=""it-IT"" version=""2.0"" xmlns=""urn:oasis:names:tc:xliff:document:2.0"">
    <file id=""f1"">
        <group id=""u1-g"" name=""original"">
            <unit id=""u1-1"" type=""html:p"">
                <segment>
                    <source>Hello Word!</source>
                    <target>Hello Word!</target>
                </segment>
            </unit>
        </group>
    </file>
</xliff>";

            XliffDocument document = LoadXliff(xliff);
            var splitter = new ParagraphSplitter(_htmlParser);

            var newDocument = splitter.ExecuteMerge(document);

            Assert.AreEqual(1, newDocument.Files[0].Containers.Count);
            var unit = newDocument.Files[0].Containers[0] as Unit;
            Assert.IsNotNull(unit);

            Assert.AreEqual("u1", unit.Id);
            Assert.AreEqual("original", unit.Name);

            Assert.AreEqual(1, unit.Resources[0].Target.Text.Count);
            var text = unit.Resources[0].Target.Text[0] as CDataTag;
            Assert.IsNotNull(text);
            Assert.AreEqual("<p>Hello Word!</p>", text.Text);
        }

        [Test()]
        public void UnorderedListInTheRootWithTwoItemIsWrappedInUL()
        {
            var xliff = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xliff srcLang=""en-GB"" trgLang=""it-IT"" version=""2.0"" xmlns=""urn:oasis:names:tc:xliff:document:2.0"">
    <file id=""f1"">
        <group id=""u1-g"" name=""original"">
            <group id=""u1-1-g"" type=""html:ul"">
                <unit id=""u1-1-1"" type=""html:li"">
                    <segment>
                            <source>Hello</source>
                            <target>Hello</target>
                    </segment>
                </unit>
                <unit id=""u1-1-2"" type=""html:li"">
                    <segment>
                            <source>Hello</source>
                            <target>Hello</target>
                    </segment>
                </unit>
            </group>
        </group>
    </file>
</xliff>";

            XliffDocument document = LoadXliff(xliff);
            var splitter = new ParagraphSplitter(_htmlParser);

            var newDocument = splitter.ExecuteMerge(document);

            Assert.AreEqual(1, newDocument.Files[0].Containers.Count);
            var unit = newDocument.Files[0].Containers[0] as Unit;
            Assert.IsNotNull(unit);

            Assert.AreEqual("u1", unit.Id);
            Assert.AreEqual("original", unit.Name);

            Assert.AreEqual(1, unit.Resources[0].Target.Text.Count);
            var text = unit.Resources[0].Target.Text[0] as CDataTag;
            Assert.IsNotNull(text);
            Assert.AreEqual("<ul><li>Hello</li><li>Hello</li></ul>", text.Text);
        }

        [Test()]
        public void UnorderedListInTheRootWithOneItemIsWrappedInUL()
        {
            var xliff = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xliff srcLang=""en-GB"" trgLang=""it-IT"" version=""2.0"" xmlns=""urn:oasis:names:tc:xliff:document:2.0"">
    <file id=""f1"">
        <group id=""u1-g"" name=""original"">
            <group id=""u1-1-g"" type=""html:ul"" >
                <unit id=""u1-1-1"" type=""html:li"">
                    <segment>
                            <source>Hello</source>
                            <target>Hello</target>
                    </segment>
                </unit>
            </group>
        </group>
    </file>
</xliff>";

            XliffDocument document = LoadXliff(xliff);
            var splitter = new ParagraphSplitter(_htmlParser);

            var newDocument = splitter.ExecuteMerge(document);

            Assert.AreEqual(1, newDocument.Files[0].Containers.Count);
            var unit = newDocument.Files[0].Containers[0] as Unit;
            Assert.IsNotNull(unit);

            Assert.AreEqual("u1", unit.Id);
            Assert.AreEqual("original", unit.Name);

            Assert.AreEqual(1, unit.Resources[0].Target.Text.Count);
            var text = unit.Resources[0].Target.Text[0] as CDataTag;
            Assert.IsNotNull(text);
            Assert.AreEqual("<ul><li>Hello</li></ul>", text.Text);
        }

        [Test()]
        public void UnorderedListAsChildWithTwoItemIsWrappedInUL()
        {
            var xliff = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xliff srcLang=""en-GB"" trgLang=""it-IT"" version=""2.0"" xmlns=""urn:oasis:names:tc:xliff:document:2.0"">
    <file id=""f1"">
    <group id=""u1-g"" name=""original"">
      <unit id=""u1-1"" type=""html:p"" >
        <segment>
          <source>whatever</source>
          <target>whatever</target>
        </segment>
      </unit>
      <group id=""u1-2-g"" type=""html:ul"" >
        <unit id= ""u1-2-1"" type=""html:li"">
          <segment>
            <source>Item 1</source>
            <target>Item 1</target>
          </segment>
        </unit>
        <unit id= ""u1-2-2"" type=""html:li"" >
          <segment>
            <source>Item 1</source>
            <target>Item 1</target>
          </segment>
        </unit>
      </group>
    </group>
    </file>
</xliff>";

            XliffDocument document = LoadXliff(xliff);
            var splitter = new ParagraphSplitter(_htmlParser);

            var newDocument = splitter.ExecuteMerge(document);

            Assert.AreEqual(1, newDocument.Files[0].Containers.Count);
            var unit = newDocument.Files[0].Containers[0] as Unit;
            Assert.IsNotNull(unit);

            Assert.AreEqual("u1", unit.Id);
            Assert.AreEqual("original", unit.Name);

            Assert.AreEqual(1, unit.Resources[0].Target.Text.Count);
            var text = unit.Resources[0].Target.Text[0] as CDataTag;
            Assert.IsNotNull(text);
            Assert.AreEqual("<p>whatever</p><ul><li>Item 1</li><li>Item 1</li></ul>", text.Text);
        }

        [Test()]
        public void SingleParagraphHtmlWithFormattingUnitIsWrappedInP()
        {
            var xliff = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xliff srcLang=""en-GB"" trgLang=""it-IT"" version=""2.0"" xmlns=""urn:oasis:names:tc:xliff:document:2.0"">
    <file id=""f1"">
    <group id=""u1-g"" name=""original"">
        <unit id=""u1-1"" type=""html:p"">
            <segment>
                    <source><![CDATA[Hello <b>Word</b>!]]></source>
                    <target><![CDATA[Hello <b>Word</b>!]]></target>
            </segment>
        </unit>
    </group>
    </file>
</xliff>";

            XliffDocument document = LoadXliff(xliff);
            var splitter = new ParagraphSplitter(_htmlParser);

            var newDocument = splitter.ExecuteMerge(document);

            Assert.AreEqual(1, newDocument.Files[0].Containers.Count);
            var unit = newDocument.Files[0].Containers[0] as Unit;
            Assert.IsNotNull(unit);

            Assert.AreEqual("u1", unit.Id);
            Assert.AreEqual("original", unit.Name);

            Assert.AreEqual(1, unit.Resources[0].Target.Text.Count);
            var text = unit.Resources[0].Target.Text[0] as CDataTag;
            Assert.IsNotNull(text);
            Assert.AreEqual("<p>Hello <b>Word</b>!</p>", text.Text);
        }

        [Test()]
        public void OneGroupMultipleParagraphPlainTextAreMergedBackIntoOneUnit()
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
            var splitter = new ParagraphSplitter(_htmlParser);

            var newDocument = splitter.ExecuteMerge(document);

            Assert.AreEqual(1, newDocument.Files[0].Containers.Count);
            var unit = newDocument.Files[0].Containers[0] as Unit;
            Assert.IsNotNull(unit);
            Assert.AreEqual("u1",unit.Id);
            Assert.AreEqual("original", unit.Name);
            Assert.AreEqual(1,unit.Resources[0].Target.Text.Count);
            var text = unit.Resources[0].Target.Text[0] as PlainText;
            Assert.IsNotNull(text);
            Assert.AreEqual("Hello Word1!"+ Environment.NewLine + "Hello Word2!" + Environment.NewLine + "Hello Word3!", text.Text);
        }

        [Test()]
        public void OneGroupWithMultipleParagraphCDataAreMergedBackIntoOneUnit()
        {
            var xliff = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xliff srcLang=""en-GB"" trgLang=""it-IT"" version=""2.0"" xmlns=""urn:oasis:names:tc:xliff:document:2.0"">
    <file id=""f1"">
        <group id=""u1-g"" name=""original"">
            <unit id=""u1-1"" type=""html:p"">
                <segment>
                    <source>Hello Word1!</source>
                    <target>Hello Word1!</target>
                </segment>
            </unit>
            <unit id=""u1-2"" type=""html:p"">
                <segment>
                    <source>Hello Word2!</source>
                    <target>Hello Word2!</target>
                </segment>
            </unit>
            <unit id=""u1-3"" type=""html:p"">
                <segment>
                    <source><![CDATA[Hello <b>Word3</b>!]]></source>
                    <target><![CDATA[Hello <b>Word3</b>!]]></target>
                </segment>
            </unit>
        </group>
    </file>
</xliff>";

            XliffDocument document = LoadXliff(xliff);
            var splitter = new ParagraphSplitter(_htmlParser);

            var newDocument = splitter.ExecuteMerge(document);

            Assert.AreEqual(1, newDocument.Files[0].Containers.Count);
            var unit = newDocument.Files[0].Containers[0] as Unit;
            Assert.IsNotNull(unit);
            Assert.AreEqual("u1", unit.Id);
            Assert.AreEqual("original", unit.Name);
            Assert.AreEqual(1, unit.Resources[0].Target.Text.Count);
            var cdata = unit.Resources[0].Target.Text[0] as CDataTag;
            Assert.IsNotNull(cdata);
            Assert.AreEqual("<p>Hello Word1!</p><p>Hello Word2!</p><p>Hello <b>Word3</b>!</p>", cdata.Text);
        }

        [Test()]
        public void OneGroupWithMultipleParagraphWithNotFormattingCDataAreMergedBackIntoOneUnit()
        {
            var xliff = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xliff srcLang=""en-GB"" trgLang=""it-IT"" version=""2.0"" xmlns=""urn:oasis:names:tc:xliff:document:2.0"">
    <file id=""f1"">
        <group id=""u1-g"" name=""original"">
            <unit id=""u1-1"" type=""html:p"">
                <segment>
                    <source>Hello Word1!</source>
                    <target>Hello Word1!</target>
                </segment>
            </unit>
            <unit id=""u1-2"" type=""html:p"">
                <segment>
                    <source>Hello Word2!</source>
                    <target>Hello Word2!</target>
                </segment>
            </unit>
            <unit id=""u1-3"" type=""html:p"">
                <segment>
                    <source>Hello Word3!</source>
                    <target>Hello Word3!</target>
                </segment>
            </unit>
        </group>
    </file>
</xliff>";

            XliffDocument document = LoadXliff(xliff);
            var splitter = new ParagraphSplitter(_htmlParser);

            var newDocument = splitter.ExecuteMerge(document);

            Assert.AreEqual(1, newDocument.Files[0].Containers.Count);
            var unit = newDocument.Files[0].Containers[0] as Unit;
            Assert.IsNotNull(unit);
            Assert.AreEqual("u1", unit.Id);
            Assert.AreEqual("original", unit.Name);
            Assert.AreEqual(1, unit.Resources[0].Target.Text.Count);
            var cdata = unit.Resources[0].Target.Text[0] as CDataTag;
            Assert.IsNotNull(cdata);
            Assert.AreEqual("<p>Hello Word1!</p><p>Hello Word2!</p><p>Hello Word3!</p>", cdata.Text);
        }

        [Test()]
        public void TwoGroupsWithMultipleParagraphCDataAreMergedBackIntoOneUnit()
        {
            var xliff = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xliff srcLang=""en-GB"" trgLang=""it-IT"" version=""2.0"" xmlns=""urn:oasis:names:tc:xliff:document:2.0"">
    <file id=""f1"">
        <group id=""u1-g"" name=""original1"">
            <unit id=""u1-1"" type=""html:p"">
                <segment>
                    <source>Hello Word1!</source>
                    <target>Hello Word1!</target>
                </segment>
            </unit>
            <unit id=""u1-2"" type=""html:p"">
                <segment>
                    <source>Hello Word2!</source>
                    <target>Hello Word2!</target>
                </segment>
            </unit>
            <unit id=""u1-3"" type=""html:p"">
                <segment>
                    <source><![CDATA[Hello <b>Word3</b>!]]></source>
                    <target><![CDATA[Hello <b>Word3</b>!]]></target>
                </segment>
            </unit>
        </group>
        <group id=""u2-g"" name=""original2"">
            <unit id=""u2-1"" type=""html:p"">
                <segment>
                    <source>Hello2 Word1!</source>
                    <target>Hello2 Word1!</target>
                </segment>
            </unit>
            <unit id=""u2-2"" type=""html:p"">
                <segment>
                    <source>Hello2 Word2!</source>
                    <target>Hello2 Word2!</target>
                </segment>
            </unit>
            <unit id=""u2-3"" type=""html:p"">
                <segment>
                    <source><![CDATA[Hello2 <b>Word3</b>!]]></source>
                    <target><![CDATA[Hello2 <b>Word3</b>!]]></target>
                </segment>
            </unit>
        </group>
    </file>
</xliff>";

            XliffDocument document = LoadXliff(xliff);
            var splitter = new ParagraphSplitter(_htmlParser);

            var newDocument = splitter.ExecuteMerge(document);

            Assert.AreEqual(2, newDocument.Files[0].Containers.Count);
            var unit1 = newDocument.Files[0].Containers[0] as Unit;
            Assert.IsNotNull(unit1);
            Assert.AreEqual("u1", unit1.Id);
            Assert.AreEqual("original1", unit1.Name);
            var unit2 = newDocument.Files[0].Containers[1] as Unit;
            Assert.IsNotNull(unit2);
            Assert.AreEqual("u2", unit2.Id);
            Assert.AreEqual("original2", unit2.Name);

            var cdata1 = unit1.Resources[0].Target.Text[0] as CDataTag;
            Assert.IsNotNull(cdata1);
            Assert.AreEqual("<p>Hello Word1!</p><p>Hello Word2!</p><p>Hello <b>Word3</b>!</p>", cdata1.Text);

            var cdata2 = unit2.Resources[0].Target.Text[0] as CDataTag;
            Assert.IsNotNull(cdata2);
            Assert.AreEqual("<p>Hello2 Word1!</p><p>Hello2 Word2!</p><p>Hello2 <b>Word3</b>!</p>", cdata2.Text);
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

using Localization.Xliff.OM.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XliffLib.Model;
using XliffLib;
using XliffLib.Test.Utils;
using XliffLib.Utils;

namespace XliffLib.Test
{
    [TestFixture]
    public class SourceExtractorTests
    {

        [Test]
        public void NotPassingABundleMakesException()
        {
            ISourceExtractor extractor = new SourceExtractorFromBundle();
            Assert.Throws<ArgumentNullException>(() => extractor.Input = "a string");
        }

        [Test]
        public void BundleWithOneDocumentWritesToXliffDocumentWithOneFile()
        {
            var bundle = new Bundle();
            var doc = new Document();
            bundle.Documents.Add(doc);

            ISourceExtractor extractor = new SourceExtractorFromBundle(bundle);
            var xliffModel = extractor.Extract("en-US","it-IT");

            var actual = xliffModel.Files.Count;
            Assert.AreEqual(1, actual);
        }

        [Test]
        public void FileWithOnePropertyWritesToXliffFileWithOneUnit()
        {
            var bundle = EmbeddedFilesReader.ReadString("XliffLib.Test.TestFiles.OnePropertyInRoot.json").ToBundle();

            ISourceExtractor extractor = new SourceExtractorFromBundle(bundle);
            var xliffModel = extractor.Extract("en-US", "it-IT");

            var actual = xliffModel.Files[0].Containers.Count;
            Assert.AreEqual(1, actual);
            Assert.IsAssignableFrom<Unit>(xliffModel.Files[0].Containers[0]);
        }

        [Test]
        public void FileWithOnePropertyGroupWritesToXliffFileWithOneGroup()
        {
            var bundle = new Bundle();
            var doc = new Document();
            var group = new PropertyGroup("content");
            var prop = new Property("title", "my content");
            group.Containers.Add(prop);
            doc.Containers.Add(group);
            bundle.Documents.Add(doc);

            ISourceExtractor extractor = new SourceExtractorFromBundle(bundle);
            var xliffModel = extractor.Extract("en-US", "it-IT");

            var actual = xliffModel.Files[0].Containers.Count;
            Assert.AreEqual(1, actual);
            Assert.IsAssignableFrom<Group>(xliffModel.Files[0].Containers[0]);
        }

        [Test]
        public void PropertyInsideFirstLevelGroupGetsRightId()
        {
            var bundle = new Bundle();
            var doc = new Document();
            var group = new PropertyGroup("content");
            var prop = new Property("title", "my content");
            group.Containers.Add(prop);
            doc.Containers.Add(group);
            bundle.Documents.Add(doc);

            ISourceExtractor extractor = new SourceExtractorFromBundle(bundle);
            var xliffModel = extractor.Extract("en-US", "it-IT");

            var xliffGroup = xliffModel.Files[0].Containers[0] as Group;
            var actual = xliffGroup.Containers[0].Id;
            Assert.AreEqual("u1", actual);
        }

        [Test]
        public void PropertiesInsideDifferentGroupsGetsDifferentIds()
        {
            var bundle = new Bundle();
            var doc = new Document();
            var group1 = new PropertyGroup("content");
            var prop1 = new Property("title", "my content");
            group1.Containers.Add(prop1);
            doc.Containers.Add(group1);
            var group2 = new PropertyGroup("content");
            var prop2 = new Property("title", "my content");
            group2.Containers.Add(prop2);
            doc.Containers.Add(group2);
            bundle.Documents.Add(doc);

            ISourceExtractor extractor = new SourceExtractorFromBundle(bundle);
            var xliffModel = extractor.Extract("en-US", "it-IT");

            var xliffGroup1 = xliffModel.Files[0].Containers[0] as Group;
            var unit1 = xliffGroup1.Containers[0] as Unit;
            Assert.AreEqual("u1", unit1.Id);
            Assert.AreEqual("#/f=f1/g=g1/u=u1", unit1.SelectorPath);

            var xliffGroup2 = xliffModel.Files[0].Containers[1] as Group;
            var unit2 = xliffGroup2.Containers[0] as Unit;
            Assert.AreEqual("u2", unit2.Id);
            Assert.AreEqual("#/f=f1/g=g2/u=u2", unit2.SelectorPath);
        }

        [Test]
        public void NestedGroupsAreCorrectlyRepresented()
        {
            var bundle = new Bundle();
            var doc = new Document();
            var group1 = new PropertyGroup("content");
            var group2 = new PropertyGroup("nestedContent");
            var prop2 = new Property("title", "my content");
            group2.Containers.Add(prop2);
            group1.Containers.Add(group2);
            doc.Containers.Add(group1);
            bundle.Documents.Add(doc);

            ISourceExtractor extractor = new SourceExtractorFromBundle(bundle);
            var xliffModel = extractor.Extract("en-US", "it-IT");

            var xliffGroup1 = xliffModel.Files[0].Containers[0] as Group;
            var actual = xliffGroup1.Id;
            Assert.AreEqual("g1", actual);

            var xliffGroup2 = xliffGroup1.Containers[0] as Group;
            var actual2 = xliffGroup2.Id;
            Assert.AreEqual("g2", actual2);

            var unit = xliffGroup2.Containers[0] as Unit;
            Assert.IsNotNull(unit);
            Assert.AreEqual("#/f=f1/g=g1/g=g2/u=u1", unit.SelectorPath);
        }

        [Test]
        public void XliffGroupKeepsNameOfPropertyGroup()
        {
            var bundle = new Bundle();
            var doc = new Document();
            var group = new PropertyGroup("content");
            doc.Containers.Add(group);
            bundle.Documents.Add(doc);

            ISourceExtractor extractor = new SourceExtractorFromBundle(bundle);
            var xliffModel = extractor.Extract("en-US", "it-IT");

            var xliffGroup1 = xliffModel.Files[0].Containers[0] as Group;
            var actual = xliffGroup1.Name;
            Assert.AreEqual("content", actual);
        }

        [Test]
        public void XliffUnitKeepsNameOfProperty()
        {
            var bundle = new Bundle();
            var doc = new Document();
            var prop = new Property("content", "my content");
            doc.Containers.Add(prop);
            bundle.Documents.Add(doc);

            ISourceExtractor extractor = new SourceExtractorFromBundle(bundle);
            var xliffModel = extractor.Extract("en-US", "it-IT");

            var unit = xliffModel.Files[0].Containers[0] as Unit;
            var actual = unit.Name;
            Assert.AreEqual("content", actual);
        }

        [Test]
        public void TextValuesAreEncodedAsSimpleText()
        {
            var bundle = new Bundle();
            var doc = new Document();
            var prop = new Property("content", "content");
            doc.Containers.Add(prop);
            bundle.Documents.Add(doc);

            ISourceExtractor extractor = new SourceExtractorFromBundle(bundle);
            var xliffModel = extractor.Extract("en-US", "it-IT");

            var unit = xliffModel.Files[0].Containers[0] as Unit;
            var segment = unit.Resources[0] as Segment;
            var actual = segment.Source.Text[0] as PlainText;
            Assert.IsNotNull(actual);
        }

        [Test]
        public void HtmlValuesAreEncodedIntoCData()
        {
            var bundle = new Bundle();
            var doc = new Document();
            var prop = new Property("content", "<p>content</p>");
            doc.Containers.Add(prop);
            bundle.Documents.Add(doc);

            ISourceExtractor extractor = new SourceExtractorFromBundle(bundle);
            var xliffModel = extractor.Extract("en-US", "it-IT");

            var unit = xliffModel.Files[0].Containers[0] as Unit;
            var segment = unit.Resources[0] as Segment;
            var actual = segment.Source.Text[0] as CDataTag;
            Assert.IsNotNull(actual);
        }
    }
}

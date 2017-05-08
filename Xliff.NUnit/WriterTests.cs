using Localization.Xliff.OM.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XliffLib.Model;
using XliffLib;

namespace Xliff.NUnit
{
    [TestFixture]
    public class WriterTests
    {
        [Test]
        public void BundleWithOneDocumentWritesToXliffDocumentWithOneFile()
        {
            Bundle bundle = new Bundle();
            Document doc = new Document();
            bundle.Documents.Add(doc);

            Extractor extractor = new Extractor();
            var xliffModel = extractor.Create(bundle, "en-US");

            int actual = xliffModel.Files.Count;
            Assert.AreEqual(1, actual);
        }

        [Test]
        public void FileWithOnePropertyWritesToXliffFileWithOneUnit()
        {
            Bundle bundle = new Bundle();
            Document doc = new Document();
            Property prop = new Property("title");
            doc.Properties.Add(prop);
            bundle.Documents.Add(doc);

            Extractor extractor = new Extractor();
            var xliffModel = extractor.Create(bundle, "en-US");

            var actual = xliffModel.Files[0].Containers.Count;
            Assert.AreEqual(1, actual);
            Assert.IsAssignableFrom<Unit>(xliffModel.Files[0].Containers[0]);
        }

        [Test]
        public void FileWithOnePropertyGroupWritesToXliffFileWithOneGroup()
        {
            Bundle bundle = new Bundle();
            Document doc = new Document();
            PropertyGroup group = new PropertyGroup("content");
            Property prop = new Property("title");
            group.Properties.Add(prop);
            doc.PropertyGroups.Add(group);
            bundle.Documents.Add(doc);

            Extractor extractor = new Extractor();
            var xliffModel = extractor.Create(bundle, "en-US");

            var actual = xliffModel.Files[0].Containers.Count;
            Assert.AreEqual(1, actual);
            Assert.IsAssignableFrom<Group>(xliffModel.Files[0].Containers[0]);
        }
        
        [Test]
        public void PropertyInsideFirstLevelGroupGetsRightId()
        {
            Bundle bundle = new Bundle();
            Document doc = new Document();
            PropertyGroup group = new PropertyGroup("content");
            Property prop = new Property("title");
            group.Properties.Add(prop);
            doc.PropertyGroups.Add(group);
            bundle.Documents.Add(doc);

            Extractor extractor = new Extractor();
            var xliffModel = extractor.Create(bundle, "en-US");

            var xliffGroup = xliffModel.Files[0].Containers[0] as Group;
            string actual = xliffGroup.Containers[0].Id;
            Assert.AreEqual("u1", actual);
        }

        [Test]
        public void PropertiesInsideDifferentGroupsGetsDifferentIds()
        {
            Bundle bundle = new Bundle();
            Document doc = new Document();
            PropertyGroup group1 = new PropertyGroup("content");
            Property prop1 = new Property("title");
            group1.Properties.Add(prop1);
            doc.PropertyGroups.Add(group1);
            PropertyGroup group2 = new PropertyGroup("content");
            Property prop2 = new Property("title");
            group2.Properties.Add(prop2);
            doc.PropertyGroups.Add(group2);
            bundle.Documents.Add(doc);

            Extractor extractor = new Extractor();
            var xliffModel = extractor.Create(bundle, "en-US");

            var xliffGroup1 = xliffModel.Files[0].Containers[0] as Group;
            string actual = xliffGroup1.Containers[0].Id;
            Assert.AreEqual("u1", actual);

            var xliffGroup2 = xliffModel.Files[0].Containers[1] as Group;
            string actual2 = xliffGroup2.Containers[0].Id;
            Assert.AreEqual("u2", actual2);
        }
    }
}

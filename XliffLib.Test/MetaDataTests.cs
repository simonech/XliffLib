using NUnit.Framework;
using System;
using XliffLib.Model;
using Localization.Xliff.OM.Modules.Metadata;

namespace XliffLib.Test
{
    [TestFixture()]
    public class MetaDataTests
    {
        [Test()]
        public void NoMetadataInXliffWhenNoAttributesOnDocumnent()
        {
            var bundle = new Bundle();
            var doc = new Document();
            bundle.Documents.Add(doc);

            ISourceExtractor extractor = new SourceExtractorFromBundle(bundle);
            var xliffModel = extractor.Extract("en-US", "it-IT");

            Assert.IsNull(xliffModel.Files[0].Metadata);
        }

        [Test()]
        public void NoMetadataInXliffWhenNoAttributesOnContentElement()
        {
            var bundle = new Bundle();
            var doc = new Document();
            var group = new PropertyGroup("content");
            doc.Containers.Add(group);
            bundle.Documents.Add(doc);

            ISourceExtractor extractor = new SourceExtractorFromBundle(bundle);
            var xliffModel = extractor.Extract("en-US", "it-IT");

            Assert.IsNull(xliffModel.Files[0].Containers[0].Metadata);
        }

        [Test()]
        public void MetadataInXliffWhenAttributesOnDocument()
        {
            var bundle = new Bundle();
            var doc = new Document();
            doc.Attributes.Add("CmsId", "123456");
            bundle.Documents.Add(doc);

            ISourceExtractor extractor = new SourceExtractorFromBundle(bundle);
            var xliffModel = extractor.Extract("en-US", "it-IT");

            Assert.IsNotNull(xliffModel.Files[0].Metadata);
        }

        [Test()]
        public void MetadataInXliffWhenAttributesOnContentElement()
        {
            var bundle = new Bundle();
            var doc = new Document();
            var group = new PropertyGroup("content");
            doc.Containers.Add(group);
            bundle.Documents.Add(doc);
            group.Attributes.Add("CmsPropertyName", "propertyName");

            ISourceExtractor extractor = new SourceExtractorFromBundle(bundle);
            var xliffModel = extractor.Extract("en-US", "it-IT");

            Assert.IsNotNull(xliffModel.Files[0].Containers[0].Metadata);
        }

        [Test()]
        public void MetadataContainsOneGroupInXliffWhenAttributesOnDocument()
        {
            var bundle = new Bundle();
            var doc = new Document();
            doc.Attributes.Add("CmsId", "123456");
            bundle.Documents.Add(doc);

            ISourceExtractor extractor = new SourceExtractorFromBundle(bundle);
            var xliffModel = extractor.Extract("en-US", "it-IT");

            Assert.AreEqual(1, xliffModel.Files[0].Metadata.Groups.Count);
        }

        [Test()]
        public void MetadataContainsOneGroupInXliffWhenAttributesOnContentElement()
        {
            var bundle = new Bundle();
            var doc = new Document();
            var group = new PropertyGroup("content");
            doc.Containers.Add(group);
            bundle.Documents.Add(doc);
            group.Attributes.Add("CmsPropertyName", "propertyName");

            ISourceExtractor extractor = new SourceExtractorFromBundle(bundle);
            var xliffModel = extractor.Extract("en-US", "it-IT");

            Assert.AreEqual(1, xliffModel.Files[0].Containers[0].Metadata.Groups.Count);
        }

        [Test()]
        public void MetadataSectionContainsMetadataInXliffWhenAttributesOnDocument()
        {
            var bundle = new Bundle();
            var doc = new Document();
            doc.Attributes.Add("CmsId", "123456");
            bundle.Documents.Add(doc);

            ISourceExtractor extractor = new SourceExtractorFromBundle(bundle);
            var xliffModel = extractor.Extract("en-US", "it-IT");

            Assert.AreEqual(1, xliffModel.Files[0].Metadata.Groups[0].Containers.Count);
        }

        [Test()]
        public void MetadataSectionContainsMetadataInXliffWhenAttributesOnContentElement()
        {
            var bundle = new Bundle();
            var doc = new Document();
            var group = new PropertyGroup("content");
            doc.Containers.Add(group);
            bundle.Documents.Add(doc);
            group.Attributes.Add("CmsPropertyName", "propertyName");

            ISourceExtractor extractor = new SourceExtractorFromBundle(bundle);
            var xliffModel = extractor.Extract("en-US", "it-IT");

            Assert.AreEqual(1, xliffModel.Files[0].Containers[0].Metadata.Groups[0].Containers.Count);
        }

        [Test()]
        public void MetadataSectionRightValueContainsMetadataInXliffWhenAttributesOnDocument()
        {
            var bundle = new Bundle();
            var doc = new Document();
            doc.Attributes.Add("CmsId", "123456");
            bundle.Documents.Add(doc);

            ISourceExtractor extractor = new SourceExtractorFromBundle(bundle);
            var xliffModel = extractor.Extract("en-US", "it-IT");

            var meta = xliffModel.Files[0].Metadata.Groups[0].Containers[0] as Meta;

            Assert.AreEqual("123456", meta.NonTranslatableText);
            Assert.AreEqual("CmsId", meta.Type);
        }

        [Test()]
        public void MetadataSectionRightValueContainsMetadataInXliffWhenAttributesOnContentElement()
        {
            var bundle = new Bundle();
            var doc = new Document();
            var group = new PropertyGroup("content");
            doc.Containers.Add(group);
            bundle.Documents.Add(doc);
            group.Attributes.Add("CmsPropertyName", "propertyName");

            ISourceExtractor extractor = new SourceExtractorFromBundle(bundle);
            var xliffModel = extractor.Extract("en-US", "it-IT");

            var meta = xliffModel.Files[0].Containers[0].Metadata.Groups[0].Containers[0] as Meta;

            Assert.AreEqual("propertyName", meta.NonTranslatableText);
            Assert.AreEqual("CmsPropertyName", meta.Type);
        }
    }
}

using Localization.Xliff.OM.Core;
using NUnit.Framework;
using System;
using XliffLib.Model;


namespace XliffLib.Test
{
    [TestFixture]
    public class SourceExtractorTests
    {
        [Test]
        public void SourceLanguageIsCorrectlyRepresentedInXliff()
        {
            var bundle = new Bundle();
            var doc = new Document();
            var prop = new Property("content", "my content");
            doc.Containers.Add(prop);
            bundle.Documents.Add(doc);

            ISourceExtractor extractor = new SourceExtractorFromBundle(bundle);
            var xliffModel = extractor.Extract("en-US", "it-IT");

            Assert.AreEqual("en-US", xliffModel.SourceLanguage);
        }

        [Test]
        public void TargetLanguageIsCorrectlyRepresentedInXliff()
        {
            var bundle = new Bundle();
            var doc = new Document();
            var prop = new Property("content", "my content");
            doc.Containers.Add(prop);
            bundle.Documents.Add(doc);

            ISourceExtractor extractor = new SourceExtractorFromBundle(bundle);
            var xliffModel = extractor.Extract("en-US", "it-IT");

            Assert.AreEqual("it-IT", xliffModel.TargetLanguage);
        }
    }
}

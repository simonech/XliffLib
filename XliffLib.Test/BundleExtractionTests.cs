using NUnit.Framework;
using System;
using XliffLib.Model;

namespace XliffLib.Test
{
    [TestFixture()]
    public class BundleExtractionTests
    {
		[Test]
		public void BundleWithOneDocumentWritesToXliffDocumentWithOneFile()
		{
			var bundle = new Bundle();
			var doc = new Document();
			bundle.Documents.Add(doc);

			ISourceExtractor extractor = new SourceExtractorFromBundle(bundle);
			var xliffModel = extractor.Extract("en-US", "it-IT");

			var actual = xliffModel.Files.Count;
			Assert.AreEqual(1, actual);
		}
    }
}

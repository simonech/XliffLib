using NUnit.Framework;
using System;
using XliffLib.Model;
using Localization.Xliff.OM.Core;
using XliffLib.Test.Utils;
using XliffLib.Utils;

namespace XliffLib.Test
{
    [TestFixture()]
    public class FileExtractionTests
    {
		[Test]
		public void FileWithOnePropertyGroupWritesToXliffFileWithOneGroup()
		{
			var bundle = new Bundle();
			var doc = new Document();
			var group = new PropertyGroup("content");
			doc.Containers.Add(group);
			bundle.Documents.Add(doc);

			ISourceExtractor extractor = new SourceExtractorFromBundle(bundle);
			var xliffModel = extractor.Extract("en-US", "it-IT");

			var actual = xliffModel.Files[0].Containers.Count;
			Assert.AreEqual(1, actual);
			Assert.IsAssignableFrom<Group>(xliffModel.Files[0].Containers[0]);
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

    }
}

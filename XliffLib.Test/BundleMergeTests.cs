using NUnit.Framework;
using System;
using Localization.Xliff.OM.Core;
using XliffLib.Test.Utils;
using XliffLib.Model;

namespace XliffLib.Test
{
    [TestFixture()]
    public class BundleMergeTests
    {
		[Test()]
		public void MergerCreatesABundle()
		{

			XliffDocument doc = PrepareXliffForMergeTest.SetupXliffFile();
			IMergerToSource merger = new MergerToBundle();

			merger.Merge(doc);

			var bundle = merger.Output as Bundle;

			Assert.IsNotNull(bundle);
		}

		[Test()]
		public void MergerCreatesABundleWithOneDocIfXliffHasOneFile()
		{
			XliffDocument doc = PrepareXliffForMergeTest.SetupXliffFile();
			IMergerToSource merger = new MergerToBundle();

			merger.Merge(doc);

			var bundle = merger.Output as Bundle;

			Assert.AreEqual(1, bundle.Documents.Count);
		}

		[Test()]
		public void MergerCreatesABundleWithTwoDocsIfXliffHastwoFiles()
		{
			var xliff = @"<?xml version=""1.0"" encoding=""utf-8""?>
                <xliff srcLang=""en-US"" trgLang=""it-IT"" version=""2.0"" xmlns=""urn:oasis:names:tc:xliff:document:2.0"">
                  <file id=""f1"">
                    <unit id=""u1"" name=""title"">
                      <segment>
                        <source>content</source>
                        <target>contenuto tradotto</target>
                      </segment>
                    </unit>
                  </file>
                  <file id=""f2"">
                    <unit id=""u2"" name=""title"">
                      <segment>
                        <source>content</source>
                        <target>contenuto tradotto</target>
                      </segment>
                    </unit>
                  </file>
                </xliff>";


			XliffDocument doc = Merger.Read(xliff);
			IMergerToSource merger = new MergerToBundle();

			merger.Merge(doc);

			var bundle = merger.Output as Bundle;

			Assert.AreEqual(2, bundle.Documents.Count);
		}


    }
}

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
            IMergerToSource merger = new MergerToBundle();
            XliffDocument doc = new XliffDocument("en-US");
            merger.Merge(doc);

            var bundle = merger.Output;

            Assert.IsNotNull(bundle);
        }

        [Test()]
        public void MergerCreatesABundleWithOneDocIfXliffHasOneFile()
        {

            XliffDocument doc = new XliffDocument("en-US");
            var file = new File("f1");
            doc.Files.Add(file);

            IMergerToSource merger = new MergerToBundle();

            merger.Merge(doc);

            var bundle = merger.Output;

            Assert.AreEqual(1, bundle.Documents.Count);
        }

        [Test()]
        public void MergerCreatesABundleWithTwoDocsIfXliffHastwoFiles()
        {
            XliffDocument doc = new XliffDocument("en-US");
            doc.Files.Add(new File("f1"));
            doc.Files.Add(new File("f2"));

            IMergerToSource merger = new MergerToBundle();

            merger.Merge(doc);

            var bundle = merger.Output;

            Assert.AreEqual(2, bundle.Documents.Count);
        }
    }
}

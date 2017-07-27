using NUnit.Framework;
using System;
using Localization.Xliff.OM.Core;
using XliffLib.Test.Utils;
using XliffLib.Model;

namespace XliffLib.Test
{
    [TestFixture()]
    public class FileMergeTests
    {
        [Test()]
        public void MergerCreatesBundleWithCorrectOriginal()
        {
            XliffDocument doc = PrepareXliffForMergeTest.SetupXliffFile();
            IMergerToSource merger = new MergerToBundle();

            merger.Merge(doc);

            var bundle = merger.Output as Bundle;

            Assert.AreEqual("cmsId", bundle.Documents[0].SourceIdentifier);
        }

        [Test()]
        public void MergerCreatesBundleWithOnePropertyIfXliffHasOneUnit()
        {
            XliffDocument doc = PrepareXliffForMergeTest.SetupXliffFile();
            IMergerToSource merger = new MergerToBundle();

            merger.Merge(doc);

            var bundle = merger.Output as Bundle;

            Assert.AreEqual(1, bundle.Documents[0].Containers.Count);
            Assert.IsInstanceOf<Property>(bundle.Documents[0].Containers[0]);
        }

        [Test()]
        public void MergerCreatesBundleWithOnePropertyGroupIfXliffHasOneGroup()
        {
            XliffDocument doc = PrepareXliffForMergeTest.SetupXliffFile(withGroup: true);
            IMergerToSource merger = new MergerToBundle();

            merger.Merge(doc);

            var bundle = merger.Output as Bundle;

            Assert.AreEqual(1, bundle.Documents[0].Containers.Count);
            Assert.IsInstanceOf<PropertyGroup>(bundle.Documents[0].Containers[0]);
        }
    }
}

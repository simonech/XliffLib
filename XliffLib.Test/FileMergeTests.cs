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
            var file = new File("f1");
            file.Original = "cmsId";

            var doc = Document.FromXliff(file);

            Assert.AreEqual("cmsId", doc.SourceIdentifier);
        }

        [Test()]
        public void MergerCreatesBundleWithOnePropertyIfXliffHasOneUnit()
        {
            var file = new File("f1");
			var unit = new Unit("u1");
			var segment = new Segment();
			segment.Target = new Target("testo tradotto");
			unit.Resources.Add(segment);
            file.Containers.Add(unit);

            var doc = Document.FromXliff(file);

            Assert.AreEqual(1, doc.Containers.Count);
            Assert.IsInstanceOf<Property>(doc.Containers[0]);
        }

        [Test()]
        public void MergerCreatesBundleWithOnePropertyGroupIfXliffHasOneGroup()
        {
			var file = new File("f1");
			var group = new Group("g1");
			file.Containers.Add(group);

            var doc = Document.FromXliff(file);

            Assert.AreEqual(1, doc.Containers.Count);
            Assert.IsInstanceOf<PropertyGroup>(doc.Containers[0]);
        }
    }
}

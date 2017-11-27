using NUnit.Framework;
using System;
using Localization.Xliff.OM.Core;
using XliffLib.Model;

namespace XliffLib.Test
{
    [TestFixture()]
    public class PropertyGroupMergeTests
    {
        [Test()]
        public void XliffGroupNameIsMergedIntoPropertyGroup()
        {
            var group = new Group("g1");
            group.Name = "title";

            PropertyGroup propertyGroup = PropertyGroup.FromXliff(group) as PropertyGroup;

            Assert.AreEqual("title", propertyGroup.Name);
        }

        [Test()]
        public void MergerCreatesBundleWithOnePropertyGroupWithProperties()
        {
            var group = new Group("g1");
            group.Name = "title";
            var unit = new Unit("u1");

            var segment = new Segment();
            segment.Target = new Target("testo tradotto");
            unit.Resources.Add(segment);

            group.Containers.Add(unit);


            PropertyGroup propertyGroup = PropertyGroup.FromXliff(group) as PropertyGroup;

            Assert.AreEqual(1, propertyGroup.Containers.Count);
        }

        [Test()]
        public void PropertyWithEmptyCDataTargetDoesntGetAddedToContainersList()
        {
            var group = new Group("g1");
            group.Name = "title";
            var unit = new Unit("u1");

            var segment = new Segment();
            segment.Target = new Target();
            segment.Target.Text.Add(new CDataTag(""));
            unit.Resources.Add(segment);
            group.Containers.Add(unit);

            PropertyGroup propertyGroup = PropertyGroup.FromXliff(group) as PropertyGroup;
            Assert.AreEqual(0, propertyGroup.Containers.Count);
        }
    }
}

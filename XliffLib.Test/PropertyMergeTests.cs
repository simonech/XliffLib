using NUnit.Framework;
using System;
using Localization.Xliff.OM.Core;
using XliffLib.Model;

namespace XliffLib.Test
{
    [TestFixture()]
    public class PropertyMergeTests
    {
        [Test()]
        public void XliffNameIsMergedIntoProperty()
        {
			var unit = new Unit("u1");
            unit.Name = "title";
			var segment = new Segment();
			segment.Target = new Target("testo tradotto");
			unit.Resources.Add(segment);

            Property property = Property.FromXliff(unit) as Property;

            Assert.AreEqual("title", property.Name);
        }

		[Test()]
		public void XliffContentIsMergedIntoProperty()
		{
			var unit = new Unit("u1");
            var segment = new Segment();
            segment.Target = new Target("testo tradotto");
            unit.Resources.Add(segment);

            Property property = Property.FromXliff(unit) as Property;

            Assert.AreEqual("testo tradotto", property.Value);
        }

		[Test()]
		public void XliffCDataContentIsMergedIntoProperty()
		{
			var unit = new Unit("u1");
			var segment = new Segment();
			segment.Target = new Target();
            segment.Target.Text.Add(new CDataTag("<p>testo tradotto</p>"));
			unit.Resources.Add(segment);

			Property property = Property.FromXliff(unit) as Property;

			Assert.AreEqual("<p>testo tradotto</p>", property.Value);
		}
    }
}

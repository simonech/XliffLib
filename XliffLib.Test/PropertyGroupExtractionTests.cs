using NUnit.Framework;
using System;
using XliffLib.Model;
using Localization.Xliff.OM.Core;

namespace XliffLib.Test
{
    [TestFixture()]
    public class PropertyGroupExtractionTests
    {

		IdCounter _idCounter;

		[SetUp()]
		public void SetupCointer()
		{
			_idCounter = new IdCounter();
		}


		[Test]
		public void GroupNameIsKeeptOnPropertyGroup()
		{
			var propertyGroup = new PropertyGroup("content");

			var group = propertyGroup.ToXliff(_idCounter) as Group;
			var actual = group.Name;
			Assert.AreEqual("content", actual);
		}

		[Test]
		public void NestedPropertyGroupsAreCorrectlyRepresented()
		{

			var group1 = new PropertyGroup("content");
			var group2 = new PropertyGroup("nestedContent");
			var prop2 = new Property("title", "my content");
			group2.Containers.Add(prop2);
			group1.Containers.Add(group2);


            var xliffGroup1 = group1.ToXliff(_idCounter) as Group;

			var actual = xliffGroup1.Id;
			Assert.AreEqual("g1", actual);

			var xliffGroup2 = xliffGroup1.Containers[0] as Group;
			var actual2 = xliffGroup2.Id;
			Assert.AreEqual("g2", actual2);

			var unit = xliffGroup2.Containers[0] as Unit;
			Assert.IsNotNull(unit);
			Assert.AreEqual("g=g1/g=g2/u=u1", unit.SelectorPath);
		}



		[Test]
		public void GroupDoesntHaveMetadataIfProperyGroupHasNotAttributes()
		{
			var propGroup = new PropertyGroup("content");

			var group = propGroup.ToXliff(_idCounter) as Group;
			Assert.IsNull(group.Metadata);
		}

		[Test]
		public void PropertyGroupAttributesAreStoredInUnitAsMetadata()
		{
			var propGroup = new PropertyGroup("content");
			propGroup.Attributes.Add("CMSID", "12345");

			var group = propGroup.ToXliff(_idCounter) as Group;
			Assert.IsNotNull(group.Metadata);
			Assert.IsTrue(group.Metadata.HasGroups);
		}
    }
}

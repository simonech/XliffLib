using NUnit.Framework;
using System;
using XliffLib.Model;
using Localization.Xliff.OM.Core;

namespace XliffLib.Test
{
    [TestFixture()]
    public class PropertyExtractionTests
    {

        IdCounter _idCounter;

        [SetUp()]
        public void SetupCointer()
        {
            _idCounter = new IdCounter();
        }

        [Test]
        public void PropertyNameIsKeptInXlffUnit()
        {
            var prop = new Property("content", "my content");

            var unit = prop.ToXliff(_idCounter) as Unit;
            var actual = unit.Name;
            Assert.AreEqual("content", actual);
        }


        [Test()]
        public void HTMLContentIsEncodedAsCData()
        {
            var prop = new Property("content", "<p>content</p>");

            var unit = prop.ToXliff(_idCounter) as Unit;
            var segment = unit.Resources[0] as Segment;
            var actual = segment.Source.Text[0] as CDataTag;
            Assert.IsNotNull(actual);
        }

        [Test]
        public void TextContentIsEncodedAsSimpleText()
        {
            var prop = new Property("content", "content");

            var unit = prop.ToXliff(_idCounter) as Unit;
            var segment = unit.Resources[0] as Segment;
            var actual = segment.Source.Text[0] as PlainText;
            Assert.IsNotNull(actual);
        }

        [Test]
        public void PropertyValueIsStoredInSegment()
        {
            var prop = new Property("content", "my content");

            var unit = prop.ToXliff(_idCounter) as Unit;
            var segment = unit.Resources[0] as Segment;
            var actual = segment.Source.Text[0] as PlainText;
            Assert.AreEqual("my content", actual.Text);
        }

        [Test]
        public void PropertyIdIsTakenFromCounter()
        {
            var prop = new Property("content", "my content");

            var unit = prop.ToXliff(_idCounter) as Unit;
            Assert.AreEqual("u1", unit.Id);
        }

        [Test]
        public void SecondPropertyIdIsTakenFromCounter()
        {
            var prop1 = new Property("content", "my content");
            var prop2 = new Property("content", "my content");

            var unit1 = prop1.ToXliff(_idCounter) as Unit;
            var unit2 = prop2.ToXliff(_idCounter) as Unit;
            Assert.AreEqual("u2", unit2.Id);
        }

        [Test]
        public void UnitDoesntHaveMetadataIfProperyHasNotAttributes()
        {
            var prop = new Property("content", "my content");

            var unit = prop.ToXliff(_idCounter) as Unit;
            Assert.IsNull(unit.Metadata);
        }

        [Test]
        public void PropertyAttributesAreStoredInUnitAsMetadata()
        {
            var prop = new Property("content", "my content");
            prop.Attributes.Add("CMSID", "12345");

            var unit = prop.ToXliff(_idCounter) as Unit;
            Assert.IsNotNull(unit.Metadata);
            Assert.IsTrue(unit.Metadata.HasGroups);
        }

    }
}

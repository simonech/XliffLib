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

        IdCounter _idCounter;

        [SetUp()]
        public void SetupCointer()
        {
            _idCounter = new IdCounter();
        }

        [Test]
        public void FileWithOnePropertyGroupWritesToXliffFileWithOneGroup()
        {
            var doc = new Document();
            var group = new PropertyGroup("content");
            doc.Containers.Add(group);

            var file = doc.ToXliff(_idCounter) as File;

            var actual = file.Containers.Count;
            Assert.AreEqual(1, actual);
            Assert.IsAssignableFrom<Group>(file.Containers[0]);
        }

        [Test]
        public void FileWithOnePropertyWritesToXliffFileWithOneUnit()
        {

            var doc = new Document();
            var prop = new Property("content", "value");
            doc.Containers.Add(prop);

            var file = doc.ToXliff(_idCounter) as File;

            var actual = file.Containers.Count;
            Assert.AreEqual(1, actual);
            Assert.IsAssignableFrom<Unit>(file.Containers[0]);
        }

    }
}

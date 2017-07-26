using NUnit.Framework;
using System;
using Localization.Xliff.OM.Core;
using XliffLib.Model;

namespace XliffLib.Test
{
    [TestFixture()]
    public class MergerTests
    {

        private XliffDocument SetupXliffFile(bool withCData = false, bool withGroup = false)
        {
            var contentValue = "contenuto tradotto";
            if (withCData)
                contentValue = "<![CDATA[<p>Ciao Mondo!</p>]]>";

            var xliff = @"<?xml version=""1.0"" encoding=""utf-8""?>
                <xliff srcLang=""en-US"" trgLang=""it-IT"" version=""2.0"" xmlns=""urn:oasis:names:tc:xliff:document:2.0"">
                  <file id=""f1"" original=""cmsId"">
                    <unit id=""u1"" name=""title"">
                      <segment>
                        <source>content</source>
                        <target>" + contentValue + @"</target>
                      </segment>
                    </unit>
                  </file>
                </xliff>";

            if (withGroup)
                xliff = @"<?xml version=""1.0"" encoding=""utf-8""?>
                <xliff srcLang=""en-US"" trgLang=""it-IT"" version=""2.0"" xmlns=""urn:oasis:names:tc:xliff:document:2.0"">
                  <file id=""f1"" original=""cmsId"">
                    <group id=""g1"" name=""Content"">
                    <unit id=""u1"" name=""title"">
                      <segment>
                        <source>content</source>
                        <target>" + contentValue + @"</target>
                      </segment>
                    </unit>
                    </group>
                  </file>
                </xliff>";

            return Merger.Read(xliff);
        }

        [Test()]
        public void MergerCreatesABundle()
        {

            XliffDocument doc = SetupXliffFile();
            IMergerToSource merger = new MergerToBundle();

            merger.Merge(doc);

            var bundle = merger.Output as Bundle;

            Assert.IsNotNull(bundle);
        }

        [Test()]
        public void MergerCreatesABundleWithOneDocIfXliffHasOneFile()
        {
            XliffDocument doc = SetupXliffFile();
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

        [Test()]
        public void MergerCreatesBundleWithCorrectOriginal()
        {
            XliffDocument doc = SetupXliffFile();
            IMergerToSource merger = new MergerToBundle();

            merger.Merge(doc);

            var bundle = merger.Output as Bundle;

            Assert.AreEqual("cmsId", bundle.Documents[0].SourceIdentifier);
        }


        [Test()]
        public void MergerCreatesBundleWithOnePropertyIfXliffHasOneUnit()
        {
            XliffDocument doc = SetupXliffFile();
            IMergerToSource merger = new MergerToBundle();

            merger.Merge(doc);

            var bundle = merger.Output as Bundle;

            Assert.AreEqual(1, bundle.Documents[0].Containers.Count);
            Assert.IsInstanceOf<Property>(bundle.Documents[0].Containers[0]);
        }


        [Test()]
        public void MergerCreatesBundleWithOnePropertyGroupIfXliffHasOneGroup()
        {
            XliffDocument doc = SetupXliffFile(withGroup: true);
            IMergerToSource merger = new MergerToBundle();

            merger.Merge(doc);

            var bundle = merger.Output as Bundle;

            Assert.AreEqual(1, bundle.Documents[0].Containers.Count);
            Assert.IsInstanceOf<PropertyGroup>(bundle.Documents[0].Containers[0]);
        }

        [Test()]
        public void MergerCreatesBundleWithOnePropertyGroupWithRightName()
        {
            XliffDocument doc = SetupXliffFile(withGroup: true);
            IMergerToSource merger = new MergerToBundle();

            merger.Merge(doc);

            var bundle = merger.Output as Bundle;
            var group = bundle.Documents[0].Containers[0] as PropertyGroup;

            Assert.AreEqual("Content", group.Name);
        }

        [Test()]
        public void MergerCreatesBundleWithOnePropertyGroupWithProperties()
        {
            XliffDocument doc = SetupXliffFile(withGroup: true);
            IMergerToSource merger = new MergerToBundle();

            merger.Merge(doc);

            var bundle = merger.Output as Bundle;
            var group = bundle.Documents[0].Containers[0] as PropertyGroup;

            Assert.AreEqual(1, group.Containers.Count);
        }

    }
}

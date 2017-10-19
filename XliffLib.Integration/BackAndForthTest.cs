using System;
using System.Xml;
using Localization.Xliff.OM.Core;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using XliffLib.Integration.Utils;
using XliffLib.Utils;

namespace XliffLib.Integration
{
    [TestFixture]
    public class BackAndForthTest
    {
        [Test, TestCaseSource(typeof(DataSamples), "BackForthTests")]
        public void CanExtractAndMerge(string filename)
        {
            var bundleString = EmbeddedFilesReader.ReadString("XliffLib.Integration.TestFiles." + filename + ".json");
            var bundle = bundleString.ToBundle();

            var extractor = new DefaultExtractor();
            var xliffModel = extractor.Extract(bundle, "en-US", "it-IT");

            var xliffString = extractor.Write(xliffModel, true);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xliffString);

            var allSources = doc.GetElementsByTagName("source");
            foreach (XmlNode source in allSources)
            {
                var newTarget = doc.CreateElement("target", "urn:oasis:names:tc:xliff:document:2.0");
                newTarget.InnerXml = source.InnerXml;
                source.ParentNode.AppendChild(newTarget);
            }

            var newXliff = doc.OuterXml;

            var merger = new DefaultMerger();

            xliffModel = merger.Read(newXliff);

            var resultingBundle = merger.Merge(xliffModel);
            var jsonResult = resultingBundle.ToJson();

            JObject expected = JObject.Parse(bundleString);
            JObject result = JObject.Parse(jsonResult);


            Assert.IsTrue(JToken.DeepEquals(expected, result), "The two bundles are different:\r\nExpected {0}\r\nResult {1}", bundleString, jsonResult);
        }
    }
}

using System;
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
        [Test, TestCaseSource(typeof(DataSamples), "FileNamesSimpleExtractor")]
        public void CanExtractAndMerge(string filename)
        {
            var bundleString = EmbeddedFilesReader.ReadString("XliffLib.Integration.TestFiles." + filename + ".json");
            var bundle = bundleString.ToBundle();

            var extractor = new SimpleExtractor();
            var xliffModel = extractor.Extract(bundle, "en-US", "it-IT");

            foreach (var unit in xliffModel.CollapseChildren<Unit>())
            {
                foreach (var res in unit.Resources)
                {
                    res.Target = new Target();
                    foreach (var source in res.Source.Text)
                    {
                        var plainText = source as PlainText;
                        if (plainText != null)
                        {
                            res.Target.Text.Add(new PlainText(plainText.Text));
                        }

                        var cDataText = source as CDataTag;
                        if (cDataText != null)
                        {
                            res.Target.Text.Add(new CDataTag(cDataText.Text));
                        }
                    }
                }
            }

            var merger = new SimpleMerger();

            var resultingBundle = merger.Merge(xliffModel);
            var jsonResult = resultingBundle.ToJson();

            JObject expected = JObject.Parse(bundleString);
            JObject result = JObject.Parse(jsonResult);


            Assert.IsTrue(JToken.DeepEquals(expected, result), "The two bundles are different:\r\nExpected {0}\r\nResult {1}", bundle, jsonResult);
        }
    }
}

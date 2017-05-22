using NUnit.Framework;
using System;
using XliffLib.Integration.Utils;
using Localization.Xliff.OM.Core;
using XliffLib.Utils;
using System.Collections;

namespace XliffLib.Integration
{
    [TestFixture()]
    public class SimpleExtractorTests
    {
		[Test(), TestCaseSource(typeof(DataSamples), "FileNames")]
        public void CanExtractSimpleFile(string filename)
        {
			var bundle = EmbeddedFilesReader.ReadString("XliffLib.Integration.TestFiles."+filename +".json").ToBundle();
            var xliff = EmbeddedFilesReader.ReadString("XliffLib.Integration.TestFiles." + filename + ".xlf");

			SimpleExtractor extractor = new SimpleExtractor();
			var xliffModel = extractor.Extract(bundle,"en-US");

            var xliffString = extractor.Write(xliffModel,true);

            string cleanedExpected = System.Text.RegularExpressions.Regex.Replace(xliff, @"\s+", " ");
            string cleanedResult = System.Text.RegularExpressions.Regex.Replace(xliffString, @"\s+", " ");

            Assert.AreEqual(cleanedExpected,cleanedResult);
        }
    }

    internal class DataSamples
    {
		public static IEnumerable FileNames
		{
			get
			{
				yield return new TestCaseData("OnePropertyInRoot");
                yield return new TestCaseData("OneNestedProperty");
			}
		}
    }
}

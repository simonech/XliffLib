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
            var xliff = EmbeddedFilesReader.ReadString("XliffLib.Integration.TestFiles." + filename + ".xlf").Replace("  ", " ");

			SimpleExtractor extractor = new SimpleExtractor();
			var xliffModel = extractor.Extract(bundle,"en-US");

            var xliffString = extractor.Write(xliffModel,true);

            Assert.AreEqual(xliff,xliffString);
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

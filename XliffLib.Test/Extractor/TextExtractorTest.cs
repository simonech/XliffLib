using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XliffLib.Extractors;

namespace XliffLib.Test.Extractor
{
    [TestClass]
    public class TextExtractorTest
    {
        [TestMethod]
        public void SingleLineDocProducesOneSegmentWithinOneUnit()
        {
            TextExtractor extractor = new TextExtractor();
            ExtractorResult result = extractor.Extract("Hello World!");
            Assert.AreEqual(1, result.File.Units.Count);
            Assert.AreEqual(1, result.File.Units[0].Segments.Count);
        }

        [TestMethod]
        public void SingleLineDocProducesSkeletonWithOnePlaceholder()
        {
            TextExtractor extractor = new TextExtractor();
            ExtractorResult result = extractor.Extract("Hello World!");
            Assert.AreEqual("%%1%%", result.Skeleton);
        }

        [TestMethod]
        public void SingleLineDocProducesCorrectSegmentContent()
        {
            TextExtractor extractor = new TextExtractor();
            ExtractorResult result = extractor.Extract("Hello World!");
            Assert.AreEqual("Hello World!", result.File.Units[0].Segments[0].Source);
        }
    }
}

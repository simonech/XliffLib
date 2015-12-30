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

        [TestMethod]
        public void LineEndingWithSpaceProducesCorrectSegmentContent()
        {
            TextExtractor extractor = new TextExtractor();
            ExtractorResult result = extractor.Extract("Hello World! ");
            Assert.AreEqual("Hello World!", result.File.Units[0].Segments[0].Source);
        }

        [TestMethod]
        public void LineEndingWithoutSeparatorProducesCorrectSegmentContent()
        {
            TextExtractor extractor = new TextExtractor();
            ExtractorResult result = extractor.Extract("Hello World");
            Assert.AreEqual("Hello World", result.File.Units[0].Segments[0].Source);
        }

        [TestMethod]
        public void MultiLineDocProducesCorrectSegmentsContent()
        {
            TextExtractor extractor = new TextExtractor();
            ExtractorResult result = extractor.Extract("First Sentence. Second Sentence.");
            Assert.AreEqual("First Sentence.", result.File.Units[0].Segments[0].Source);
            Assert.AreEqual("Second Sentence.", result.File.Units[0].Segments[1].Source);
        }

        [TestMethod]
        public void MultiLineDocWithUnterminatedLastLineProducesCorrectSegmentsContent()
        {
            TextExtractor extractor = new TextExtractor();
            ExtractorResult result = extractor.Extract("First Sentence. Second Sentence");
            Assert.AreEqual("First Sentence.", result.File.Units[0].Segments[0].Source);
            Assert.AreEqual("Second Sentence", result.File.Units[0].Segments[1].Source);
        }

        [TestMethod]
        public void MultiLineDocProducesOneSegmentWithinTwoUnits()
        {
            TextExtractor extractor = new TextExtractor();
            ExtractorResult result = extractor.Extract("First Sentence. Second Sentence.");
            Assert.AreEqual(1, result.File.Units.Count);
            Assert.AreEqual(2, result.File.Units[0].Segments.Count);
        }
    }
}

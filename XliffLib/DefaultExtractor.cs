using System;
namespace XliffLib
{
    public class DefaultExtractor : Extractor
    {
        public DefaultExtractor() : base(new SourceExtractorFromBundle())
        {
            ProcessingSteps.AddLast(new ParagraphSplitter());
            ProcessingSteps.AddLast(new InlineCodeProcessing());
        }
    }
}

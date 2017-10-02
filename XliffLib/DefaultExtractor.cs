using System;
namespace XliffLib
{
    public class DefaultExtractor : Extractor
    {
        public DefaultExtractor() : base(new SourceExtractorFromBundle())
        {
            ProcessingSteps.Add(new CDataSplitter());
            ProcessingSteps.Add(new InlineCodeProcessing());
        }
    }
}

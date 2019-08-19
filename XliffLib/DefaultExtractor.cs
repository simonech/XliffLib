using System;
using XliffLib.HtmlProcessing;

namespace XliffLib
{
    public class DefaultExtractor : Extractor
    {
        public DefaultExtractor() : base(new SourceExtractorFromBundle())
        {
            ProcessingSteps.AddLast(new ParagraphSplitter(new SimpleHtmlParser()));
            ProcessingSteps.AddLast(new InlineCodeProcessing());
        }
    }
}

using System;
using XliffLib.HtmlProcessing;

namespace XliffLib
{
    public class DefaultExtractor : Extractor
    {
        public DefaultExtractor() : this(new HierarchicHtmlParser())
        { }

        public DefaultExtractor(IHtmlParser htmlParser) : base(new SourceExtractorFromBundle())
        {
            ProcessingSteps.AddLast(new ParagraphSplitter(htmlParser));
            ProcessingSteps.AddLast(new InlineCodeProcessing());
        }
    }
}

namespace XliffLib
{
    public class SimpleExtractor : Extractor
    {
        public SimpleExtractor() : base(new SourceExtractorFromBundle())
        {
        }
    }
}

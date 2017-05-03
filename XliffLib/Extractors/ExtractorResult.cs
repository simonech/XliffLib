using XliffLib.Model;

namespace XliffLib.Extractors
{
    public class ExtractorResult
    {
        public ExtractorResult()
        {
            File = new Document();
        }
        public string Skeleton { get; set; }
        public Document File { get; set; }
    }
}
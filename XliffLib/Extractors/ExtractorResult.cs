using XliffLib.Model;

namespace XliffLib.Extractors
{
    public class ExtractorResult
    {
        public ExtractorResult()
        {
            File = new XliffFile();
        }
        public string Skeleton { get; set; }
        public XliffFile File { get; set; }
    }
}
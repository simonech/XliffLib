namespace XliffLib.Model
{
    public class XliffSegments
    {
        public XliffSegments()
        {
            Id = string.Empty;
        }
        public XliffSegments(string id)
        {
            Id = id;
        }
        public string Source { get; set; }
        public string Id { get; private set; }
    }
}
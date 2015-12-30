using System.Collections.Generic;

namespace XliffLib.Model
{
    public class XliffUnit
    {
        public XliffUnit(string id)
        {
            Segments = new List<XliffSegments>();
            Id = id;
        }
        public IList<XliffSegments> Segments { get; set; }
        public string Id { get; private set; }  
    }
}
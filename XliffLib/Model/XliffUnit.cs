using System.Collections.Generic;

namespace XliffLib.Model
{
    public class XliffUnit
    {
        public XliffUnit()
        {
            Segments = new List<XliffSegments>();
        }
        public IList<XliffSegments> Segments { get; set; }
    }
}
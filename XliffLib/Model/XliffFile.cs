using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XliffLib.Model
{
    public class XliffFile
    {
        public XliffFile()
        {
            Units = new List<XliffUnit>();
        }
        public IList<XliffUnit> Units { get; set; }
    }
}
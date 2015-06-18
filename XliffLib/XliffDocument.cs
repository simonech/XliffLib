using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XliffLib
{
    public class XliffDocument
    {

        public XliffDocument()
        {
            Files = new List<XliffFile>();
        }

        public IList<XliffFile> Files { get; set; }
    }
}

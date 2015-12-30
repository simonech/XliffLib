using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XliffLib.Model
{
    /// <summary>
    /// Logical structure of the Xliff document
    /// </summary>
    public class XliffDocument
    {

        public XliffDocument()
        {
            Files = new List<XliffFile>();
        }

        public IList<XliffFile> Files { get; set; }
    }
}

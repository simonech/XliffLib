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
    public class Bundle : ContentElement
    {
        public Bundle() : base()
        {
            Documents = new List<Document>();
        }

        public IList<Document> Documents { get; private set; }
    }
}

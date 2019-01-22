using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Localization.Xliff.OM;
using Localization.Xliff.OM.Core;

namespace XliffLib.Model
{
    /// <summary>
    /// A Bundle represents a translation request.
    /// </summary>
    public class Bundle
    {
        public Bundle()
        {
            Documents = new List<Document>();
        }

        /// <summary>
        /// The various documents that are part of translation request.
        /// </summary>
        /// <value>The documents part of the bundle.</value>
        public IList<Document> Documents { get; private set; }
    }
}

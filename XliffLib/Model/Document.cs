using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XliffLib.Model
{
    public class Document
    {
        public Document()
        {
            ContentItems = new List<IContentItem>();
        }
        public IList<IContentItem> ContentItems { get; private set; }
    }
}
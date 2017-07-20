using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XliffLib.Model
{
    public class Document : ContentElement
    {
        public Document() : base()
        {
            Containers = new List<PropertyContainer>();
        }

        public string SourceIdentifier { get; set; }

        public IList<PropertyContainer> Containers { get; private set; }
    }
}
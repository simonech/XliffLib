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
            PropertyGroups = new List<PropertyGroup>();
            Properties = new List<Property>();
        }
        public IList<PropertyGroup> PropertyGroups { get; private set; }
        public IList<Property> Properties { get; private set; }
    }
}
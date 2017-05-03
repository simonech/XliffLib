using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XliffLib.Model
{
    public class PropertyGroup
    {
        public PropertyGroup(string id)
        {
            PropertyGroups = new List<PropertyGroup>();
            Properties = new List<Property>();
            Id = id;
        }
        public IList<PropertyGroup> PropertyGroups { get; private set; }
        public IList<Property> Properties { get; private set; }
        public string Id { get; private set; }
        public string Name { get; set; }
    }
}

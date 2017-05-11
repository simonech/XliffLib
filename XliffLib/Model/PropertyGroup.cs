using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XliffLib.Model
{
    public class PropertyGroup: IPropertyContainer
    {
        public PropertyGroup(string name)
        {
            PropertyGroups = new List<PropertyGroup>();
            Properties = new List<Property>();
            Name = name;
        }
        public IList<PropertyGroup> PropertyGroups { get; private set; }
        public IList<Property> Properties { get; private set; }
        public string Name { get; set; }
    }
}

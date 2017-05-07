using System.Collections.Generic;

namespace XliffLib.Model
{
    public interface IPropertyContainer
    {
        IList<PropertyGroup> PropertyGroups { get; }
        IList<Property> Properties { get; }
    }
}
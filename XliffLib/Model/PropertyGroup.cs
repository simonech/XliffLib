using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Localization.Xliff.OM;
using Localization.Xliff.OM.Core;

namespace XliffLib.Model
{
    public class PropertyGroup : PropertyContainer
    {
        public PropertyGroup(string name) : base(name)
        {
            Containers = new List<PropertyContainer>();
        }
        public IList<PropertyContainer> Containers { get; private set; }

        public static new PropertyContainer FromXliff(TranslationContainer xliffGroup)
        {
            var group = xliffGroup as Group;
            var propertyGroup = new PropertyGroup(xliffGroup.Name);
            if (group.Metadata != null)
                propertyGroup.Attributes = AttributeList.FromXliffMetadata(group.Metadata);

            foreach (var container in group.Containers)
            {
                propertyGroup.Containers.Add(PropertyContainer.FromXliff(container));
            }

            return propertyGroup;
        }

        public override XliffElement ToXliff(IdCounter idCounter)
        {
            var id = "g" + (idCounter.GetNextGroupId());
            var xliffGroup = new Group(id)
            {
                Name = this.Name,
                Metadata=Attributes.ToXliffMetadata()
            };

            foreach (var container in Containers)
            {
                var xliffContainer = container.ToXliff(idCounter) as TranslationContainer;
                xliffGroup.Containers.Add(xliffContainer);
            }
            return xliffGroup;
        }
    }
}

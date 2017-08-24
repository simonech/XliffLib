using System;
using System.Collections.Generic;
using Localization.Xliff.OM.Modules.Metadata;
using System.Linq;

namespace XliffLib.Model
{
    public class AttributeList : Dictionary<string, string>
    {
        public AttributeList()
        {
        }

        public MetadataContainer ToXliffMetadata()
        {
            if (this.Count == 0)
            {
                return null;
            }

            var metadata = new MetadataContainer();

            var defaultGroup = new MetaGroup()
            {
                Id = "XliffLib"
            };

            foreach (var attribute in this)
            {
                var metadataItem = new Meta(attribute.Key, attribute.Value);
                defaultGroup.Containers.Add(metadataItem);
            }

            metadata.Groups.Add(defaultGroup);
            return metadata;
        }

        public static AttributeList FromXliffMetadata(MetadataContainer metadata)
        {
            if (metadata != null && metadata.HasGroups)
            {
                MetaGroup defaultGroup = null;
                foreach (var group in metadata.Groups)
                {
                    if (group.Id == "XliffLib")
                    {
                        defaultGroup = group;
                        break;
                    }
                }
                if (defaultGroup != null)
                {
                    var attributes = new AttributeList();
                    foreach (Meta item in defaultGroup.Containers.OfType<Meta>())
                    {
                        attributes.Add(item.Type, item.NonTranslatableText);
                    }
                    return attributes;
                }
            }

            return null;
        }
    }
}

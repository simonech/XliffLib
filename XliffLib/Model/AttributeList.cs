using System;
using System.Collections.Generic;
using Localization.Xliff.OM.Modules.Metadata;

namespace XliffLib.Model
{
    public class AttributeList: Dictionary<string, string>
    {
        public AttributeList()
        {
        }

        public MetadataContainer ToXliffMetadata()
        {
			var metadata = new MetadataContainer();

			var defaultGroup = new MetaGroup()
			{
				Id = "XliffLib"
			};

			foreach (var attribute in this)
			{
				var metadataItem = new Meta(attribute.Key, attribute.Value);
				defaultGroup.Containers.Add((metadataItem));
			}

			metadata.Groups.Add(defaultGroup);
            return metadata;
        }
    }
}

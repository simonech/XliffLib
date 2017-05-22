using System.Collections.Generic;
using Localization.Xliff.OM.Core;
using XliffLib.Model;
using XliffLib.Utils;
using System;

namespace XliffLib
{
    public class SourceExtractorFromBundle: ISourceExtractor
    {

        public SourceExtractorFromBundle()
        {

        }
        public SourceExtractorFromBundle(Bundle sourceDocument)
        {
            Input = sourceDocument;
        }

        public object Input
        {
            get {
                return Xliff;
            }
            set {

                var xliff = value as Bundle;

				if (xliff == null)
				{
                    throw new ArgumentNullException(nameof(value), "Input value should be of type XliffLib.Model.Bundle.");

			    }
                Xliff = xliff;
            }
        }

        public Bundle Xliff
        {
            get;
            set;
        }

        public XliffDocument Extract(string sourceLanguage){
            return Extract(Xliff, sourceLanguage);
        }

		private XliffDocument Extract(Bundle xliff, string sourceLanguage)
		{
			XliffDocument document = new XliffDocument(sourceLanguage);

			int fileNum = 0;
			int groupNum = 0;
			int unitNum = 0;
			foreach (var doc in xliff.Documents)
			{
				string fileId = "f" + (++fileNum);
				File xliffFile = new File(fileId);

				var containers = ProcessPropertyContainers(doc, ref groupNum, ref unitNum);
				xliffFile.Containers.AddAll(containers);
				document.Files.Add(xliffFile);
			}
			return document;
		}

		static Unit ProcessProperty(Property property, ref int unitNum)
		{
			string unitId = "u" + (++unitNum);
			Unit xliffUnit = new Unit(unitId);
			xliffUnit.Name = property.Name;

			Segment segment = new Segment();
			if (property.Value.IsHtml())
			{
				var source = new Source();
				source.Text.Add(new CDataTag(property.Value));
				segment.Source = source;
			}
			else
				segment.Source = new Source(property.Value);


			xliffUnit.Resources.Add(segment);
			return xliffUnit;
		}

		IList<TranslationContainer> ProcessPropertyContainers(IPropertyContainer propertyContainer, ref int groupNum, ref int unitNum)
		{
			var containers = new List<TranslationContainer>();
			foreach (var group in propertyContainer.PropertyGroups)
			{
				string id = "g" + (++groupNum);
				Group xliffGroup = new Group(id);
				xliffGroup.Name = group.Name;
				containers.Add(xliffGroup);
				xliffGroup.Containers.AddAll(ProcessPropertyContainers(group, ref groupNum, ref unitNum));
			}
			foreach (var property in propertyContainer.Properties)
			{
				Unit xliffUnit = ProcessProperty(property, ref unitNum);

				containers.Add(xliffUnit);
			}
			return containers;
		}
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Localization.Xliff.OM;
using Localization.Xliff.OM.Core;

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

        public override XliffElement ToXliff(IdCounter idCounter)
        {
            var fileId = "f" + idCounter.GetNextFileId();
            var xliffFile = new File(fileId);
            xliffFile.Original = this.SourceIdentifier;

            if (this.Attributes.Count > 0)
            {
                xliffFile.Metadata = this.Attributes.ToXliffMetadata();
            }

            foreach (var container in this.Containers)
            {
                var xliffContainer = container.ToXliff(idCounter) as TranslationContainer;
                xliffFile.Containers.Add(xliffContainer);
            }

            return xliffFile;
        }
    }
}
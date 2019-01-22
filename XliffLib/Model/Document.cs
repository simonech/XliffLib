using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Localization.Xliff.OM;
using Localization.Xliff.OM.Core;

namespace XliffLib.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class Document : ContentElement
    {
        public Document() : base()
        {
            Containers = new List<PropertyContainer>();
        }

        /// <summary>
        /// Gets or sets the identifier for the document in the source system.
        /// It could, for example, be the ID or a guid in the CMS from which the document is exported from and imported to.
        /// </summary>
        /// <value>The source identifier.</value>
        public string SourceIdentifier { get; set; }

        /// <summary>
        /// Gets the containers.
        /// </summary>
        /// <value>The containers.</value>
        public IList<PropertyContainer> Containers { get; private set; }


        public override XliffElement ToXliff(IdCounter idCounter)
        {
            var fileId = "f" + idCounter.GetNextFileId();
            var xliffFile = new File(fileId)
            {
                Original = this.SourceIdentifier,
                Metadata = Attributes.ToXliffMetadata()
            };

            foreach (var container in this.Containers)
            {
                var xliffContainer = container.ToXliff(idCounter) as TranslationContainer;
                xliffFile.Containers.Add(xliffContainer);
            }

            return xliffFile;
        }


        public static Document FromXliff(File file)
        {
            var document = new Document();
            document.SourceIdentifier = file.Original;
            if (file.Metadata != null)
            {
                document.Attributes = AttributeList.FromXliffMetadata(file.Metadata);
            }

            foreach (var container in file.Containers)
            {
                var propertyContainer = PropertyContainer.FromXliff(container);
                if (propertyContainer != null)
                {
                    document.Containers.Add(propertyContainer);
                }
            }

            return document;
        }
    }
}
using System;
using Localization.Xliff.OM.Core;
using XliffLib.Model;

namespace XliffLib
{
    public class MergerToBundle: IMergerToSource
    {
        public MergerToBundle()
        {
        }

		private Bundle Bundle
		{
			get;
			set;
		}

        public object Output 
        {
            get 
            {
                return Bundle;
            }
        }

        public void Merge(XliffDocument xliff)
        {
            Bundle = new Bundle();
            foreach (var file in xliff.Files)
            {
                var document = new Document();
                document.SourceIdentifier = file.Original;

                foreach (var container in file.Containers)
                {
                    var propertyContainer = PropertyContainer.FromXliff(container);
                    document.Containers.Add(propertyContainer);
                }

                Bundle.Documents.Add(document);
            }
        }
    }
}

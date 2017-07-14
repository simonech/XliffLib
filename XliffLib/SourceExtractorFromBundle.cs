using System;
using System.Collections.Generic;
using Localization.Xliff.OM.Core;
using XliffLib.Model;
using XliffLib.Utils;

namespace XliffLib
{
    public class SourceExtractorFromBundle : ISourceExtractor
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
            get
            {
                return Xliff;
            }
            set
            {

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

        public XliffDocument Extract(string sourceLanguage, string targetLanguage)
        {
            return Extract(Xliff, sourceLanguage,targetLanguage);
        }

        private XliffDocument Extract(Bundle xliff, string sourceLanguage, string targetLanguage)
        {
            var document = new XliffDocument(sourceLanguage);
            document.TargetLanguage = targetLanguage;

            var idCounter = new IdCounter();
            foreach (var doc in xliff.Documents)
            {
                var fileId = "f" + (idCounter.GetNextFileId());
                var xliffFile = new File(fileId);

                var containers = ProcessPropertyContainers(doc.Containers, idCounter);
                xliffFile.Containers.AddAll(containers);
                document.Files.Add(xliffFile);
            }
            return document;
        }

        IList<TranslationContainer> ProcessPropertyContainers(IList<PropertyContainer> propertyContainers, IdCounter counter)
        {
            var containers = new List<TranslationContainer>();

            foreach (var container in propertyContainers)
            {
                var xliffContainer = container.ToXliff(counter);
                containers.Add(xliffContainer);
            }

            return containers;
        }
    }
}
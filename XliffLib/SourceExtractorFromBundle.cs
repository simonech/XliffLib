using System;
using System.Collections.Generic;
using Localization.Xliff.OM.Core;
using Localization.Xliff.OM.Modules.Metadata;
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


        public Bundle Input
        {
            get;
            set;
        }

        public XliffDocument Extract(string sourceLanguage, string targetLanguage)
        {
            return Extract(Input, sourceLanguage, targetLanguage);
        }

        private XliffDocument Extract(Bundle xliff, string sourceLanguage, string targetLanguage)
        {
            var document = new XliffDocument(sourceLanguage);
            document.TargetLanguage = targetLanguage;

            var idCounter = new IdCounter();
            foreach (var doc in xliff.Documents)
            {
                document.Files.Add(doc.ToXliff(idCounter) as File);
            }
            return document;
        }
    }
}
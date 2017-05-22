using System;
using System.Collections.Generic;
using Localization.Xliff.OM.Core;
using IO=System.IO;
using Localization.Xliff.OM.Serialization;
using System.Linq;

namespace XliffLib
{
    public class Extractor
    {
		public IList<IProcessingStep> ProcessingSteps
		{
			get;
			private set;
		}

        public ISourceExtractor SourceExtractor
        {
            get;
            set;
        }

        public Extractor(ISourceExtractor sourceExtractor)
        {
            SourceExtractor = sourceExtractor;
            ProcessingSteps = new List<IProcessingStep>();
        }

        public string Write(XliffDocument document, bool indent = false)
        {
            string result = String.Empty;
            using (IO.Stream stream = new IO.MemoryStream())
			{
				XliffWriter writer;

                var settings = new XliffWriterSettings();
                settings.Indent = indent;

				writer = new XliffWriter(settings);
				writer.Serialize(stream, document);
                stream.Position = 0;
				var sr = new IO.StreamReader(stream);
				result = sr.ReadToEnd();
            }

            return result;
        }

        public XliffDocument Extract(object sourceDocument, string sourceLanguage)
        {
            SourceExtractor.Input = sourceDocument;
            XliffDocument document = SourceExtractor.Extract(sourceLanguage);

            ProcessingSteps.OrderBy(s => s.Order);

            foreach (var step in ProcessingSteps)
            {
                document = step.ExecuteExtraction(document);
            }

            return document;
        }

    }
}

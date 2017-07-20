using System;
using System.Collections.Generic;
using System.IO;
using Localization.Xliff.OM.Core;
using Localization.Xliff.OM.Serialization;

namespace XliffLib
{
    public class Merger
    {

		public IList<IProcessingStep> ProcessingSteps
		{
			get;
			private set;
		}

		public IMergerToSource SourceMerger
		{
			get;
			set;
		}

		public Merger(IMergerToSource sourceMerger)
        {
            SourceMerger = sourceMerger;
            ProcessingSteps = new List<IProcessingStep>();
        }

		public static XliffDocument Read(string xliff)
		{
			XliffDocument document = null;
			using (Stream stream = GenerateStreamFromString(xliff))
			{
				var reader = new XliffReader();
				document = reader.Deserialize(stream);
			}

			return document;
		}

		private static Stream GenerateStreamFromString(string s)
		{
			var stream = new MemoryStream();
			var writer = new StreamWriter(stream);
			writer.Write(s);
			writer.Flush();
			stream.Position = 0;
			return stream;
		}

    }
}

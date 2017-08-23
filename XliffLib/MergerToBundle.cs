using System;
using Localization.Xliff.OM.Core;
using XliffLib.Model;

namespace XliffLib
{
    public class MergerToBundle : IMergerToSource
    {
        public Bundle Output
        {
            get;
            private set;
        }

        public string TargetLanguage { get; private set;  }

        public void Merge(XliffDocument xliff)
        {
            Output = new Bundle();
            TargetLanguage = xliff.TargetLanguage;
            foreach (var file in xliff.Files)
            {
                Output.Documents.Add(Document.FromXliff(file));
            }
        }
    }
}

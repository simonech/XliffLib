using System;
using Localization.Xliff.OM.Core;
using XliffLib.Model;

namespace XliffLib
{
    public class MergerToBundle : IMergerToSource
    {
        public MergerToBundle()
        {
        }

        public Bundle Output
        {
            get;
            set;
        }

        public void Merge(XliffDocument xliff)
        {
            Output = new Bundle();
            foreach (var file in xliff.Files)
            {
                Output.Documents.Add(Document.FromXliff(file));
            }
        }
    }
}

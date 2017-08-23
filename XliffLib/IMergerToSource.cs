using System;
using Localization.Xliff.OM.Core;
using XliffLib.Model;

namespace XliffLib
{
    public interface IMergerToSource
    {
        void Merge(XliffDocument xliff);
        Bundle Output { get; }
        string TargetLanguage { get; }
    }
}

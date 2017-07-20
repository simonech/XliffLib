using System;
using Localization.Xliff.OM.Core;

namespace XliffLib
{
    public interface IMergerToSource
    {
        void Merge(XliffDocument xliff);
        object Output { get; }
    }
}

using Localization.Xliff.OM.Core;

namespace XliffLib
{
    public interface IProcessingStep
    {
        XliffDocument ExecuteExtraction(XliffDocument document);
        XliffDocument ExecuteMerge(XliffDocument document);
    }
}
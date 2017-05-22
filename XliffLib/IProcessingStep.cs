using Localization.Xliff.OM.Core;

namespace XliffLib
{
    public interface IProcessingStep
    {
        int Order { get; }
        XliffDocument ExecuteExtraction(XliffDocument document);
		XliffDocument ExecuteMerge(XliffDocument document);
    }
}
using Localization.Xliff.OM.Core;

namespace XliffLib
{
    public interface IProcessingStep
    {
        XliffDocument Execute(XliffDocument document);
    }
}
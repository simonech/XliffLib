using Localization.Xliff.OM.Core;

namespace XliffLib
{
    public interface ISourceExtractor
    {
        object Input{get; set;}
        XliffDocument Extract(string sourceLanguage);
    }
}
using Localization.Xliff.OM.Core;
using XliffLib.Model;

namespace XliffLib
{
    public interface ISourceExtractor
    {
        Bundle Input { get; set; }
        XliffDocument Extract(string sourceLanguage, string targetLanguage);
    }
}
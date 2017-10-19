using NUnit.Framework;
using System.Collections;

namespace XliffLib.Integration
{
    internal static class DataSamples
    {
        public static IEnumerable FileNamesSimpleExtractor
        {
            get
            {
                yield return new TestCaseData("OnePropertyInRoot");
                yield return new TestCaseData("OneNestedProperty");
                yield return new TestCaseData("OnePropertyWithAttributes");
            }
        }

        public static IEnumerable FileNamesDefaultExtractor
        {
            get
            {
                yield return new TestCaseData("OnePropertyInRoot");
                yield return new TestCaseData("OneNestedProperty");
                yield return new TestCaseData("OnePropertyWithAttributes");
                yield return new TestCaseData("HtmlMarkupInProperty");
            }
        }

        public static IEnumerable BackForthTests
        {
            get
            {
                yield return new TestCaseData("OnePropertyInRoot");
                yield return new TestCaseData("OneNestedProperty");
                yield return new TestCaseData("OnePropertyWithAttributes");
                yield return new TestCaseData("HtmlMarkupInProperty");
                yield return new TestCaseData("ULMarkupInProperty");
                yield return new TestCaseData("ComplexMarkupInProperty");
            }
        }
    }
}

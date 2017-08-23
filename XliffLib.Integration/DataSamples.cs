using NUnit.Framework;
using System.Collections;

namespace XliffLib.Integration
{
    internal class DataSamples
    {
        public static IEnumerable FileNames
        {
            get
            {
                yield return new TestCaseData("OnePropertyInRoot");
                yield return new TestCaseData("OneNestedProperty");
                yield return new TestCaseData("OnePropertyWithAttributes");
            }
        }
    }
}

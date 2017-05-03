using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Xliff.NUnit
{
    [TestFixture] 
    public class SampleTest
    {
        [Test]
        public void CanAddTwoNumbers()
        {
            int expected = 4;
            int result = 2 + 2;
            Assert.AreEqual(expected, result);
        }
    }
}

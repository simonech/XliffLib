using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Linq;
using XliffLib.Writers;
using XliffLib.Model;

namespace XliffLib.Test
{
    [TestClass]
    public class XliffWriterV12Test
    {
        private XliffWriterV12 SetupWriter()
        {
            return SetupWriter(new XliffDocument());
        }
        private XliffWriterV12 SetupWriter(XliffDocument doc)
        {
            var writer = new XliffWriterV12();
            writer.Create(doc);
            return writer;
        }

        [TestMethod]
        public void CanGenerateXmlDoc()
        {
            var writer = SetupWriter();
            Assert.IsNotNull(writer.XliffDoc);
        }

        [TestMethod]
        public void XmlDocContainsDefinition()
        {
            var writer = SetupWriter();
            string xliffDoc = writer.ToXmlString();
            Assert.IsTrue(xliffDoc.Contains("<?xml"));
        }

        [TestMethod]
        public void XmlDocContainsXliffSpecVersion()
        {
            var writer = SetupWriter();
            string xliffDoc = writer.ToXmlString();
            Assert.IsTrue(xliffDoc.Contains("<xliff version=\"1.2\""));
        }
    }
}

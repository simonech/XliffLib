using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace XliffLib.Writers
{
    public class XliffWriterV12: BaseXliffWriter
    {
        private static XNamespace xliffNS="urn:oasis:names:tc:xliff:document:1.2";


        /// <summary>
        /// Creates the XML document with the representation of the XLIFF
        /// </summary>
        /// <returns>The XLIFF document</returns>
        public void Create(XliffDocument xliff)
        {
            ValidateXliff(xliff);
            XDeclaration declaration = new XDeclaration("1.0", "utf-8", "");
            XDocument root = new XDocument(declaration);
            var xliffRoot = new XElement(xliffNS+"xliff");
            xliffRoot.Add(new XAttribute("version", "1.2"));
            root.Add(xliffRoot);
            XliffDoc = root;
        }

        private void ValidateXliff(XliffDocument xliff)
        {
            if (xliff == null) throw new ArgumentNullException("xliff");
            if (xliff.Files.Count == 0) throw new ArgumentException("xliff", "The Xliff document must have at least 1 file");
        }
    }
}

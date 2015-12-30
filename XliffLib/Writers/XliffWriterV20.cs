using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Schema;
using XliffLib.Model;
using XliffLib.Utils;

namespace XliffLib.Writers
{
    public class XliffWriterV20: BaseXliffWriter
    {
        private static XNamespace xliffNS = "urn:oasis:names:tc:xliff:document:2.0";

        public void Create(XliffDocument xliff)
        {
            ValidateXliff(xliff);
            XDeclaration declaration = new XDeclaration("1.0", "utf-8", "");
            XDocument root = new XDocument(declaration);
            var xliffRoot = new XElement(xliffNS + "xliff");
            xliffRoot.Add(new XAttribute("version", "2.0"));
            root.Add(xliffRoot);
            XliffDoc = root;
        }
    }
}

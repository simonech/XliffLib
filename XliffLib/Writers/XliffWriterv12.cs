using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using XliffLib.Model;
using XliffLib.Utils;

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

            foreach (var file in xliff.Files)
            {
                var xliffFile = new XElement(xliffNS + "file");
                foreach (var unit in file.Units)
                {
                    foreach (var segment in unit.Segments)
                    {
                        var transUnit = new XElement(xliffNS + "trans-unit");
                        transUnit.Add(new XAttribute("id", unit.Id));

                        var source = new XElement(xliffNS + "source");
                        source.Value = segment.Source;
                        transUnit.Add(source);

                        xliffFile.Add(transUnit);
                    }
                }
                xliffRoot.Add(xliffFile);
            }
        }

    }
}

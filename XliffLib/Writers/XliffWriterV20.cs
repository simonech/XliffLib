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

            foreach (var file in xliff.Files)
            {
                var xliffFile = new XElement(xliffNS + "file");
                foreach (var unit in file.Units)
                {
                    var transUnit = new XElement(xliffNS + "unit");
                    transUnit.Add(new XAttribute("id", unit.Id));

                    foreach (var segment in unit.Segments)
                    {
                        var xliffSegment = new XElement(xliffNS + "segment");
                        xliffSegment.Add(new XAttribute("id", unit.Id));

                        var source = new XElement(xliffNS + "source");
                        source.Value = segment.Source;
                        xliffSegment.Add(source);

                        transUnit.Add(xliffSegment);
                    }
                    xliffFile.Add(transUnit);
                }
                xliffRoot.Add(xliffFile);
            }
        }
    }
}

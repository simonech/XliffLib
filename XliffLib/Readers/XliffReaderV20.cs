using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using XliffLib.Utils;

namespace XliffLib.Readers
{
    public class XliffReaderV20: BaseXliffReader
    {
        private readonly XmlSchemaSet schema;

        public XliffReaderV20(): base()
        {
            schema = new XmlSchemaSet();
            schema.Add(null, XmlReader.Create(new StringReader(EmbeddedFilesReader.ReadString("XliffLib.XsdSchemas.xliff-core-2.0.xsd"))));
            schema.Add("http://www.w3.org/XML/1998/namespace", XmlReader.Create(new StringReader(EmbeddedFilesReader.ReadString("XliffLib.XsdSchemas.xml.xsd"))));
        }

        internal override XmlSchemaSet XsdSchema
        {
            get
            {
                return schema;
            }
        }
    }
}

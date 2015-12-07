using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Schema;

namespace XliffLib.Readers
{
    public abstract class BaseXliffReader
    {
        /// <summary>
        /// Underlying XML document
        /// </summary>
        public XDocument XliffDoc { get; protected set; }

        internal abstract XmlSchemaSet XsdSchema { get; }

        internal void ValidateSchema()
        {
            bool errors = false;
            XliffDoc.Validate(XsdSchema, (o, e) => {
                Console.WriteLine("{0}", e.Message);
                errors = true;
            });
            Console.WriteLine("Xliff {0}", errors ? "did not validate" : "validated");
        }

    }
}

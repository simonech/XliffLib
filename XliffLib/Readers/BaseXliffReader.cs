using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
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

        public bool IsValid { get; set; }

        public IList<ValidationError> ValidationErrors { get; }

        public BaseXliffReader()
        {
            ValidationErrors = new List<ValidationError>();
            IsValid = false;
        }


        public void Parse(string xmlFile)
        {
            Read(new StringReader(xmlFile));
        }

        public void Read(string filename)
        {
            Read(new StreamReader(filename));
        }

        public void Read(TextReader input)
        {

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Schemas = XsdSchema;
            settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;

            using (XmlReader vreader = XmlReader.Create(input, settings))
            {
                try
                {
                    while (vreader.Read()) { }
                }
                catch (XmlException e)
                {
                    IsValid = false;
                    ValidationErrors.Add(new ValidationError() { Type = ErrorType.Syntax, Severity = XmlSeverityType.Error, ErrorMessage = e.Message, LineNumber = e.LineNumber, ColNumber = e.LinePosition });
                }
                
            };

        }

        private void ValidationCallBack(object sender, ValidationEventArgs args)
        {
            IsValid = false;
            ValidationErrors.Add(new ValidationError() { Type = ErrorType.Validation, Severity = args.Severity, ErrorMessage = args.Message, LineNumber = args.Exception.LineNumber, ColNumber = args.Exception.LinePosition });
        }



    }
}

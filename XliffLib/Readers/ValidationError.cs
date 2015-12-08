using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace XliffLib.Readers
{
    public class ValidationError
    {
        public XmlSeverityType Severity { get; set; }
        public string ErrorMessage { get; set; }
        public int LineNumber { get; set; }
        public int ColNumber { get; set; }
        public ErrorType Type { get; set; }
    }

    public enum ErrorType
    {
        Syntax,
        Validation
    }
}

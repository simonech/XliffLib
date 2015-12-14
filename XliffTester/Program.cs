using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XliffLib.Readers;

namespace XliffTester
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Validating v1.2 file");
            XliffReaderV12 reader = new XliffReaderV12();
            reader.Read(@"C:\Projects\XliffLib\sample xliff\Hello-World\xliff-output-v1.2.xml");

            if (!reader.IsValid)
            {
                foreach (var error in reader.ValidationErrors)
                {
                    Console.WriteLine("[{0} - {1} ({2},{3})] {4}", error.Type, error.Severity, error.LineNumber, error.ColNumber, error.ErrorMessage);
                }
            }

            Console.WriteLine("Validating v2.0 file");
            XliffReaderV20 reader2 = new XliffReaderV20();
            reader2.Read(@"C:\Projects\XliffLib\sample xliff\Hello-World\xliff-output-v2.0.xml");

            if (!reader2.IsValid)
            {
                foreach (var error in reader2.ValidationErrors)
                {
                    Console.WriteLine("[{0} - {1} ({2},{3})] {4}", error.Type, error.Severity, error.LineNumber, error.ColNumber, error.ErrorMessage);
                }
            }

            Console.WriteLine("Validating v2.0 string");
            XliffReaderV20 reader3 = new XliffReaderV20();
            reader3.Parse(@"<?xml version='1.0' encoding='UTF-8'?><xliff xmlns='urn:oasis:names:tc:xliff:document:2.0' version='2.0' srcLang='en' trgLang='it'>  <file>    <unit>      <segment id='1'>        <source>Hello Word!</source>      </segment>    </unit>  </file></xliff>");

            if (!reader3.IsValid)
            {
                foreach (var error in reader3.ValidationErrors)
                {
                    Console.WriteLine("[{0} - {1} ({2},{3})] {4}", error.Type, error.Severity, error.LineNumber, error.ColNumber, error.ErrorMessage);
                }
            }

            Console.ReadLine();
        }
    }
}

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
            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;

            //once you have the path you get the directory with:
            var directory = System.IO.Path.GetDirectoryName(path);
            reader.Read(Path.Combine(directory, "Samples", "xliff-output-v1.2.xml"));

            DisplayValidationErrors(reader);

            Console.WriteLine("Validating v2.0 file");
            XliffReaderV20 reader2 = new XliffReaderV20();
            reader2.Read(Path.Combine(directory, "Samples", "xliff-output-v2.0.xml"));

            DisplayValidationErrors(reader2);

            Console.WriteLine("Validating v2.0 string");
            XliffReaderV20 reader3 = new XliffReaderV20();
            reader3.Parse(@"<?xml version='1.0' encoding='UTF-8'?><xliff xmlns='urn:oasis:names:tc:xliff:document:2.0' version='2.0' srcLang='en' trgLang='it'>  <file>    <unit>      <segment id='1'>        <source>Hello Word!</source>      </segment>    </unit>  </file></xliff>");

            DisplayValidationErrors(reader3);

            Console.ReadLine();
        }

        private static void DisplayValidationErrors(BaseXliffReader reader)
        {
            if (!reader.IsValid)
            {
                foreach (var error in reader.ValidationErrors)
                {
                    Console.WriteLine("[{0} - {1} ({2},{3})] {4}", error.Type, error.Severity, error.LineNumber, error.ColNumber, error.ErrorMessage);
                }
            }
            else
                Console.WriteLine("All good!");
        }
    }
}

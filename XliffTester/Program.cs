using System;
using System.Collections.Generic;
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
            XliffReaderV12 reader = new XliffReaderV12();
            reader.Read(@"C:\Projects\XliffLib\sample xliff\Hello-World\xliff-output-v1.2.xml");

            if (!reader.IsValid)
            {
                foreach (var error in reader.ValidationErrors)
                {
                    Console.WriteLine("[{0} - {1} ({2},{3})] {4}", error.Type, error.Severity, error.LineNumber, error.ColNumber, error.ErrorMessage);
                }
            }

            XliffReaderV20 reader2 = new XliffReaderV20();
            reader2.Read(@"C:\Projects\XliffLib\sample xliff\Hello-World\xliff-output-v2.0.xml");

            if (!reader2.IsValid)
            {
                foreach (var error in reader2.ValidationErrors)
                {
                    Console.WriteLine("[{0} - {1} ({2},{3})] {4}", error.Type, error.Severity, error.LineNumber, error.ColNumber, error.ErrorMessage);
                }
            }

            Console.ReadLine();
        }
    }
}

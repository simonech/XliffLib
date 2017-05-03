using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XliffLib.Extractors;
using XliffLib.Model;
using XliffLib.Readers;
using XliffLib.Writers;

namespace XliffTester
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var directory = System.IO.Path.GetDirectoryName(path);
            string content = File.ReadAllText(Path.Combine(directory, "Samples", "original.txt"));

            TextExtractor extractor = new TextExtractor();
            var result = extractor.Extract(content);

            Bundle doc = new Bundle();
            doc.Documents.Add(result.File);

            XliffWriterV20 writer2 = new XliffWriterV20();
            writer2.Create(doc,"en-us");
            writer2.Save(Path.Combine(directory, "Samples", "original-v20.xml"));

            File.WriteAllText(Path.Combine(directory, "Samples", "skeleton.txt"), result.Skeleton);

            //DisplayValidationErrors(reader3);

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

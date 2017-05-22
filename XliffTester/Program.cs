using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Localization.Xliff.OM.Core;
using XliffLib;
using XliffLib.Model;
using Localization.Xliff.OM.Exceptions;

namespace XliffTester
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var directory = System.IO.Path.GetDirectoryName(path);
            string content = System.IO.File.ReadAllText(Path.Combine(directory, "Samples", "original.txt"));


            var bundle = new Bundle();
            var doc = new Document();
            bundle.Documents.Add(doc);
            Property property = new Property("original", content);
            doc.Properties.Add(property);

            //Extractor extractor = new SimpleExtractor();
            Extractor extractor = new DefaultExtractor();
            XliffDocument xliff = extractor.Extract(bundle, "en-GB");


            try
            {
                var result = extractor.Write(xliff,true);
                Console.WriteLine(result);
            }
			catch (ValidationException e)
			{
				Console.WriteLine("ValidationException Details:");
                Console.WriteLine(e.Message);
				if (e.Data != null)
				{
					foreach (object key in e.Data.Keys)
					{
						Console.WriteLine("  '{0}': '{1}'", key, e.Data[key]);
					}
				}
			}


            Console.ReadLine();
        }


    }
}

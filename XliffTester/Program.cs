using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XliffLib.Model;

namespace XliffTester
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var directory = System.IO.Path.GetDirectoryName(path);
            string content = File.ReadAllText(Path.Combine(directory, "Samples", "original.txt"));


            Bundle doc = new Bundle();



            //DisplayValidationErrors(reader3);

            Console.ReadLine();
        }


    }
}

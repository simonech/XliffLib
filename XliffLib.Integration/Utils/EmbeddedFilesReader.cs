using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace XliffLib.Integration.Utils
{
    public static class EmbeddedFilesReader
    {
        public static string ReadString(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var result = string.Empty;

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }
    }
}

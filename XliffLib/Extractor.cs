using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Schema;
using XliffLib.Model;
using XliffLib.Utils;
using Localization.Xliff.OM.Core;
using IO=System.IO;
using Localization.Xliff.OM.Serialization;

namespace XliffLib
{
    public class Extractor
    {

        public string Write(XliffDocument document)
        {
            string result = String.Empty;
            using (IO.Stream stream = new IO.MemoryStream())
			{
				XliffWriter writer;

				writer = new XliffWriter();
				writer.Serialize(stream, document);
                stream.Position = 0;
				var sr = new IO.StreamReader(stream);
				result = sr.ReadToEnd();
            }

            return result;
        }

        public XliffDocument Extract(Bundle xliff, string sourceLanguage)
        {
            XliffDocument document = new XliffDocument(sourceLanguage);

            int fileNum = 0;
            int groupNum = 0;
            int unitNum = 0;
            foreach (var doc in xliff.Documents)
            {
                string fileId = "f" + (++fileNum);
                File xliffFile = new File(fileId);

                var containers = ProcessPropertyContainers(doc, ref groupNum, ref unitNum);
                xliffFile.Containers.AddAll(containers);
                document.Files.Add(xliffFile);
            }
            return document;
        }

        static Unit ProcessProperty(ref int unitNum)
        {
            string unitId = "u" + (++unitNum);
            Unit xliffUnit = new Unit(unitId);
            return xliffUnit;
        }

        IList<TranslationContainer> ProcessPropertyContainers(IPropertyContainer propertyContainer, ref int groupNum, ref int unitNum)
        {
            var containers = new List<TranslationContainer>();
            foreach (var group in propertyContainer.PropertyGroups)
            {
                string id = "g" + (++groupNum);
                Group xliffGroup = new Group(id);
                containers.Add(xliffGroup);
                xliffGroup.Containers.AddAll(ProcessPropertyContainers(group, ref groupNum, ref unitNum));
            }
            foreach (var property in propertyContainer.Properties)
            {
                Unit xliffUnit = ProcessProperty(ref unitNum);

                containers.Add(xliffUnit);
            }
            return containers;
        }
    }
}

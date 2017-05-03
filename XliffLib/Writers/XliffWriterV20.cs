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

namespace XliffLib.Writers
{
    public class XliffWriterV20: BaseXliffWriter
    {

        public void Create(Bundle xliff, string sourceLanguage)
        {
            XliffDocument document = new XliffDocument(sourceLanguage);


            int fileNum = 0;
            int groupNum = 0;
            int unitNum = 0;
            foreach (var file in xliff.Documents)
            {
                string fileId = "f" + (++fileNum);
                File xliffFile = new File(fileId);

                foreach (var group in file.PropertyGroups)
                {
                    xliffFile.Containers.Add(ProcessPropertyGroup(group, ref groupNum, ref unitNum));
                }
                foreach (var property in file.Properties)
                {
                    Unit xliffUnit = ProcessProperty(ref unitNum);
                    xliffFile.Containers.Add(xliffUnit);
                }
                document.Files.Add(xliffFile);
            }
        }

        private static Unit ProcessProperty(ref int unitNum)
        {
            string unitId = "u" + (++unitNum);
            Unit xliffUnit = new Unit(unitId);
            return xliffUnit;
        }

        private Group ProcessPropertyGroup(PropertyGroup propertyGroup, ref int groupNum, ref int unitNum)
        {
            string id = "g" + (++groupNum);
            Group xliffGroup = new Group(id);
            foreach (var group in propertyGroup.PropertyGroups)
            {
                xliffGroup.Containers.Add(ProcessPropertyGroup(group, ref groupNum, ref unitNum));
            }
            foreach (var property in propertyGroup.Properties)
            {
                Unit xliffUnit = ProcessProperty(ref unitNum);

                xliffGroup.Containers.Add(xliffUnit);
            }
            return xliffGroup;
        }
    }
}

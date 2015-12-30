using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XliffLib.Model;

namespace XliffLib.Extractors
{
    public class TextExtractor : BaseExtractor
    {
        private const string unitIdentifierFormat = "%%{0}%%";

        public ExtractorResult Extract(string fileContent)
        {
            ExtractorResult result = new ExtractorResult();

            result.Skeleton = string.Format(unitIdentifierFormat, 1);
            XliffUnit unit = new XliffUnit("1");
            unit.Segments.Add(new XliffSegments("1") { Source = fileContent });
            result.File.Units.Add(unit);
            return result;
        }
    }
}

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
        private readonly char[] segmentSeparators = { '.','!','?'};

        public ExtractorResult Extract(string fileContent)
        {
            ExtractorResult result = new ExtractorResult();

            result.Skeleton = string.Format(unitIdentifierFormat, 1);
            XliffUnit unit = new XliffUnit("1");
            string unitContent = fileContent;
            List<string> segments = SplitInSegments(fileContent);
            int segmentCount = 0;
            foreach (var segment in segments)
            {
                segmentCount++;
                unit.Segments.Add(new XliffSegments(segmentCount.ToString()) { Source = segment });
            }
            
            result.File.Units.Add(unit);
            return result;
        }

        private List<string> SplitInSegments(string unitText)
        {
            List<string> result = new List<string>();
            int i = 0;
            bool keepOn = true;
            do
            {
                var current = unitText.IndexOfAny(segmentSeparators, i) + 1;
                if (current == 0) {
                    string text = unitText.Substring(i);
                    if (!String.IsNullOrWhiteSpace(text))
                    {
                        result.Add(text.Trim());
                    }
                    keepOn = false;
                }
                else
                {
                    result.Add(unitText.Substring(i, current - i).Trim());
                    i = current;
                }
            } while (keepOn);
            return result;
        }
    }
}

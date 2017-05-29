using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XliffLib.Model;

namespace XliffLib
{
    public class BasicSegmenter
    {
        private readonly char[] segmentSeparators = { '.','!','?'};

        public List<string> SplitInSegments(string unitText)
        {
            var result = new List<string>();
            var i = 0;
            var keepOn = true;
            do
            {
                var current = unitText.IndexOfAny(segmentSeparators, i) + 1;
                if (current == 0) {
                    var text = unitText.Substring(i);
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

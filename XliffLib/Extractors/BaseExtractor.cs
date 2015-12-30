using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XliffLib.Extractors
{
    abstract public class BaseExtractor
    {
        abstract public ExtractorResult Extract(string fileContent);
    }
}

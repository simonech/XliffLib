using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XliffLib.HtmlProcessing;

namespace XliffLib
{
    public class DefaultMerger : Merger
    {
        public DefaultMerger() : base(new MergerToBundle())
        {
            ProcessingSteps.AddLast(new InlineCodeProcessing());
            ProcessingSteps.AddLast(new ParagraphSplitter(new SimpleHtmlParser()));
        }
    }
}

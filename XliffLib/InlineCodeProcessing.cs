using System;
using Localization.Xliff.OM.Core;
using XliffLib.Utils;

namespace XliffLib
{
    public class InlineCodeProcessing: IProcessingStep
    {
        public XliffDocument ExecuteExtraction(XliffDocument document)
        {
            var units = document.CollapseChildren<Unit>();

            foreach (var unit in units)
            {
                var originalData = new OriginalData();
                foreach (var resource in unit.Resources)
                {
                    var segment = resource as Segment;
                    if(segment!=null)
                    {
                        var cdata = segment.Source.Text[0] as CDataTag;
                        if (cdata == null) continue;
                        var html = cdata.Text.RemoveContainingTag();
                        var inlineCodeExtraction = html.ConvertHtmlTagsInInLineCodes();
                        foreach (var data in inlineCodeExtraction.OriginalData)
                        {
                            originalData.AddData(data.Value,data.Key);
                        }
                        segment.Source.Text.Clear();
                        segment.Source.Text.AddAll(inlineCodeExtraction.Text);
                    }
                }
                if(originalData.HasData)
                {
                    unit.OriginalData = originalData;
                }
            }
            return document;
        }

        public XliffDocument ExecuteMerge(XliffDocument document)
        {
            throw new NotImplementedException();
        }
    }
}

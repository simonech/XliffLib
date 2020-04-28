using System;
using System.Collections.Generic;
using Localization.Xliff.OM.Core;
using XliffLib.Utils;

namespace XliffLib
{
    public class InlineCodeProcessing : IProcessingStep
    {
        public XliffDocument ExecuteExtraction(XliffDocument document)
        {
            var units = document.CollapseChildren<Unit>();

            foreach (var unit in units)
            {
                var originalData = new OriginalData();
                var subflows = new Dictionary<string, string>();
                foreach (var resource in unit.Resources)
                {
                    var segment = resource as Segment;
                    if (segment != null)
                    {
                        var cdata = segment.Source.Text[0] as CDataTag;
                        if (cdata == null) continue;
                        var html = cdata.Text;
                        var inlineCodeExtraction = html.ConvertHtmlTagsInInLineCodes(unit.Id + "-");
                        foreach (var data in inlineCodeExtraction.OriginalData)
                        {
                            originalData.AddData(data.Value, data.Key);
                        }
                        segment.Source.Text.Clear();
                        segment.Source.Text.AddAll(inlineCodeExtraction.Text);
                        if (inlineCodeExtraction.SubFlow.Count > 0)
                        {
                            subflows = inlineCodeExtraction.SubFlow;
                        }
                    }
                }
                if (originalData.HasData)
                {
                    unit.OriginalData = originalData;
                }
                if (subflows.Count > 0)
                {
                    var newGroup = new Group(unit.Id + "-g");
                    newGroup.Type = unit.Type;
                    newGroup.Name = unit.Name;

                    foreach (var subflowItem in subflows)
                    {
                        var subFlowUnit = new Unit(subflowItem.Key);
                        var newSegment = new Segment();
                        var source = new Source();
                        var content = new PlainText(subflowItem.Value);
                        source.Text.Add(content);
                        newSegment.Source = source;
                        subFlowUnit.Resources.Add(newSegment);
                        newGroup.Containers.Add(subFlowUnit);
                    }

                    var parentFile = unit.Parent as File;
                    if (parentFile != null)
                    {
                        var index = parentFile.Containers.IndexOf(unit);
                        parentFile.Containers[index] = newGroup;
                    }
                    else
                    {
                        var parentGroup = unit.Parent as Group;
                        if (parentGroup != null)
                        {
                            var index = parentGroup.Containers.IndexOf(unit);
                            parentGroup.Containers[index] = newGroup;
                        }
                    }
                    newGroup.Containers.Add(unit);
                }
            }
            return document;
        }

        public XliffDocument ExecuteMerge(XliffDocument document)
        {
            var units = document.CollapseChildren<Unit>();
            foreach (var unit in units)
            {
                foreach (var resource in unit.Resources)
                {
                    var segment = resource as Segment;
                    if (segment != null)
                    {
                        ConvertToHtmlAndStore(segment.Source);
                        ConvertToHtmlAndStore(segment.Target);
                    }
                }
            }
            return document;
        }

        private void ConvertToHtmlAndStore(ResourceString resource)
        {
            if (resource.Text.Count == 1 && resource.Text[0] is PlainText) return;
            var html = resource.Text.ConvertToHtml();
            resource.Text.Clear();
            resource.Text.Add(new CDataTag(html));
        }
    }
}

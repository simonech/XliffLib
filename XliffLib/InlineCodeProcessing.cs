using System;
using System.Collections.Generic;
using System.Linq;
using Localization.Xliff.OM.Core;
using Localization.Xliff.OM.Modules.Metadata;
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
                    unit.Type = null;

                    if (unit.Metadata != null)
                    {
                        var newMetadataContainer = new MetadataContainer();
                        foreach (var metaGroup in unit.Metadata.Groups)
                        {
                            var newMetaGroup = new MetaGroup();
                            newMetaGroup.Id = metaGroup.Id;

                            foreach (Meta item in metaGroup.Containers)
                            {
                                var newElement = new Meta(item.Type, item.NonTranslatableText);
                                newMetaGroup.Containers.Add(newElement);
                            }
                            newMetadataContainer.Groups.Add(newMetaGroup);
                        }
                        newGroup.Metadata = newMetadataContainer;
                        unit.Metadata = null;
                    }


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
                if (unit.Resources.Count > 1)
                    throw new InvalidOperationException("At this stage I expect only one segment. This unit is invalid: " + unit.SelectorPath);

                Segment segment = unit.Resources[0] as Segment;

                if (segment != null)
                {
                    var hasSubFlow = segment.Source.Text.HasSubFlow() || segment.Target.Text.HasSubFlow();

                    ConvertToHtmlAndStore(segment.Source, subflows => SearchSubflowsInDocument(subflows, units, s=>s.Source));
                    ConvertToHtmlAndStore(segment.Target, subflows => SearchSubflowsInDocument(subflows, units, s=>s.Target));
                

                    if(hasSubFlow)
                    {
                        var group = unit.Parent as Group;
                        var newUnit = new Unit(unit.Id);
                        newUnit.Name = group.Name;
                        newUnit.Type = group.Type;

                        if (group.Metadata != null)
                        {
                            var newMetadataContainer = new MetadataContainer();
                            foreach (var metaGroup in group.Metadata.Groups)
                            {
                                var newMetaGroup = new MetaGroup();
                                newMetaGroup.Id = metaGroup.Id;

                                foreach (Meta item in metaGroup.Containers)
                                {
                                    var newElement = new Meta(item.Type, item.NonTranslatableText);
                                    newMetaGroup.Containers.Add(newElement);
                                }

                                newMetadataContainer.Groups.Add(newMetaGroup);
                            }
                            newUnit.Metadata = newMetadataContainer;
                        }

                        var newSegment = unit.Resources[0];
                        unit.Resources.Clear();
                        newUnit.Resources.Add(newSegment);

                        var parentFile = group.Parent as File;
                        if (parentFile != null)
                        {
                            var pos = parentFile.Containers.IndexOf(group);
                            parentFile.Containers.Insert(pos, newUnit);
                            parentFile.Containers.Remove(group);
                        }
                        else
                        {
                            var parentGroup = group.Parent as Group;
                            if (parentGroup != null)
                            {
                                var pos = parentGroup.Containers.IndexOf(group);
                                parentGroup.Containers.Insert(pos, newUnit);
                                parentGroup.Containers.Remove(group);
                            }
                        }
                    }
                }
            }
            return document;
        }

        private IEnumerable<KeyValuePair<string, string>> SearchSubflowsInDocument(string subflowsStringList, IList<Unit> units, Func<Segment,ResourceString> resourceSelector)
        {
            var subFlowsPairs = new Dictionary<string, string>();
            var subFlowList = subflowsStringList.Split(' ');

            foreach (var item in subFlowList)
            {
                var res = resourceSelector(units.SingleOrDefault(u => u.Id.Equals(item)).Resources[0] as Segment);
                subFlowsPairs.Add(item, (res.Text[0] as PlainText).Text);
            }
            return subFlowsPairs;
        }

        private void ConvertToHtmlAndStore(ResourceString resource, Func<string, IEnumerable<KeyValuePair<string, string>>> subflowsSelector)
        {
            if (resource.Text.Count == 1 && resource.Text[0] is PlainText) return;
            var html = resource.Text.ConvertToHtml(subflowsSelector);
            resource.Text.Clear();
            resource.Text.Add(new CDataTag(html));
        }
    }
}

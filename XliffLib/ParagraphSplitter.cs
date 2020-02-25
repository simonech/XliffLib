﻿using System;
using System.Collections.Generic;
using Localization.Xliff.OM.Core;
using XliffLib.Utils;
using System.Linq;
using Localization.Xliff.OM.Modules.Metadata;
using System.Text;
using XliffLib.HtmlProcessing;
using Localization.Xliff.OM.Extensibility;

namespace XliffLib
{
    public class ParagraphSplitter : IProcessingStep
    {
        private const string ORIGINALATTRIBUTES = "originalAttributes";
        private IHtmlParser _htmlParser;

        public ParagraphSplitter(IHtmlParser htmlParser)
        {
            _htmlParser = htmlParser;
        }

        public XliffDocument ExecuteExtraction(XliffDocument document)
        {
            var units = document.CollapseChildren<Unit>();
            foreach (var unit in units)
            {
                SplitUnitAndIterate(unit);
            }

            return document;
        }

        private void SplitUnitAndIterate(Unit unit)
        {
            if (unit.Resources.Count == 0)
            {
                return;
            }
            if (unit.Resources.Count > 1)
            {
                throw new InvalidOperationException("Cannot work with Units with multiple Segments: "+ unit.SelectorPath);
            }

            var segment = unit.Resources[0] as Segment;

            if (segment == null) return;

            if (segment.Source.Text.Count > 1)
            {
                throw new InvalidOperationException("At this stage I expect only a plain text or a CData, not multiple elements. This unit is invalid: " + unit.SelectorPath);
            }

            if (segment.Source.Text[0] is CDataTag)
            {
                var cdata = segment.Source.Text[0] as CDataTag;
                var typeBefore = unit.Type;
                var container = SplitUnit(unit, cdata.Text, true);

                if (container is Group)
                {
                    var group = container as Group;
                    foreach (Unit innerUnit in group.Containers.ToList())
                    {
                        SplitUnitAndIterate(innerUnit);
                    }
                }
                else if(container is Unit)
                {
                    if(typeBefore!=null && container.Type != null && !container.Type.Equals(typeBefore))
                    {
                        SplitUnitAndIterate(container as Unit);
                    }
                }
            }
            else if (segment.Source.Text[0] is PlainText)
            {
                var text = segment.Source.Text[0] as PlainText;
                SplitUnit(unit, text.Text, false);
            }
            else
            {
                throw new InvalidOperationException("At this stage I expect only a plain text or a CData. This unit is invalid: " + unit.SelectorPath);
            }
        }

        private TranslationContainer SplitUnit(Unit unit, string text, bool isCData=true)
        {
            SimplifiedHtmlContentItem[] paragraphs;
            if (isCData)
                paragraphs = _htmlParser.SplitHtml(text);
            else
            {
                paragraphs = _htmlParser.SplitPlainText(text);
            }

            if(paragraphs.Count()==0)
            {
                return unit;
            }
            else if(!isCData && paragraphs.Count() == 1)
            {
                return unit;
            }
            else
            {
                var newGroup = new Group(unit.Id + "-g");
                newGroup.Type = unit.Type;
                newGroup.Name = unit.Name;
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
                }

                var i = 0;
                foreach (var para in paragraphs)
                {
                    i++;
                    var paraUnit = new Unit(unit.Id + "-" + i);

                    var newSegment = new Segment();

                    if (para.Attributes.Count() > 0)
                    {
                        var newMetadataContainer = new MetadataContainer();
                        var attributeMetaGroup = new MetaGroup();
                        attributeMetaGroup.Id = ORIGINALATTRIBUTES;

                        foreach (var attribute in para.Attributes)
                        {
                            var newElement = new Meta(attribute.Key, attribute.Value);
                            attributeMetaGroup.Containers.Add(newElement);
                        }
                        newMetadataContainer.Groups.Add(attributeMetaGroup);
                        paraUnit.Metadata = newMetadataContainer;
                    }

                    var source = new Source();
                    ResourceStringContent content;
                    if (isCData)
                    {
                        string containingTag = para.Name;
                        string newContent = para.InnerContent();
                        if (!string.IsNullOrWhiteSpace(containingTag))
                        {
                            paraUnit.Type = _htmlParser.ToXliffHtmlType(containingTag);
                        }
                        if(newContent.IsHtml())
                            content = new CDataTag(newContent);
                        else
                            content = new PlainText(newContent);
                    }
                    else
                    {
                        content = new PlainText(para.Content);
                    }
                    source.Text.Add(content);
                    newSegment.Source = source;

                    paraUnit.Resources.Add(newSegment);

                    newGroup.Containers.Add(paraUnit);
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
                return newGroup;
            }
        }

        public XliffDocument ExecuteMerge(XliffDocument document)
        {
            var groups = document.CollapseChildren<Group>().Where(g => g.Id.StartsWith("u") && g.Id.Split('-').Length==2);

            foreach (var group in groups)
            {
                var unitId = group.Id.Replace("-g", "");
                var newUnit = new Unit(unitId);
                newUnit.Name = group.Name;

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

                var newSegment = new Segment();

                newSegment.Source = SetSourceValue(group);
                newSegment.Target = SetTargetValue(group);
                newUnit.Resources.Add(newSegment);

                var parentFile = group.Parent as File;
                if (parentFile != null)
                {
                    parentFile.Containers.Add(newUnit);
                    parentFile.Containers.Remove(group);
                }
                else
                {
                    var parentGroup = group.Parent as Group;
                    if (parentGroup != null)
                    {
                        parentGroup.Containers.Add(newUnit);
                        parentGroup.Containers.Remove(group);
                    }
                }
            }
            return document;
        }

        private Source SetSourceValue(Group group)
        {
            var source = new Source();
            SetResourceString(source, group, (s => s.Source));
            return source;
        }

        private Target SetTargetValue(Group group)
        {
            var target = new Target();
            SetResourceString(target, group, (s => s.Target));
            return target;
        }

        private void SetResourceString(ResourceString text, Group group, Func<Segment, ResourceString> selector)
        {
            var content = RetrieveInnerContent(group, selector);

            if (content.IsHtml())
            {
                text.Text.Add(new CDataTag(content));
            }
            else
            {
                text.Text.Add(new PlainText(content.TrimEnd('\r', '\n')));
            }
        }

        private string RetrieveInnerContent(Group group, Func<Segment, ResourceString> selector)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var container in group.Containers)
            {
                var attributeList = string.Empty;
                if (container.Metadata != null && container.Metadata.HasGroups)
                {
                    var attributeMetaGroup = container.Metadata.Groups.Single(g => g.Id.Equals(ORIGINALATTRIBUTES));
                    if (attributeMetaGroup != null)
                    {
                        foreach (Meta meta in attributeMetaGroup.Containers)
                        {
                            attributeList += string.Format(" {0}=\"{1}\"", meta.Type, meta.NonTranslatableText);
                        }
                    }
                }

                if (container is Group)
                {
                    sb.AppendFormat("<{0}{2}>{1}</{0}>", _htmlParser.FromXliffHtmlType(container.Type), RetrieveInnerContent(container as Group, selector), attributeList);
                }
                else
                {
                    var content = GetTextContent(container as Unit, selector, attributeList);
                    if (content.IsHtml())
                        sb.Append(content);
                    else
                        sb.AppendLine(content);
                }   
            }
            return sb.ToString();
        }

        private string GetTextContent(Unit nestedUnit, Func<Segment, ResourceString> selector, string attributeList)
        {
            if(nestedUnit.Resources.Count>1)
                throw new InvalidOperationException("At this stage I expect only one segment. This unit is invalid: " + nestedUnit.SelectorPath);
            Segment segment = nestedUnit.Resources[0] as Segment;
            if (segment == null) return "";

            ResourceString item = selector(segment);
            if (item.Text.Count > 1)
                throw new InvalidOperationException("At this stage I expect only a plain text or a CData, not multiple elements. This unit is invalid: " + nestedUnit.SelectorPath);
            var cdata = item.Text[0] as CDataTag;
            var text = item.Text[0] as PlainText;

            var unitType = nestedUnit.Type;

            if (string.IsNullOrEmpty(unitType))
            {
                if (cdata != null)
                {
                    return cdata.Text;
                }
                else
                {
                    return text.Text;
                }
            }
            else
            {
                if (cdata != null)
                {
                    return string.Format("<{0}{2}>{1}</{0}>", _htmlParser.FromXliffHtmlType(unitType), cdata.Text, attributeList);
                }
                else if (text != null)
                {
                    return string.Format("<{0}{2}>{1}</{0}>", _htmlParser.FromXliffHtmlType(unitType), text.Text, attributeList);
                }
                else
                {
                    throw new InvalidOperationException("At this stage I expect only a plain text or a CData. This unit is invalid: " + item.SelectableAncestor.SelectorPath);
                }
            }
        }

    }
}

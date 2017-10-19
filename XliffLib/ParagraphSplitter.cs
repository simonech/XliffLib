using System;
using System.Collections.Generic;
using Localization.Xliff.OM.Core;
using XliffLib.Utils;
using System.Linq;
using Localization.Xliff.OM.Modules.Metadata;
using System.Text;

namespace XliffLib
{
    public class ParagraphSplitter : IProcessingStep
    {
        public ParagraphSplitter()
        {
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
                var nameBefore = unit.Name;
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
                    if(!container.Name.Equals(nameBefore))
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
            string[] paragraphs;
            if (isCData)
                paragraphs = text.SplitByDefaultTags();
            else
            {
                paragraphs = text.SplitPlainText();
            }
            if(paragraphs.Count()==0)
            {
                return unit;
            }
            else if (paragraphs.Count() == 1)
            {
                string newContent;
                if (isCData)
                {
                    string containingTag = paragraphs[0].GetContainingTag();
                    newContent = paragraphs[0].RemoveContainingTag();
                    if(String.IsNullOrEmpty(unit.Name))
                    {
                        unit.Name = containingTag;
                    }
                    else
                    {
                        unit.Name = unit.Name + "|" + containingTag;
                    }
                }
                else
                {
                    newContent = paragraphs[0];
                }

                var source = new Source();
                ResourceStringContent content;
                if (newContent.IsHtml())
                {
                    content = new CDataTag(newContent);
                }
                else
                {
                    content = new PlainText(newContent);
                }
                source.Text.Add(content);
                unit.Resources[0].Source = source;
                return unit;
            }
            else
            {
                //TODO: Copy name and other attributes
                var newGroup = new Group(unit.Id + "-g");
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

                    var source = new Source();
                    ResourceStringContent content;
                    if (isCData)
                    {
                        content = new CDataTag(para);
                    }
                    else
                    {
                        content = new PlainText(para);
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
            var groups = document.CollapseChildren<Group>().Where(g => g.Id.StartsWith("u"));

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
                var source = new Source();
                var target = new Target();

                //source.Text.Add(MergeBackUnits(group.CollapseChildren<Source>()));
                target.Text.Add(MergeBackUnits(group.CollapseChildren<Target>()));

                newSegment.Source = source;
                newSegment.Target = target;
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

            var units = document.CollapseChildren<Unit>().Where(u => u.Name!=null && u.Name.Contains("|"));

            foreach (var unit in units)
            {
                var nameArray = unit.Name?.Split('|');
                if (nameArray.Length == 0)
                {
                    continue;
                }
                unit.Name = nameArray[0];
                var enclosingTag = nameArray[1];

                var newSegment = new Segment();
                var source = new Source();
                var target = new Target();

                //var sourceText = unit.Resources[0].Source.Text[0] as PlainText;
                //source.Text.Clear();
                //source.Text.Add(new CDataTag(string.Format("<{0}>{1}</{0}>", enclosingTag, sourceText.Text)));

                var targetCdata = unit.Resources[0].Target.Text[0] as CDataTag;
                var targetText = unit.Resources[0].Target.Text[0] as PlainText;

                target.Text.Clear();
                if(targetCdata!=null)
                {
                    target.Text.Add(new CDataTag(string.Format("<{0}>{1}</{0}>", enclosingTag, targetCdata.Text)));
                }
                else if (targetText != null)
                {
                    target.Text.Add(new CDataTag(string.Format("<{0}>{1}</{0}>", enclosingTag, targetText.Text)));
                }

                newSegment.Source = source;
                newSegment.Target = target;

                unit.Resources.Clear();
                unit.Resources.Add(newSegment);
            }

            return document;
        }

        private ResourceStringContent MergeBackUnits<T>(IList<T> list) where T : ResourceString
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in list)
            {
                if(item.Text.Count>1)
                    throw new InvalidOperationException("At this stage I expect only a plain text or a CData, not multiple elements. This unit is invalid: " + item.SelectableAncestor.SelectorPath);

                var segment = item.SelectableAncestor as Segment;
                var unit = segment.Parent as Unit;
                var unitName = unit.Name;

                var cdata = item.Text[0] as CDataTag;
                var text = item.Text[0] as PlainText;

                if (string.IsNullOrEmpty(unitName))
                {
                    sb.AppendLine(text.Text);
                }
                else
                {
                    if (cdata != null)
                    {
                        sb.Append(string.Format("<{0}>{1}</{0}>", unitName, cdata.Text));
                    }
                    else if (text != null)
                    {
                        sb.Append(string.Format("<{0}>{1}</{0}>", unitName, text.Text));
                    }
                    else
                    {
                        throw new InvalidOperationException("At this stage I expect only a plain text or a CData. This unit is invalid: " + item.SelectableAncestor.SelectorPath);
                    }
                }


            }

            var finalText = sb.ToString();
            if (finalText.IsHtml())
                return new CDataTag(finalText);
            else
                return new PlainText(finalText.TrimEnd('\r','\n'));
        }
    }
}

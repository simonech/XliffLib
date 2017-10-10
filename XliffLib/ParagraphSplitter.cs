using System;
using System.Collections.Generic;
using Localization.Xliff.OM.Core;
using XliffLib.Utils;
using System.Linq;
using Localization.Xliff.OM.Modules.Metadata;

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
                foreach (var resource in unit.Resources)
                {
                    var segment = resource as Segment;
                    if (segment != null)
                    {
                        if (segment.Source.Text.Count > 1)
                        {
                            throw new InvalidOperationException("At this stage I expect only a plain text or a CData, not multiple elements. This unit is invalid: "+unit.SelectorPath);
                        }
                        if(segment.Source.Text[0] is CDataTag)
                        {
                            SplitCData(unit, segment.Source.Text[0] as CDataTag);
                        }
                        else if (segment.Source.Text[0] is PlainText)
                        {
                            SplitPlainText(unit, segment.Source.Text[0] as PlainText);
                        }
                        else
                        {
                            throw new InvalidOperationException("At this stage I expect only a plain text or a CData. This unit is invalid: " + unit.SelectorPath);
                        }
                    }
                }
            }

            return document;
        }

        private void SplitPlainText(Unit unit, PlainText plainText)
        {
            SplitUnit(unit, plainText.Text, false);
        }

        private void SplitCData(Unit unit, CDataTag cDataTag)
        {
            SplitUnit(unit, cDataTag.Text, true);
        }


        private void SplitUnit(Unit unit, string text, bool isCData=true)
        {
            var paragraphs = text.SplitByParagraphs();
            if (paragraphs.Count() > 1)
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
                    parentFile.Containers.Add(newGroup);
                    parentFile.Containers.Remove(unit);
                }
                else
                {
                    var parentGroup = unit.Parent as Group;
                    if (parentGroup != null)
                    {
                        parentGroup.Containers.Add(newGroup);
                        parentGroup.Containers.Remove(unit);
                    }
                }
            }
        }

        public XliffDocument ExecuteMerge(XliffDocument document)
        {
            return document;
        }
    }
}

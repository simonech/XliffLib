using System;
using System.Collections.Generic;
using Localization.Xliff.OM.Core;
using XliffLib.Utils;
using System.Linq;

namespace XliffLib
{
    public class CDataSplitter : IProcessingStep
    {
        public CDataSplitter()
        {
        }

        public int Order { get => 1; }

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
                        var cdata = segment.Source.Text[0] as CDataTag;
                        var html = cdata.Text;
                        var paragraphs = html.SplitByParagraphs();
                        if (paragraphs.Count() > 1)
                        {
                            //TODO: Copy name and other attributes
                            var newGroup = new Group(unit.Id + "-g");

                            var i = 0;
                            foreach (var para in paragraphs)
                            {
                                i++;
                                var paraUnit = new Unit(unit.Id + "-" + i);

                                var newSegment = new Segment();

                                var source = new Source();
                                source.Text.Add(new CDataTag(para));
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
                }

            }

            return document;
        }



        public XliffDocument ExecuteMerge(XliffDocument document)
        {
            return document;
        }
    }
}

using System;
using System.Collections.Generic;
using Localization.Xliff.OM.Core;
using XliffLib.Utils;
using System.Linq;

namespace XliffLib
{
    public class CDataSplitter: IProcessingStep
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
					Segment segment = resource as Segment;
					if (segment != null)
					{
						CDataTag cdata = segment.Source.Text[0] as CDataTag;
						string html = cdata.Text;
						var paragraphs = html.SplitByParagraphs();
						if (paragraphs.Count() > 1)
						{
							Group newGroup = new Group(unit.Id+"-g");

                            int i = 0;
                            foreach (var para in paragraphs)
                            {
                                i++;
                                Unit paraUnit = new Unit(unit.Id + "-" + i);

								Segment newSegment = new Segment();

								var source = new Source();
								source.Text.Add(new CDataTag(para));
								newSegment.Source = source;

                                paraUnit.Resources.Add(newSegment);

                                newGroup.Containers.Add(paraUnit);
                            }

                            File parentFile = unit.Parent as File;
                            if(parentFile!=null)
                            {
								parentFile.Containers.Add(newGroup);
								parentFile.Containers.Remove(unit);
                            }
                            else
                            {
                                Group parentGroup = unit.Parent as Group;
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

using System;
using System.Collections.Generic;
using Localization.Xliff.OM.Core;
using XliffLib.Utils;

namespace XliffLib.Model
{
    public class Property: PropertyContainer
    {
        public Property(string name, string text): base(name)
        {
            Value = text;
        }
        public string Value { get; set; }

        public override TranslationContainer ToXliff(IdCounter counter)
        {
			var unitId = "u" + (counter.GetNextUnitId());
			var xliffUnit = new Unit(unitId);
			xliffUnit.Name = this.Name;

			var segment = new Segment();
			if (this.Value.IsHtml())
			{
				var source = new Source();
				source.Text.Add(new CDataTag(this.Value));
				segment.Source = source;
			}
			else
				segment.Source = new Source(this.Value);


			xliffUnit.Resources.Add(segment);
			return xliffUnit;
        }
    }
}
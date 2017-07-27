using System;
using System.Collections.Generic;
using Localization.Xliff.OM.Core;
using XliffLib.Utils;

namespace XliffLib.Model
{
    public class Property : PropertyContainer
    {
        public Property(string name, string text) : base(name)
        {
            Value = text;
        }
        public string Value { get; set; }

        public static new PropertyContainer FromXliff(TranslationContainer xliffUnit)
        {
            var unit = xliffUnit as Unit;
            //TODO: Add test for this condition
            if (unit.Resources.Count > 1)
                throw new InvalidOperationException("Cannot operate on multiple segments. Make sure previous steps of the import have merged all segments into one.");
            var segment = unit.Resources[0] as Segment;

            //TODO: Add test for this condition
            if (segment.Target == null || segment.Target.Text == null | segment.Target.Text.Count == 0)
                throw new InvalidOperationException("Property doesn't have a target: cannot import.");

            //TODO: Add test for this condition
            if (segment.Target.Text.Count > 1)
                throw new InvalidOperationException("Cannot operate on target with multiple elements. Make sure previous steps have converted all inline markup into a CData section");

            var text = segment.Target.Text[0] as PlainText;

            if (text != null)
                return new Property(xliffUnit.Name, text.Text);

            var html = segment.Target.Text[0] as CDataTag;
            if (html != null)
                return new Property(xliffUnit.Name, html.Text);

            //TODO: Add test for this condition
            return null;

        }

        public override TranslationContainer ToXliff(IdCounter counter)
        {
            var unitId = "u" + (counter.GetNextUnitId());
            var xliffUnit = new Unit(unitId);
            xliffUnit.Name = this.Name;

            if (this.Attributes.Count > 0)
            {
                xliffUnit.Metadata = this.Attributes.ToXliffMetadata();
            }

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
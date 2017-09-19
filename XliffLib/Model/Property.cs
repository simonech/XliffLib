using System;
using System.Collections.Generic;
using Localization.Xliff.OM;
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
                throw new InvalidOperationException("Error on unit " + unit.SelectorPath + ": Cannot operate on multiple segments. Make sure previous steps of the import have merged all segments into one.");
            var segment = unit.Resources[0] as Segment;

            //TODO: Add test for this condition
            if (segment.Target == null || segment.Target.Text == null || segment.Target.Text.Count == 0)
                throw new InvalidOperationException("Unit " + unit.SelectorPath + " doesn't have a target: cannot import.");

            //TODO: Add test for this condition
            if (segment.Target.Text.Count > 1)
                throw new InvalidOperationException("Error on unit " + unit.SelectorPath + ": Cannot operate on target with multiple elements. Make sure previous steps have converted all inline markup into a CData section");

            string textValue = string.Empty;
            var text = segment.Target.Text[0] as PlainText;

            if (text != null)
                textValue = text.Text;

            var html = segment.Target.Text[0] as CDataTag;
            if (html != null)
                textValue = html.Text;

            if(!String.IsNullOrWhiteSpace(textValue))
            {
                var property = new Property(unit.Name, textValue);
                if (unit.Metadata != null)
                {
                    property.Attributes = AttributeList.FromXliffMetadata(unit.Metadata);
                }
                return property;
            }


            //TODO: Add test for this condition
            return null;

        }

        public override XliffElement ToXliff(IdCounter idCounter)
        {
            var unitId = "u" + (idCounter.GetNextUnitId());
            var xliffUnit = new Unit(unitId)
            {
                Name = this.Name,
                Metadata = Attributes.ToXliffMetadata()
            };
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
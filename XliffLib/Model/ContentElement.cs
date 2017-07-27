using System;
using System.Collections.Generic;
using Localization.Xliff.OM;
using Localization.Xliff.OM.Core;

namespace XliffLib.Model
{
    public abstract class ContentElement
    {
        public ContentElement()
        {
            Attributes = new AttributeList();
        }

        public AttributeList Attributes { get; set; }

        public bool ShouldSerializeAttributes()
        {
            return Attributes.Count > 0;
        }

        public abstract XliffElement ToXliff(IdCounter idCounter);
    }
}

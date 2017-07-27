using System;
using Localization.Xliff.OM.Core;

namespace XliffLib.Model
{
    public abstract class PropertyContainer : ContentElement
    {
        public PropertyContainer(string name) : base()
        {
            Name = name;
        }
        public string Name { get; set; }


        public static PropertyContainer FromXliff(TranslationContainer container)
        {
            if (container is Group)
                return PropertyGroup.FromXliff(container);
            if (container is Unit)
                return Property.FromXliff(container);
            return null;
        }
    }
}
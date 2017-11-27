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
            PropertyContainer propertyContainer = null;
            if (container is Group)
            {
                propertyContainer = PropertyGroup.FromXliff(container);
            }
            if (container is Unit)
            {
                propertyContainer = Property.FromXliff(container);
            }
            return propertyContainer;
        }
    }
}
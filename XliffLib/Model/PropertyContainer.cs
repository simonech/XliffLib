using System.Collections.Generic;
using Localization.Xliff.OM.Core;

namespace XliffLib.Model
{
    public abstract class PropertyContainer
    {
        public PropertyContainer(string name)
        {
            Name = name;
        }
        public string Name { get; set; }

        public abstract TranslationContainer ToXliff(IdCounter counter);

    }
}
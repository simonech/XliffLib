using Localization.Xliff.OM.Core;

namespace XliffLib.Model
{
    public abstract class PropertyContainer: ContentElement
    {
        public PropertyContainer(string name): base()
        {
            Name = name;
        }
        public string Name { get; set; }

		public abstract TranslationContainer ToXliff(IdCounter counter);
    }
}
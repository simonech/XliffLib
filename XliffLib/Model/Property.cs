using System.Collections.Generic;

namespace XliffLib.Model
{
    public class Property
    {
        public Property(string name, string text)
        {
            Name = name;
            Value = text;
        }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
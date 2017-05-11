using System.Collections.Generic;

namespace XliffLib.Model
{
    public class Property
    {
        public Property(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
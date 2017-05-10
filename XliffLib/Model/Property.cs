using System.Collections.Generic;

namespace XliffLib.Model
{
    public class Property
    {
        public Property(string id)
        {
            Id = id;
        }
        public string Id { get; private set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
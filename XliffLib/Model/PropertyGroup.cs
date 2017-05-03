using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XliffLib.Model
{
    public class PropertyGroup: IContentItem
    {
        public PropertyGroup(string id)
        {
            ContentItems = new List<IContentItem>();
            Id = id;
        }
        public IList<IContentItem> ContentItems { get; set; }
        public string Id { get; private set; }
        public string Name { get; set; }
    }
}

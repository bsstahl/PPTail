using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Data.Ef
{
    public class ContentItemCollection: List<PPTail.Entities.ContentItem>
    {
        public ContentItemCollection(IEnumerable<ContentItem> items)
        {
            Load(items);
        }

        private void Load(IEnumerable<ContentItem> items)
        {
            foreach (var item in items)
                this.Add(item.AsEntity());
        }
    }
}

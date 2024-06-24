using PPTail.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PPTail.Data.Forestry
{
    internal class CategoryCollection
    {
        public IEnumerable<Category>? Categories { get; set; }

        public IEnumerable<Entities.Category> AsEntity()
        {
            return this.Categories.Select(c => new Entities.Category()
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            });
        }
    }
}

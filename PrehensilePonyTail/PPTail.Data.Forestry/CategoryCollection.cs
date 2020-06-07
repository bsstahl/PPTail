using PPTail.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace PPTail.Data.Forestry
{
    internal class CategoryCollection
    {
        public IEnumerable<Category> Categories { get; set; }

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

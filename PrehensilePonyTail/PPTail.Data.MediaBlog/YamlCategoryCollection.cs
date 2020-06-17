using PPTail.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace PPTail.Data.MediaBlog
{
    internal class YamlCategoryCollection
    {
        public IEnumerable<Category> Category { get; set; }

        public IEnumerable<Entities.Category> AsEntity()
        {
            return this.Category.Select(c => new Entities.Category()
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            });
        }
    }
}

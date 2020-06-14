using PPTail.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPTail.Builders
{
    public class CategoryCollectionBuilder
    {
        readonly List<Category> _categories = new List<Category>();

        public IEnumerable<Category> Build()
        {
            return _categories;
        }

        public CategoryCollectionBuilder AddCategory(Category category)
        {
            _categories.Add(category);
            return this;
        }

        public CategoryCollectionBuilder AddCategory(Guid id, String name, String description)
        {
            _categories.Add(new CategoryBuilder()
                .Id(id)
                .Name(name)
                .Description(description));
            return this;
        }

    }
}

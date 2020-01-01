﻿using PPTail.Entities;
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

        public CategoryCollectionBuilder AddCategory(Guid id, string name, string description)
        {
            _categories.Add(new CategoryBuilder()
                .Id(id)
                .Name(name)
                .Description(description));
            return this;
        }

        public CategoryCollectionBuilder AddCategories(IEnumerable<Category> categories)
        {
            _categories.AddRange(categories);
            return this;
        }

    }
}
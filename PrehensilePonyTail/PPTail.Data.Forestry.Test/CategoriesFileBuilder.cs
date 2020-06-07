using PPTail.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestHelperExtensions;

namespace PPTail.Data.Forestry.Test
{
    public class CategoriesFileBuilder
    {
        private readonly List<Category> _categories;

        public CategoriesFileBuilder()
        {
            _categories = new List<Category>();
        }

        public CategoriesFileBuilder(IEnumerable<Category> categories)
        {
            _categories = new List<Category>(categories);
        }

        public CategoriesFileBuilder AddCategory(Category category)
        {
            _categories.Add(category);
            return this;
        }

        public CategoriesFileBuilder AddCategory(Guid id, string name, string description)
        {
            return this.AddCategory(new Entities.Category()
            {
                Id = id,
                Name = name,
                Description = description
            });
        }

        public CategoriesFileBuilder AddCategories(IEnumerable<Entities.Category> categories)
        {
            foreach (var category in categories)
                this.AddCategory(category);
            return this;
        }

        public CategoriesFileBuilder AddRandomCategory()
        {
            this.AddCategory(Guid.NewGuid(), string.Empty.GetRandom(), string.Empty.GetRandom());
            return this;
        }

        public CategoriesFileBuilder AddRandomCategories()
        {
            return this.AddRandomCategories(10.GetRandom(3));
        }

        public CategoriesFileBuilder AddRandomCategories(int count)
        {
            for (int i = 0; i < count; i++)
                this.AddRandomCategory();
            return this;
        }

        public String Build()
        {
            var sb = new StringBuilder();
            sb.AppendLine("---");
            sb.AppendLine("categories:");

            foreach (var category in _categories)
            {
                sb.AppendLine($"- id: {category.Id.ToString()}");
                sb.AppendLine($"  name: {category.Name}");
                sb.AppendLine($"  description: {category.Description}");
            }

            sb.AppendLine();
            sb.AppendLine("---");
            sb.AppendLine();
            return sb.ToString();
        }

    }
}

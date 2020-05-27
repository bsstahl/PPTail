using PPTail.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PPTail.Data.Forestry.Test
{
    public class CategoriesFileBuilder
    {
        public readonly List<Category> _categories;

        public CategoriesFileBuilder()
        {
            _categories = new List<Category>();
        }

        public CategoriesFileBuilder(IEnumerable<Category> categories)
        {
            _categories = new List<Category>(categories);
        }

        public CategoriesFileBuilder Category(Category category)
        {
            _categories.Add(category);
            return this;
        }

        public CategoriesFileBuilder Category(Guid id, string name, string description)
        {
            return this.Category(new Entities.Category()
            {
                Id = id,
                Name = name,
                Description = description
            });
        }

        //public CategoriesFileBuilder CategoryNames(IEnumerable<String> names)
        //{
        //    foreach (var name in names)
        //    {
        //        Guid id = Guid.NewGuid();
        //        string description = $"{id.ToString()}_{name}";
        //        this.Category(id, name, description);
        //    }
        //    return this;
        //}

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

        //public IEnumerable<Guid> BuildIds()
        //{
        //    return _categories.Select(c => c.Id);
        //}
    }
}

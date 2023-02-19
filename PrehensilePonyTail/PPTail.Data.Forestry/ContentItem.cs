using PPTail.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Data.Forestry
{
    internal class ContentItem
    {
        public Guid Id { get; set; }
        public String Author { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public String Content { get; set; }
        public bool IsPublished { get; set; }
        public bool BuildIfNotPublished { get; set; }
        public bool ShowInList { get; set; }
        public DateTime PublicationDate { get; set; }
        public DateTime LastModificationDate { get; set; }
        public String Slug { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public Int32 MenuOrder { get; set; }

        public String ByLine { get; set; }

        public IEnumerable<String> Categories { get; set; }

        internal Entities.ContentItem AsEntity(IEnumerable<Entities.Category> categories)
        {
            return new Entities.ContentItem()
            {
                Id = this.Id,
                Author = this.Author,
                ByLine = this.ByLine,
                Content = this.Content,
                Description = this.Description ?? string.Empty,
                IsPublished = this.IsPublished,
                BuildIfNotPublished = this.BuildIfNotPublished,
                LastModificationDate = this.LastModificationDate,
                MenuOrder = this.MenuOrder,
                PublicationDate = this.PublicationDate,
                ShowInList = this.ShowInList,
                Slug = this.Slug,
                Tags = this.Tags,
                Title = this.Title,
                CategoryIds = categories
                    .Where(c => this.Categories.Contains(c.Name))
                    .Select(c => c.Id)
            };
        }

    }
}

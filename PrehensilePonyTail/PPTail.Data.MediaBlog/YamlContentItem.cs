using PPTail.Entities;
using PPTail.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PPTail.Data.MediaBlog
{
    internal class YamlContentItem
    {
        public Guid Id { get; set; }
        public String Author { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public String Content { get; set; }
        public bool IsPublished { get; set; }
        public bool ShowInList { get; set; }
        public DateTime PublicationDate { get; set; }
        public DateTime LastModificationDate { get; set; }
        public String Slug { get; set; }
        public String ByLine { get; set; }
        public Int32 MenuOrder { get; set; }

        public IEnumerable<string> Tags { get; set; }
        public IEnumerable<String> Categories { get; set; }


        internal Entities.ContentItem AsEntity(IEnumerable<Category> categories)
        {
            return new Entities.ContentItem()
            {
                Id = this.Id,
                Author = this.Author,
                Title = this.Title,
                Description = this.Description,
                Content = this.Content,
                IsPublished = this.IsPublished,
                ShowInList = this.ShowInList,
                PublicationDate = this.PublicationDate,
                LastModificationDate = this.LastModificationDate,
                Slug = this.Slug ?? this.Title.CreateSlug(),
                ByLine = this.ByLine ?? $"by {this.Author}",
                MenuOrder = this.MenuOrder,
                Tags = this.Tags,
                CategoryIds = categories
                    .Join(this.Categories, c => c.Name, c => c, (c, cn) => c.Id)
            };
        }
    }
}

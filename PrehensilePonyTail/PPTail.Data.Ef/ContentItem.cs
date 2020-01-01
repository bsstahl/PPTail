using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Data.Ef
{
    public class ContentItem
    {
        [Key]
        public Guid Id { get; set; }

        public String Author { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public String Content { get; set; }
        public String Slug { get; set; }
        public String ByLine { get; set; }

        public DateTime PublicationDate { get; set; }
        public DateTime LastModificationDate { get; set; }

        public bool IsPublished { get; set; }
        public bool ShowInList { get; set; }

        public String Tags { get; set; }
        public String CategoryIds { get; set; }


        public PPTail.Entities.ContentItem AsEntity()
        {
            return new PPTail.Entities.ContentItem()
            {
                Id = this.Id,
                Title = this.Title,
                Author = this.Author,
                Description = this.Description,
                Content = this.Content,
                Slug = this.Slug,
                ByLine = this.ByLine,
                PublicationDate = this.PublicationDate,
                LastModificationDate = this.LastModificationDate,
                IsPublished = this.IsPublished,
                ShowInList = this.ShowInList,
                Tags = this.Tags.GetTags(),
                CategoryIds = this.CategoryIds.GetCategoryIds()
            };
        }
    }
}

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

        public string Author { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Slug { get; set; }
        public string ByLine { get; set; }

        public DateTime PublicationDate { get; set; }
        public DateTime LastModificationDate { get; set; }

        public bool IsPublished { get; set; }
        public bool ShowInList { get; set; }


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
                ShowInList = this.ShowInList
            };
        }
    }
}

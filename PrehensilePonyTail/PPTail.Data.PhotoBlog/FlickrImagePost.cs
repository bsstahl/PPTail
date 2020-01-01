using System;
using System.Collections.Generic;
using System.Text;
using PPTail.Extensions;

namespace PPTail.Data.PhotoBlog
{
    internal class FlickrImagePost
    {
        public String Author { get; set; }
        public String Description { get; set; }
        public String Title { get; set; }
        public DateTime Posted { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public FlickrImage Image { get; set; }


        public Entities.ContentItem AsContentItem()
        {
            return this.AsContentItem(Guid.NewGuid());
        }

        public Entities.ContentItem AsContentItem(Guid Id)
        {
            return new Entities.ContentItem()
            {
                Author = this.Author,
                ByLine = $"by {this.Author}",
                CategoryIds = new Guid[] { Guid.Parse("663D2D20-6B79-47B1-AFAD-615F15E226A7") },
                Content = $"<a data-flickr-embed=\"true\" href=\"{this.Image.FlickrListUrl}\" title=\"{this.Image.Title}\"><img class=\"img-responsive\" src=\"{this.Image.ImageUrl}\" alt=\"{this.Image.Title}\"></a>",
                Description = this.Description,
                Id = Id,
                IsPublished = true,
                LastModificationDate = this.Image.ImageTaken,
                MenuOrder = 0,
                PublicationDate = this.Posted,
                ShowInList = true,
                Slug = this.Title.CreateSlug(),
                Tags = this.Tags,
                Title = this.Title
            };
        }
    }
}

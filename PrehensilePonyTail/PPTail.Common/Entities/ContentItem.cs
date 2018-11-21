using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Entities
{
    public class ContentItem
    {
        public Guid Id { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public bool IsPublished { get; set; }
        public bool ShowInList { get; set; }
        public DateTime PublicationDate { get; set; }
        public DateTime LastModificationDate { get; set; }
        public string Slug { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public IEnumerable<Guid> CategoryIds { get; set; }
        public int MenuOrder { get; set; }

        public string ByLine { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Entities
{
    public class ContentItem
    {
        public Guid Id { get; set; }
        public String Author { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public String Teaser {  get; set; }
        public String Content { get; set; }
        public bool IsPublished { get; set; }
        public bool BuildIfNotPublished { get; set; }
        public bool ShowInList { get; set; }
        public DateTime PublicationDate { get; set; }
        public DateTime LastModificationDate { get; set; }
        public String Slug { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public IEnumerable<Guid> CategoryIds { get; set; }
        public Int32 MenuOrder { get; set; }

        public String ByLine { get; set; }
    }
}

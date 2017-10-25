using PPTail.Entities;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPTail.Entities
{
    public class ContentPageSource
    {
        public ContentItem ContentItem { get; set; }

        public ISettings Settings { get; set; }

        public string SidebarContent { get; set; }

        public string NavigationContent { get; set; }


    }
}

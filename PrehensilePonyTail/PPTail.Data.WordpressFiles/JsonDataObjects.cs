using System;
using System.Collections.Generic;
using System.Text;

namespace PPTail.Data.WordpressFiles
{
    public class Guid
    {
        public string rendered { get; set; }
    }

    public class Title
    {
        public string rendered { get; set; }
    }

    public class Content
    {
        public string rendered { get; set; }
        public bool _protected { get; set; }
    }

    public class Excerpt
    {
        public string rendered { get; set; }
        public bool _protected { get; set; }
    }

    public class _Links
    {
        public Self[] self { get; set; }
        public Collection[] collection { get; set; }
        public About[] about { get; set; }
        public Author[] author { get; set; }
        public Reply[] replies { get; set; }
        public VersionHistory[] versionhistory { get; set; }
        public WpAttachment[] wpattachment { get; set; }
        public WpTerm[] wpterm { get; set; }
        public Cury[] curies { get; set; }
        public WpPost_Type[] wppost_type { get; set; }
    }

    public class WpPost_Type
    {
        public string href { get; set; }
    }

    public class WpTerm
    {
        public string taxonomy { get; set; }
        public bool embeddable { get; set; }
        public string href { get; set; }
    }

    public class Self
    {
        public string href { get; set; }
    }

    public class Collection
    {
        public string href { get; set; }
    }

    public class About
    {
        public string href { get; set; }
    }

    public class Author
    {
        public bool embeddable { get; set; }
        public string href { get; set; }
    }

    public class Reply
    {
        public bool embeddable { get; set; }
        public string href { get; set; }
    }

    public class VersionHistory
    {
        public string href { get; set; }
    }

    public class WpAttachment
    {
        public string href { get; set; }
    }

    public class Cury
    {
        public string name { get; set; }
        public string href { get; set; }
        public bool templated { get; set; }
    }

}

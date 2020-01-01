using System;
using System.Collections.Generic;
using System.Text;

namespace PPTail.Data.WordpressFiles
{
    public class Guid
    {
        public String rendered { get; set; }
    }

    public class Title
    {
        public String rendered { get; set; }
    }

    public class Content
    {
        public String rendered { get; set; }
        public bool _protected { get; set; }
    }

    public class Excerpt
    {
        public String rendered { get; set; }
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
        public String href { get; set; }
    }

    public class WpTerm
    {
        public String taxonomy { get; set; }
        public bool embeddable { get; set; }
        public String href { get; set; }
    }

    public class Self
    {
        public String href { get; set; }
    }

    public class Collection
    {
        public String href { get; set; }
    }

    public class About
    {
        public String href { get; set; }
    }

    public class Author
    {
        public bool embeddable { get; set; }
        public String href { get; set; }
    }

    public class Reply
    {
        public bool embeddable { get; set; }
        public String href { get; set; }
    }

    public class VersionHistory
    {
        public String href { get; set; }
    }

    public class WpAttachment
    {
        public String href { get; set; }
    }

    public class Cury
    {
        public String name { get; set; }
        public String href { get; set; }
        public bool templated { get; set; }
    }

}

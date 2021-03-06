﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PPTail.Data.WordpressFiles
{

    public class Post
    {
        public Int32 id { get; set; }
        public DateTime date { get; set; }
        public DateTime date_gmt { get; set; }
        public Guid guid { get; set; }
        public DateTime modified { get; set; }
        public DateTime modified_gmt { get; set; }
        public String slug { get; set; }
        public String status { get; set; }
        public String type { get; set; }
        public String link { get; set; }
        public Title title { get; set; }
        public Content content { get; set; }
        public Excerpt excerpt { get; set; }
        public Int32 author { get; set; }
        public Int32 featured_media { get; set; }
        public String comment_status { get; set; }
        public String ping_status { get; set; }
        public bool sticky { get; set; }
        public String template { get; set; }
        public String format { get; set; }
        public object[] meta { get; set; }
        public int[] categories { get; set; }
        public int?[] tags { get; set; }
        public _Links _links { get; set; }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PPTail.Entities;

namespace PPTail.Data.Ef
{
    public class ContentContext: DbContext
    {
        public DbSet<ContentItem> Pages { get; set; }

        public DbSet<ContentItem> Posts { get; set; }


        public ContentContext(DbContextOptions<ContentContext> options): base(options) { }


        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //{
        //    base.OnConfiguring(options);
        //}
    }
}

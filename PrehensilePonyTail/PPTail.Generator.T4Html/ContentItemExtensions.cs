using PPTail.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Generator.T4Html
{
    public static class ContentItemExtensions
    {
        public static string ProcessTemplate(this ContentItem item, string template)
        {
            return template.Replace("{Title}", item.Title).Replace("{Content}", item.Content).Replace("{Author}", item.Author);
        }
    }
}

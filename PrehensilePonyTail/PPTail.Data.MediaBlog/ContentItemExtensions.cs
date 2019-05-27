using PPTail.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPTail.Data.MediaBlog
{
    public static class ContentItemExtensions
    {
        public static ContentItem FromJson(this ContentItem ignore, String contentJson, Guid Id)
        {
            var contentItem = Newtonsoft.Json.JsonConvert.DeserializeObject<Entities.ContentItem>(contentJson);
            if (contentItem != null)
            {
                contentItem.Id = Id;
                contentItem.ByLine = $"by {contentItem.Author}";
            }

            return contentItem;
        }

    }
}

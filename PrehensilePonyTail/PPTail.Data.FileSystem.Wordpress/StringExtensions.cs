using Newtonsoft.Json.Linq;
using PPTail.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace PPTail.Data.FileSystem.Wordpress
{
    public static class StringExtensions
    {
        public static JArray ToJArray(this string json)
        {
            JArray result;

            try
            {
                result = JArray.Parse(json);
            }
            catch (Newtonsoft.Json.JsonReaderException)
            {
                result = new JArray();
            }

            return result;
        }

        public static IEnumerable<ContentItem> ParseContentItems(this string jsonText, string objectType, IDictionary<int, string> users, string defaultAuthorName)
        {
            var result = new List<ContentItem>();
            var nodes = jsonText.ToJArray();

            foreach (var node in nodes)
            {
                if (node != null)
                {
                    int authorId = node.ParseInt32("author");
                    string author = users.Find(authorId, defaultAuthorName);
                    string byLine = string.IsNullOrEmpty(author) ? string.Empty : $"by {author}";

                    string content = node["content"]?["rendered"].ToString();
                    string title = node["title"]?["rendered"]?.ToString();
                    string description = node["excerpt"]?["rendered"]?.ToString();
                    string status = node["status"]?.ToString();
                    string slug = node["slug"]?.ToString();

                    DateTime publicationDate = node.ParseDate("date_gmt");
                    DateTime lastModificationDate = node.ParseDate("modified_gmt");

                    //    bool isShowInList = false;
                    //    bool.TryParse(node.GetElementValue("showinlist"), out isShowInList);

                    //    var tagElements = node.Descendants().Where(n => n.Name.LocalName == "tag");
                    //    var tags = tagElements.Select(e => e.Value);
                    var tags = new List<string>();

                    //    var categoryNodes = node.Descendants().Where(n => n.Name.LocalName == "category");
                    //    var categoryIds = categoryNodes.Select(n => Guid.Parse(n.Value));

                    result.Add(new ContentItem()
                    {
                        //Id = id,
                        IsPublished = status.Trim().ToLower().StartsWith("publish", StringComparison.CurrentCulture),
                        //ShowInList = isShowInList,
                        Title = title,
                        Description = description,
                        Content = content,
                        Slug = slug,
                        Author = author,
                        PublicationDate = publicationDate,
                        LastModificationDate = lastModificationDate,
                        Tags = tags,
                        ByLine = byLine,
                        //CategoryIds = categoryIds
                    });
                }
            }

            return result;
        }


    }
}

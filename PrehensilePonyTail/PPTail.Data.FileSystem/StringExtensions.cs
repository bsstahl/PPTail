using PPTail.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PPTail.Data.FileSystem
{
    public static class StringExtensions
    {

        public static SiteSettings ParseSettings(this String xmlText)
        {
            SiteSettings result = null;

            XElement node;
            try
            {
                node = XElement.Parse(xmlText);
            }
            catch (System.Xml.XmlException)
            {
                node = null;
            }

            if (node != null)
            {
                String ppPage = node.GetElementValue("postsperpage");
                Int32 postsPerPage = 0;
                Int32.TryParse(ppPage, out postsPerPage);

                String ppFeed = node.GetElementValue("postsperfeed");
                Int32 postsPerFeed = 0;
                Int32.TryParse(ppFeed, out postsPerFeed);

                result = new SiteSettings()
                {
                    Title = node.GetElementValue("name"),
                    Description = node.GetElementValue("description"),
                    PostsPerPage = postsPerPage,
                    PostsPerFeed = postsPerFeed,
                    Theme = node.GetElementValue("theme")
                };
            }

            return result;
        }

        public static ContentItem ParseContentItem(this String xmlText, String filename, String nodeLocalName)
        {
            ContentItem result = null;

            XElement node;
            try
            {
                node = XElement.Parse(xmlText);
            }
            catch (System.Xml.XmlException)
            {
                node = null;
            }

            if (node != null && node.Name.LocalName == nodeLocalName)
            {
                String fileId = System.IO.Path.GetFileNameWithoutExtension(filename);
                Guid id = Guid.Parse(fileId);

                String author = node.GetElementValue("author");

                bool isPublished = false;
                bool.TryParse(node.GetElementValue("ispublished"), out isPublished);

                bool isShowInList = false;
                bool.TryParse(node.GetElementValue("showinlist"), out isShowInList);

                DateTime publicationDate = DateTime.MinValue;
                DateTime.TryParse(node.GetElementValue("pubDate"), out publicationDate);

                DateTime lastModificationDate = DateTime.MinValue;
                DateTime.TryParse(node.GetElementValue("lastModified"), out lastModificationDate);

                var tagElements = node.Descendants().Where(n => n.Name.LocalName == "tag");
                var tags = tagElements.Select(e => e.Value);

                var categoryNodes = node.Descendants().Where(n => n.Name.LocalName == "category");
                var categoryIds = categoryNodes.Select(n => Guid.Parse(n.Value));

                result = new ContentItem()
                {
                    Id = id,
                    IsPublished = isPublished,
                    ShowInList = isShowInList,
                    Title = node.GetElementValue("title"),
                    Description = node.GetElementValue("description"),
                    Content = node.GetElementValue("content"),
                    Slug = node.GetElementValue("slug"),
                    Author = author,
                    PublicationDate = publicationDate,
                    LastModificationDate = lastModificationDate,
                    Tags = tags,
                    ByLine = string.IsNullOrEmpty(author) ? string.Empty : $"by {author}",
                    CategoryIds = categoryIds
                };
            }

            return result;
        }
    }
}

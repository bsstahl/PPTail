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
        public static ContentItem ParseContentItem(this string xmlText, string nodeLocalName)
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
                string author = node.GetElementValue("author");

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

                result = new ContentItem()
                {
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
                    ByLine = string.IsNullOrEmpty(author) ? string.Empty : $"by {author}"
                };
            }

            return result;
        }
    }
}

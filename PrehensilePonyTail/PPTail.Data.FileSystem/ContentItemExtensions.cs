using PPTail.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Threading.Tasks;

namespace PPTail.Data.FileSystem
{
    public static class ContentItemExtensions
    {
        //TODO: Test each property
        public static SiteSettings ParseSettings(this string xmlText)
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
                result = new SiteSettings()
                {
                    Title = node.GetElementValue("name"),
                    Description = node.GetElementValue("description"),
                    PostsPerPage = Convert.ToInt32(node.GetElementValue("postsperpage"))
                };

            return result;
        }

        //TODO: Test each property
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
                result = new ContentItem()
                {
                    IsPublished = Convert.ToBoolean(node.GetElementValue("ispublished")),
                    Title = node.GetElementValue("title"),
                    Content = node.GetElementValue("content"),
                    Slug = node.GetElementValue("slug")
                };

            return result;
        }

    }
}

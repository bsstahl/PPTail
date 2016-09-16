using PPTail.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PPTail.Data.FileSystem
{
    public static class ContentItemExtensions
    {

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
            {
                string ppPage = node.GetElementValue("postsperpage");
                int postsPerPage = 0;
                Int32.TryParse(ppPage, out postsPerPage);

                result = new SiteSettings()
                {
                    Title = node.GetElementValue("name"),
                    Description = node.GetElementValue("description"),
                    PostsPerPage = postsPerPage
                };
            }

            return result;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PPTail.Entities;

namespace PPTail.Generator.T4Html
{
    public class PageGenerator : Interfaces.IPageGenerator
    {
        private string _styleTemplate;
        private string _homePageTemplate;
        private string _contentPageTemplate;
        private string _postPageTemplate;
        private string _itemTemplate;
        private string _dateTimeFormatSpecifier;
        private string _itemSeparator;

        public PageGenerator(string styleTemplate, string homePageTemplate, string contentPageTemplate, string postPageTemplate, string itemTemplate, string dateTimeFormatSpecifier, string itemSeparator)
        {
            _styleTemplate = styleTemplate;
            _homePageTemplate = homePageTemplate;
            _contentPageTemplate = contentPageTemplate;
            _postPageTemplate = postPageTemplate;
            _itemTemplate = itemTemplate;
            _dateTimeFormatSpecifier = dateTimeFormatSpecifier;
            _itemSeparator = itemSeparator;
        }

        private string StyleTemplate  { get { return _styleTemplate; } }
        private string HomePageTemplate { get { return _homePageTemplate;  } }
        private string ContentPageTemplate { get { return _contentPageTemplate; } }
        private string PostPageTemplate { get { return _postPageTemplate; } }
        private string ItemTemplate { get { return _itemTemplate; } }
        private string DateTimeFormatSpecifier { get { return _dateTimeFormatSpecifier; } }
        private string ItemSeparator { get { return _itemSeparator; } }


        //TODO: Test this method
        public string GenerateStylesheet(SiteSettings settings)
        {
            // TODO: Implement template processing (replace any settings values as needed)
            return _styleTemplate;
        }

        public string GenerateHomepage(SiteSettings settings, IEnumerable<ContentItem> posts)
        {
            return posts.ProcessTemplate(settings, this.HomePageTemplate, this.ItemTemplate, this.DateTimeFormatSpecifier, this.ItemSeparator);
        }

        public string GenerateContentPage(SiteSettings settings, ContentItem pageData)
        {
            return pageData.ProcessTemplate(settings, this.ContentPageTemplate, this.DateTimeFormatSpecifier);
        }

        //TODO: Test this method
        public string GeneratePostPage(SiteSettings settings, ContentItem article)
        {
            return article.ProcessTemplate(settings, this.PostPageTemplate, this.DateTimeFormatSpecifier);
        }


        public void LoadContentPageTemplate(string path)
        {
            throw new NotImplementedException();
            // _contentPageTemplate = System.IO.File.ReadAllText(path);
        }

        public void LoadPostPageTemplate(string path)
        {
            throw new NotImplementedException();
            // _postPageTemplate = System.IO.File.ReadAllText(path);
        }

    }
}

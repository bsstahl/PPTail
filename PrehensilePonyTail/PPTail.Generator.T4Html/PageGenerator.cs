using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PPTail.Entities;

namespace PPTail.Generator.T4Html
{
    public class PageGenerator : Interfaces.IPageGenerator
    {
        private string _contentPageTemplate;
        private string _postPageTemplate;
        private string _dateTimeFormatSpecifier;

        public PageGenerator(string contentPageTemplate, string postPageTemplate, string dateTimeFormatSpecifier)
        {
            _contentPageTemplate = contentPageTemplate;
            _dateTimeFormatSpecifier = dateTimeFormatSpecifier;
        }

        private string ContentPageTemplate
        {
            get
            {
                return _contentPageTemplate;
            }
        }

        private string DateTimeFormatSpecifier
        {
            get
            {
                return _dateTimeFormatSpecifier;
            }
        }

        public string GenerateContentPage(ContentItem pageData)
        {
            return pageData.ProcessTemplate(this.ContentPageTemplate, this.DateTimeFormatSpecifier);
        }

        public string GeneratePostPage(ContentItem article)
        {
            throw new NotImplementedException();
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

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

        public PageGenerator(string contentPageTemplate, string postPageTemplate)
        {
            _contentPageTemplate = contentPageTemplate;
        }

        private string ContentPageTemplate
        {
            get
            {
                return _contentPageTemplate;
            }
        }

        public string GenerateContentPage(ContentItem pageData)
        {
            return pageData.ProcessTemplate(this.ContentPageTemplate);
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

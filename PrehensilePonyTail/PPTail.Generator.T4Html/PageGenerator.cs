using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PPTail.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace PPTail.Generator.T4Html
{
    public class PageGenerator : Interfaces.IPageGenerator
    {
        // private string _contentPageTemplate;
        // private string _dateTimeFormatSpecifier;

        private readonly IServiceProvider _serviceProvider;
        private readonly Settings _settings;
        private readonly IEnumerable<Template> _templates;

        public PageGenerator(IServiceCollection container)
        {
            _serviceProvider = container.BuildServiceProvider();
            _settings = _serviceProvider.GetService<Settings>();
            _templates = _serviceProvider.GetService<IEnumerable<Template>>();
        }

        private Template ContentPageTemplate
        {
            get
            {
                return _templates.SingleOrDefault(t => t.TemplateType == Enumerations.TemplateType.ContentPage);
            }
        }

        private string DateTimeFormatSpecifier
        {
            get
            {
                return _settings.DateTimeFormatSpecifier;
            }
        }

        public string GenerateContentPage(ContentItem pageData)
        {
            return pageData.ProcessTemplate(this.ContentPageTemplate.Content, this.DateTimeFormatSpecifier);
        }

        public string GeneratePostPage(ContentItem article)
        {
            throw new NotImplementedException();
        }

        //public void LoadContentPageTemplate(string path)
        //{
        //    throw new NotImplementedException();
        //    // _contentPageTemplate = System.IO.File.ReadAllText(path);
        //}

        //public void LoadPostPageTemplate(string path)
        //{
        //    throw new NotImplementedException();
        //    // _postPageTemplate = System.IO.File.ReadAllText(path);
        //}
    }
}

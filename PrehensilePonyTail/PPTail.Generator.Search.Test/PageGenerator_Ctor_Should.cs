using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using TestHelperExtensions;
using PPTail.Interfaces;
using PPTail.Exceptions;
using PPTail.Entities;

namespace PPTail.Generator.Search.Test
{
    public class PageGenerator_Ctor_Should
    {
        [Fact]
        public void ThrowADependencyNotFoundExceptionIfTheServiceProviderIsNotProvided()
        {
            IServiceProvider serviceProvider = null;
            Assert.Throws(typeof(DependencyNotFoundException), () => (null as ISearchProvider).Create(serviceProvider));
        }

        [Fact]
        public void ThrowADependencyNotFoundExceptionIfTheTemplatesAreNotProvided()
        {
            IEnumerable<Template> templates = null;
            Assert.Throws(typeof(DependencyNotFoundException), () => (null as ISearchProvider).Create(templates));
        }

        [Fact]
        public void ThrowADependencyNotFoundExceptionIfTheSettingsAreNotProvided()
        {
            var templates = (null as IEnumerable<Template>).Create();
            Settings settings = null;
            SiteSettings siteSettings = Mock.Of<SiteSettings>();

            Assert.Throws(typeof(DependencyNotFoundException), () => (null as ISearchProvider).Create(templates, settings, siteSettings));
        }

        [Fact]
        public void ThrowADependencyNotFoundExceptionIfTheSiteSettingsAreNotProvided()
        {
            var templates = (null as IEnumerable<Template>).Create();
            Settings settings = Mock.Of<Settings>();
            SiteSettings siteSettings = null;

            Assert.Throws(typeof(DependencyNotFoundException), () => (null as ISearchProvider).Create(templates, settings, siteSettings));
        }

        [Fact]
        public void ThrowATemplateNotFoundExceptionIfThePageTemplateIsNotProvided()
        {
            var template = (null as Template).Create(Enumerations.TemplateType.Item);
            IEnumerable<Template> templates = (null as IEnumerable<Template>).Create(template, null);
            Assert.Throws(typeof(TemplateNotFoundException), () => (null as ISearchProvider).Create(templates));
        }

        [Fact]
        public void ThrowATemplateNotFoundExceptionIfTheItemTemplateIsNotProvided()
        {
            var template = (null as Template).Create(Enumerations.TemplateType.SearchPage);
            IEnumerable<Template> templates = (null as IEnumerable<Template>).Create(null, template);
            Assert.Throws(typeof(TemplateNotFoundException), () => (null as ISearchProvider).Create(templates));
        }
    }
}

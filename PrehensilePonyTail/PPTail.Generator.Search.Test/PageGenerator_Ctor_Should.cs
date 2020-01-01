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
using Microsoft.Extensions.DependencyInjection;

namespace PPTail.Generator.Search.Test
{
    public class PageGenerator_Ctor_Should
    {
        [Fact]
        public void ThrowAnArgumentNullExceptionIfTheServiceProviderIsNotProvided()
        {
            IServiceProvider serviceProvider = null;
            Assert.Throws<ArgumentNullException>(() => (null as ISearchProvider).Create(serviceProvider));
        }

        [Fact]
        public void ThrowADependencyNotFoundExceptionIfTheContentRepositoryIsNotProvided()
        {
            var container = (null as IServiceCollection).Create();
            container.RemoveDependency<IContentRepository>();
            Assert.Throws<DependencyNotFoundException>(() => (null as ISearchProvider).Create(container));
        }

        [Fact]
        public void ThrowWithTheProperInterfaceTypeNameIfTheContentRepositoryIsNotProvided()
        {
            var container = (null as IServiceCollection).Create();
            container.RemoveDependency<IContentRepository>();

            String expected = typeof(IContentRepository).Name;
            try
            {
                var target = (null as ISearchProvider).Create(container);
            }
            catch (DependencyNotFoundException ex)
            {
                Assert.Equal(expected, ex.InterfaceTypeName);
            }
        }

        [Fact]
        public void ThrowADependencyNotFoundExceptionIfTheTemplateProviderIsNotProvided()
        {
            var container = (null as IServiceCollection).Create();
            container.RemoveDependency<ITemplateProcessor>();
            Assert.Throws<DependencyNotFoundException>(() => (null as ISearchProvider).Create(container));
        }

        [Fact]
        public void ThrowWithTheProperInterfaceTypeNameIfTheTemplateProviderIsNotProvided()
        {
            var container = (null as IServiceCollection).Create();
            container.RemoveDependency<ITemplateProcessor>();

            String expected = typeof(ITemplateProcessor).Name;
            try
            {
                var target = (null as ISearchProvider).Create(container);
            }
            catch (DependencyNotFoundException ex)
            {
                Assert.Equal(expected, ex.InterfaceTypeName);
            }
        }

        [Fact]
        public void ThrowATemplateNotFoundExceptionIfThePageTemplateIsNotProvided()
        {
            var template = (null as Template).Create(Enumerations.TemplateType.Item);
            IEnumerable<Template> templates = (null as IEnumerable<Template>).Create(template, null);
            Assert.Throws<TemplateNotFoundException>(() => (null as ISearchProvider).Create(templates));
        }

        [Fact]
        public void ThrowATemplateNotFoundExceptionIfTheItemTemplateIsNotProvided()
        {
            var template = (null as Template).Create(Enumerations.TemplateType.SearchPage);
            IEnumerable<Template> templates = (null as IEnumerable<Template>).Create(null, template);
            Assert.Throws<TemplateNotFoundException>(() => (null as ISearchProvider).Create(templates));
        }

    }
}

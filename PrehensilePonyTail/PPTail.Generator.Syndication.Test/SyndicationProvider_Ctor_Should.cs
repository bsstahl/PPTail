using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using TestHelperExtensions;
using PPTail.Entities;
using PPTail.Interfaces;
using System.Text.RegularExpressions;
using PPTail.Exceptions;
using PPTail.Extensions;
using Microsoft.Extensions.DependencyInjection;
using PPTail.Enumerations;

namespace PPTail.Generator.Syndication.Test
{
    public class SyndicationProvider_Ctor_Should
    {
        [Fact]
        public void ThrowAnArgumentNullExceptionIfTheServiceProviderIsNotProvided()
        {
            IServiceProvider serviceProvider = null;
            Assert.Throws<ArgumentNullException>(() => (null as ISyndicationProvider).Create(serviceProvider));
        }

        [Fact]
        public void ReturnTheCorrectArgumentNameIfTheServiceProviderIsNotProvided()
        {
            IServiceProvider serviceProvider = null;

            String actual = string.Empty;
            try
            {
                var target = (null as ISyndicationProvider).Create(serviceProvider);
            }
            catch (ArgumentNullException ex)
            {
                actual = ex.ParamName;
            }

            Assert.Equal("serviceProvider", actual);
        }

        [Fact]
        public void ThrowADependencyNotFoundExceptionIfTheContentRepositoryIsNotProvided()
        {
            var container = (null as IServiceCollection).Create();
            container.RemoveDependency<IContentRepository>();
            Assert.Throws<DependencyNotFoundException>(() => (null as ISyndicationProvider).Create(container));
        }

        [Fact]
        public void ReturnTheCorrectDependencyNameIfTheContentRepositoryIsNotProvided()
        {
            var container = (null as IServiceCollection).Create();
            container.RemoveDependency<IContentRepository>();

            String expected = nameof(IContentRepository);
            String actual = string.Empty;
            try
            {
                var target = (null as ISyndicationProvider).Create(container);
            }
            catch (DependencyNotFoundException ex)
            {
                actual = ex.InterfaceTypeName;
            }

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ThrowADependencyNotFoundExceptionIfTheTemplateProcessorIsNotProvided()
        {
            var container = (null as IServiceCollection).Create();
            container.RemoveDependency<ITemplateProcessor>();
            Assert.Throws<DependencyNotFoundException>(() => (null as ISyndicationProvider).Create(container));
        }

        [Fact]
        public void ReturnTheCorrectDependencyNameIfTheTemplateProcessorIsNotProvided()
        {
            var container = (null as IServiceCollection).Create();
            container.RemoveDependency<ITemplateProcessor>();

            String actual = string.Empty;
            try
            {
                var target = (null as ISyndicationProvider).Create(container);
            }
            catch (DependencyNotFoundException ex)
            {
                actual = ex.InterfaceTypeName;
            }

            Assert.Equal("ITemplateProcessor", actual);
        }

        [Fact]
        public void ThrowATemplateNotFoundExceptionIfTheSyndicationTemplateIsNotProvided()
        {
            IServiceCollection container = (null as IServiceCollection).Create();

            var templates = new List<Template>
            {
                new Template() { Content = string.Empty.GetRandom(), TemplateType = TemplateType.SyndicationItem }
            };
            container.ReplaceTemplateRepo(templates);

            Assert.Throws<TemplateNotFoundException>(() => (null as ISyndicationProvider).Create(container));
        }

        [Fact]
        public void ReturnTheCorrectTemplateTypeIfTheSyndicationTemplateIsNotProvided()
        {
            IServiceCollection container = (null as IServiceCollection).Create();

            var templates = new List<Template>
            {
                new Template() { Content = string.Empty.GetRandom(), TemplateType = TemplateType.SyndicationItem }
            };
            container.ReplaceTemplateRepo(templates);

            TemplateType actual = TemplateType.Archive; // Anything but Syndication
            try
            {
                var target = (null as ISyndicationProvider).Create(container);
            }
            catch (TemplateNotFoundException ex)
            {
                actual = ex.TemplateType;
            }

            Assert.Equal(TemplateType.Syndication, actual);
        }

        [Fact]
        public void ThrowATemplateNotFoundExceptionIfTheSyndicationItemTemplateIsNotProvided()
        {
            IServiceCollection container = (null as IServiceCollection).Create();

            var templates = new List<Template>
            {
                new Template() { Content = string.Empty.GetRandom(), TemplateType = TemplateType.Syndication }
            };
            container.ReplaceTemplateRepo(templates);

            Assert.Throws<TemplateNotFoundException>(() => (null as ISyndicationProvider).Create(container));
        }

        [Fact]
        public void ReturnTheCorrectTemplateTypeIfTheSyndicationItemTemplateIsNotProvided()
        {
            IServiceCollection container = (null as IServiceCollection).Create();

            var templates = new List<Template>
            {
                new Template() { Content = string.Empty.GetRandom(), TemplateType = TemplateType.Syndication }
            };
            container.ReplaceTemplateRepo(templates);

            TemplateType actual = TemplateType.Archive; // Anything but Syndication
            try
            {
                var target = (null as ISyndicationProvider).Create(container);
            }
            catch (TemplateNotFoundException ex)
            {
                actual = ex.TemplateType;
            }

            Assert.Equal(TemplateType.SyndicationItem, actual);
        }
    }
}

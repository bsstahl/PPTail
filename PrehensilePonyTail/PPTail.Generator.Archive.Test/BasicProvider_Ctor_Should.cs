using Microsoft.Extensions.DependencyInjection;
using Moq;
using PPTail.Entities;
using PPTail.Exceptions;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestHelperExtensions;
using Xunit;

namespace PPTail.Generator.Archive.Test
{
    public class BasicProvider_Ctor_Should
    {
        [Fact]
        public void ThrowAnArgumentNullExceptionIfTheServiceProviderIsNotSupplied()
        {
            Assert.Throws<ArgumentNullException>(() => new Archive.BasicProvider(null));
        }

        [Fact]
        public void ThrowADependencyNotFoundExceptionIfTheTemplateProcessorIsNotProvided()
        {
            var container = (null as IServiceCollection).Create();
            container.RemoveDependency<ITemplateProcessor>();
            Assert.Throws<DependencyNotFoundException>(() => new Archive.BasicProvider(container.BuildServiceProvider()));
        }

        [Fact]
        public void ReturnTheProperInterfaceNameIfTheTemplateProcessorIsNotProvided()
        {
            var container = (null as IServiceCollection).Create();
            container.RemoveDependency<ITemplateProcessor>();

            String interfaceName = string.Empty;
            try
            {
                var target = new Archive.BasicProvider(container.BuildServiceProvider());
            }
            catch (DependencyNotFoundException ex)
            {
                interfaceName = ex.InterfaceTypeName;
            }

            Assert.Equal(nameof(ITemplateProcessor), interfaceName);
        }

        [Fact]
        public void ThrowATemplateNotFoundExceptionIfTheArchiveTemplateIsNotSupplied()
        {
            var container = (null as IServiceCollection).Create();

            var templates = (null as IEnumerable<Template>).Create();
            var activeTemplates = templates.Where(t => t.TemplateType != Enumerations.TemplateType.Archive);

            var templateRepo = new Mock<ITemplateRepository>();
            templateRepo.Setup(r => r.GetAllTemplates())
                .Returns(activeTemplates);
            container.ReplaceDependency<ITemplateRepository>(templateRepo.Object);

            Assert.Throws<TemplateNotFoundException>(() => new Archive.BasicProvider(container.BuildServiceProvider()));
        }

        [Fact]
        public void ThrowATemplateNotFoundExceptionIfTheArchiveItemTemplateIsNotSupplied()
        {
            var container = (null as IServiceCollection).Create();

            var templates = (null as IEnumerable<Template>).Create();
            var activeTemplates = templates.Where(t => t.TemplateType != Enumerations.TemplateType.ArchiveItem);

            var templateRepo = new Mock<ITemplateRepository>();
            templateRepo.Setup(r => r.GetAllTemplates())
                .Returns(activeTemplates);
            container.ReplaceDependency<ITemplateRepository>(templateRepo.Object);

            Assert.Throws<TemplateNotFoundException>(() => new Archive.BasicProvider(container.BuildServiceProvider()));
        }

        [Fact]
        public void ReturnTheProperTemplateTypeIfTheArchiveTemplateIsNotSupplied()
        {
            var container = (null as IServiceCollection).Create();

            var templates = (null as IEnumerable<Template>).Create();
            var activeTemplates = templates.Where(t => t.TemplateType != Enumerations.TemplateType.Archive);

            var templateRepo = new Mock<ITemplateRepository>();
            templateRepo.Setup(r => r.GetAllTemplates())
                .Returns(activeTemplates);
            container.ReplaceDependency<ITemplateRepository>(templateRepo.Object);

            Enumerations.TemplateType actual = Enumerations.TemplateType.Bootstrap;
            try
            {
                var target = new Archive.BasicProvider(container.BuildServiceProvider());
            }
            catch (TemplateNotFoundException ex)
            {
                actual = ex.TemplateType;
            }

            Assert.Equal(Enumerations.TemplateType.Archive, actual);
        }

        [Fact]
        public void ReturnTheProperTemplateNameIfTheArchiveItemTemplateIsNotSupplied()
        {
            var container = (null as IServiceCollection).Create();

            var templates = (null as IEnumerable<Template>).Create();
            var activeTemplates = templates.Where(t => t.TemplateType != Enumerations.TemplateType.ArchiveItem);

            var templateRepo = new Mock<ITemplateRepository>();
            templateRepo.Setup(r => r.GetAllTemplates())
                .Returns(activeTemplates);
            container.ReplaceDependency<ITemplateRepository>(templateRepo.Object);

            Enumerations.TemplateType actual = Enumerations.TemplateType.Bootstrap;
            try
            {
                var target = new Archive.BasicProvider(container.BuildServiceProvider());
            }
            catch (TemplateNotFoundException ex)
            {
                actual = ex.TemplateType;
            }

            Assert.Equal(Enumerations.TemplateType.ArchiveItem, actual);
        }
    }
}

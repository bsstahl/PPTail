using Microsoft.Extensions.DependencyInjection;
using PPTail.Entities;
using PPTail.Exceptions;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using TestHelperExtensions;

namespace PPTail.Generator.T4Html.Test
{
    public class PageGenerator_GenerateBootstrapPage_Should
    {

        [Fact]
        public void ReturnAnEmptyStringIfTheTemplateIsNotSupplied()
        {
            var allTemplates = (null as IEnumerable<Template>).CreateBlankTemplates();
            var templates = allTemplates.Where(t => t.TemplateType != Enumerations.TemplateType.Bootstrap);
            var settings = (null as Settings).CreateDefault("MM/dd/yyyy");

            var container = new ServiceCollection();
            container.AddSingleton<IEnumerable<Template>>(templates);
            container.AddSingleton<ISettings>(settings);

            var target = (null as IPageGenerator).Create(templates, settings);
            var actual = target.GenerateBootstrapPage();

            Assert.Equal(string.Empty, actual);
        }

        [Fact]
        public void ReturnTheTemplateIfItIsSupplied()
        {
            var allTemplates = (null as IEnumerable<Template>).CreateBlankTemplates();
            var thisTemplate = allTemplates.Single(t => t.TemplateType == Enumerations.TemplateType.Bootstrap);
            thisTemplate.Content = string.Empty.GetRandom();

            var settings = (null as Settings).CreateDefault("MM/dd/yyyy");

            var container = new ServiceCollection();
            container.AddSingleton<IEnumerable<Template>>(allTemplates);
            container.AddSingleton<ISettings>(settings);

            var target = (null as IPageGenerator).Create(allTemplates, settings);
            var actual = target.GenerateBootstrapPage();

            Assert.Equal(thisTemplate.Content, actual);
        }


    }
}

﻿using Microsoft.Extensions.DependencyInjection;
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
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class PageGenerator_GenerateBootstrapPage_Should
    {

        [Fact]
        public void ReturnAnEmptyStringIfTheTemplateIsNotSupplied()
        {
            var allTemplates = (null as IEnumerable<Template>).CreateBlankTemplates();
            var templates = allTemplates.Where(t => t.TemplateType != Enumerations.TemplateType.Bootstrap);

            var container = new ServiceCollection();
            container.AddSingleton<IEnumerable<Template>>(templates);

            var target = (null as IPageGenerator).Create(templates);
            var actual = target.GenerateBootstrapPage();

            Assert.Equal(string.Empty, actual);
        }

        [Fact]
        public void ReturnTheTemplateIfItIsSupplied()
        {
            var allTemplates = (null as IEnumerable<Template>).CreateBlankTemplates();
            var thisTemplate = allTemplates.Single(t => t.TemplateType == Enumerations.TemplateType.Bootstrap);
            thisTemplate.Content = string.Empty.GetRandom();

            var container = new ServiceCollection();
            container.AddSingleton<IEnumerable<Template>>(allTemplates);

            var target = (null as IPageGenerator).Create(allTemplates);
            var actual = target.GenerateBootstrapPage();

            Assert.Equal(thisTemplate.Content, actual);
        }


    }
}

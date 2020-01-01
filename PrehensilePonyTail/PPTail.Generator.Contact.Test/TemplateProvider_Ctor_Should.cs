﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using TestHelperExtensions;
using PPTail.Interfaces;
using PPTail.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using PPTail.Entities;

namespace PPTail.Generator.Contact.Test
{
    public class TemplateProvider_Ctor_Should
    {
        [Fact]
        public void ThrowArgumentNullExceptionIfServiceProviderNotProvided()
        {
            Assert.Throws<ArgumentNullException>(() => new TemplateProvider(null));
        }

        //[Fact]
        //public void ThrowDependencyNotFoundExceptionIfSiteSettingsAreNotProvided()
        //{
        //    String navigationContent = string.Empty.GetRandom();
        //    String sidebarContent = string.Empty.GetRandom();
        //    String pathToRoot = string.Empty.GetRandom();

        //    var template = (null as Template).Create();
        //    var templates = new List<Template>() { template };

        //    var container = new ServiceCollection();
        //    container.AddSingleton<IEnumerable<Template>>(templates);
        //    container.AddSingleton<ISettings>(Mock.Of<ISettings>());

        //    Assert.Throws<DependencyNotFoundException>(() => new TemplateProvider(container.BuildServiceProvider()));
        //}

        [Fact]
        public void ThrowWithProperInterfaceTypeNameIfSiteSettingsAreNotProvided()
        {
            String navigationContent = string.Empty.GetRandom();
            String sidebarContent = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();

            var template = (null as Template).Create();
            var templates = new List<Template>() { template };

            var container = new ServiceCollection();
            container.AddSingleton<IEnumerable<Template>>(templates);
            container.AddSingleton<ISettings>(Mock.Of<ISettings>());

            String expected = typeof(SiteSettings).Name;
            try
            {
                var target = new TemplateProvider(container.BuildServiceProvider());
            }
            catch (DependencyNotFoundException ex)
            {
                Assert.Equal(expected, ex.InterfaceTypeName);
            }
        }

        [Fact]
        public void ThrowDependencyNotFoundExceptionIfSettingsAreNotProvided()
        {
            String navigationContent = string.Empty.GetRandom();
            String sidebarContent = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();

            var template = (null as Template).Create();
            var templates = new List<Template>() { template };

            var container = new ServiceCollection();
            container.AddSingleton<IEnumerable<Template>>(templates);
            // container.AddSingleton<SiteSettings>(Mock.Of<SiteSettings>());

            Assert.Throws<DependencyNotFoundException>(() => new TemplateProvider(container.BuildServiceProvider()));
        }

        [Fact]
        public void ThrowWithProperInterfaceTypeNameIfSettingsAreNotProvided()
        {
            String navigationContent = string.Empty.GetRandom();
            String sidebarContent = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();

            var template = (null as Template).Create();
            var templates = new List<Template>() { template };

            var container = new ServiceCollection();
            container.AddSingleton<IEnumerable<Template>>(templates);
            // container.AddSingleton<SiteSettings>(Mock.Of<SiteSettings>());

            String expected = typeof(ISettings).Name;
            try
            {
                var target = new TemplateProvider(container.BuildServiceProvider());
            }
            catch (DependencyNotFoundException ex)
            {
                Assert.Equal(expected, ex.InterfaceTypeName);
            }
        }

        [Fact]
        public void ThrowTemplateNotFoundExceptionIfContactPageTemplateNotProvided()
        {
            String navigationContent = string.Empty.GetRandom();
            String sidebarContent = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();

            var templates = new List<Template>();
            var siteSettings = (null as SiteSettings).Create();
            var settings = (null as ISettings).Create();

            var container = new ServiceCollection();
            container.AddSingleton<ISettings>(settings);
            container.AddSingleton<IEnumerable<Template>>(templates);

            Assert.Throws<TemplateNotFoundException>(() => new TemplateProvider(container.BuildServiceProvider()));
        }

    }
}

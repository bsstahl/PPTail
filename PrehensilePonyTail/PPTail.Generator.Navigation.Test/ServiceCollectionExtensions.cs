﻿using Microsoft.Extensions.DependencyInjection;
using Moq;
using PPTail.Builders;
using PPTail.Entities;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace PPTail.Generator.Navigation.Test
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddContentRepository(this IServiceCollection container)
        {
            return container.AddContentRepository(new Mock<IContentRepository>());
        }

        public static IServiceCollection AddContentRepository(this IServiceCollection container, Mock<IContentRepository> mockContentRepo)
        {
            var siteSettings = new SiteSettingsBuilder().Build();
            return container.AddContentRepository(mockContentRepo, siteSettings);
        }

        public static IServiceCollection AddContentRepository(this IServiceCollection container, Mock<IContentRepository> mockContentRepo, SiteSettings siteSettings)
        {
            mockContentRepo.Setup(r => r.GetSiteSettings()).Returns(siteSettings);
            return container.AddSingleton<IContentRepository>(mockContentRepo.Object);
        }

        public static IServiceCollection AddLinkProvider(this IServiceCollection container)
        {
            var linkProvider = new Mock<ILinkProvider>();
            linkProvider
                .Setup(l => l.GetUrl(string.Empty, It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>()))
                .Returns((string pathToRoot, string relativePath, string filename, string extension) => $"{relativePath}\\{filename}.{extension}");
            return container.AddLinkProvider(linkProvider.Object);
        }

        public static IServiceCollection AddLinkProvider(this IServiceCollection container, ILinkProvider linkProvider)
        {
            return container.AddSingleton<ILinkProvider>(linkProvider);
        }
    }
}

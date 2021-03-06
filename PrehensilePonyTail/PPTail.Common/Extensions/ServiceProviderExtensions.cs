﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PPTail.Entities;
using PPTail.Interfaces;

namespace PPTail.Extensions
{
    public static class ServiceProviderExtensions
    {
        public static void ValidateService<T>(this IServiceProvider serviceProvider)
        {
            T service = serviceProvider.GetService<T>();
            if (service == null)
                throw new Exceptions.DependencyNotFoundException(typeof(T).Name);
        }

        public static IEnumerable<Entities.Template> GetTemplates(this IServiceProvider serviceProvider)
        {
            var templateRepo = serviceProvider.GetService<ITemplateRepository>();
            return templateRepo.GetAllTemplates();
        }

        public static SiteSettings GetSiteSettings(this IServiceProvider serviceProvider)
        {
            var contentRepo = serviceProvider.GetService<IContentRepository>();
            return contentRepo.GetSiteSettings();
        }
    }
}

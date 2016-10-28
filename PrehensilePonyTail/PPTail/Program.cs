using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PPTail.Entities;
using PPTail.Enumerations;
using PPTail.Interfaces;

namespace PPTail
{
    public class Program
    {
        public static void Main(string[] args)
        {
            const string _sourceDataPathSettingName = "sourceDataPath";
            const string _outputPathSettingName = "outputPath";

            string outputFileExtension = "html";
            string dateFormatSpecifier = "yyyy-MM-dd";
            string dateTimeFormatSpecifier = "yyyy-MM-dd H:mm UTC";
            string itemSeparator = "<hr/>";
            string additionalFilePaths = "images,pics";

            string styleTemplatePath = "..\\Style.template.css";
            string bootstrapTemplatePath = "..\\bootstrap.min.css";
            string homePageTemplatePath = "..\\HomePage.template.html";
            string searchPageTemplatePath = "..\\ContentPage.template.html";
            string contentPageTemplatePath = "..\\ContentPage.template.html";
            string postPageTemplatePath = "..\\PostPage.template.html";
            string redirectTemplatePath = "..\\Redirect.template.html";
            string archiveTemplatePath = "..\\Archive.template.html";
            string archiveItemTemplatePath = "..\\ArchiveItem.template.html";
            string syndicationTemplatePath = "..\\Syndication.template.xml";
            string syndicationItemTemplatePath = "..\\SyndicationItem.template.xml";
            string contactPageTemplatePath = "..\\ContactPage.template.html";
            string itemTemplatePath = "..\\ContentItem.template.html";

            bool createDasBlogSyndicationCompatibilityFile = true;

            var builder = new ConfigurationBuilder().AddEnvironmentVariables();
            var config = builder.Build();

            string sourceDataPath = config[_sourceDataPathSettingName];
            string outputPath = config[_outputPathSettingName];

            var settings = (null as ISettings).Create(sourceDataPath, outputPath, dateFormatSpecifier, dateTimeFormatSpecifier, itemSeparator, outputFileExtension, additionalFilePaths, createDasBlogSyndicationCompatibilityFile);
            var templates = (null as IEnumerable<Template>).Create(styleTemplatePath, bootstrapTemplatePath, homePageTemplatePath, contentPageTemplatePath, postPageTemplatePath, contactPageTemplatePath, redirectTemplatePath, syndicationTemplatePath, syndicationItemTemplatePath, itemTemplatePath, searchPageTemplatePath, archiveTemplatePath, archiveItemTemplatePath);

            var container = (null as IServiceCollection).Create(settings, templates);

            var serviceProvider = container.BuildServiceProvider();

            var siteBuilder = serviceProvider.GetService<PPTail.SiteGenerator.Builder>();
            var sitePages = siteBuilder.Build();

            var outputRepo = serviceProvider.GetService<Interfaces.IOutputRepository>();
            outputRepo.Save(sitePages);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PPTail.Entities;
using Microsoft.Extensions.DependencyInjection;
using System.Xml.Linq;
using PPTail.Interfaces;
using PPTail.Extensions;

namespace PPTail.Data.FileSystem.Wordpress
{
    public class Repository : Interfaces.IContentRepository
    {
        //const int _defaultPostsPerPage = 3;
        //const int _defaultPostsPerFeed = 5;

        //const string _widgetRelativePath = "datastore\\widgets";
        //const string _categoriesRelativePath = "categories.xml";


        const string _sourceDataPathSettingName = "sourceDataPath";

        private readonly IServiceProvider _serviceProvider;
        private readonly string _rootDataPath;
        private readonly string _rootSitePath;


        public Repository(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _serviceProvider.ValidateService<ISettings>();
            _serviceProvider.ValidateService<IFile>();

            var settings = _serviceProvider.GetService<ISettings>();
            settings.Validate(_sourceDataPathSettingName);

            _rootSitePath = settings.ExtendedSettings.Get(_sourceDataPathSettingName);
            _rootDataPath = System.IO.Path.Combine(_rootSitePath, "App_Data");
        }

        public IEnumerable<ContentItem> GetAllPages()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ContentItem> GetAllPosts()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Widget> GetAllWidgets()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Category> GetCategories()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SourceFile> GetFolderContents(string relativePath)
        {
            throw new NotImplementedException();
        }

        public SiteSettings GetSiteSettings()
        {
            throw new NotImplementedException();
        }
    }
}

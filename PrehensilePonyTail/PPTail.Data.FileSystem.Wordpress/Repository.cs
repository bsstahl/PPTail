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

        const string _defaultAuthorName = "AZGiveCamp";

        const string _sourceDataPathSettingName = "sourceDataPath";

        private readonly IServiceProvider _serviceProvider;
        private readonly string _rootDataPath;

        private IDictionary<int, string> _users;


        public Repository(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _serviceProvider.ValidateService<ISettings>();
            _serviceProvider.ValidateService<IFile>();

            var settings = _serviceProvider.GetService<ISettings>();
            settings.Validate(_sourceDataPathSettingName);

            _rootDataPath = settings.ExtendedSettings.Get(_sourceDataPathSettingName);

            _users = new Dictionary<int, string>();
            _users.Add(0, _defaultAuthorName);
        }

        public Repository(IServiceProvider serviceProvider, IDictionary<int, string> users)
            : this(serviceProvider)
        {
            _users = users;
        }

        public IEnumerable<ContentItem> GetAllPages()
        {
            var fileSystem = _serviceProvider.GetService<IFile>();
            var directory = _serviceProvider.GetService<IDirectory>();

            var results = new List<ContentItem>();
            var files = directory.EnumerateFiles(_rootDataPath);

            // TODO: Handle if page not found or multiple pages found
            var pageFile = files.Single(f => f.ToLowerInvariant().Contains("pages.json"));

            var contentItems = fileSystem.ReadAllText(pageFile).ParseContentItems("page", _users, _defaultAuthorName);
            results.AddRange(contentItems);

            return results;
        }

        public IEnumerable<ContentItem> GetAllPosts()
        {
            var fileSystem = _serviceProvider.GetService<IFile>();
            var directory = _serviceProvider.GetService<IDirectory>();

            var results = new List<ContentItem>();
            var files = directory.EnumerateFiles(_rootDataPath);

            // TODO: Handle if page not found or multiple pages found
            var postsFile = files.Single(f => f.ToLowerInvariant().Contains("posts.json"));

            var contentItems = fileSystem.ReadAllText(postsFile).ParseContentItems("post", _users, _defaultAuthorName);
            results.AddRange(contentItems);

            return results;
        }

        public IEnumerable<Widget> GetAllWidgets()
        {
            return new List<Widget>();
        }

        public IEnumerable<Category> GetCategories()
        {
            return new List<Category>();
        }

        public IEnumerable<SourceFile> GetFolderContents(string relativePath)
        {
            return new List<SourceFile>();
        }

        public SiteSettings GetSiteSettings()
        {
            return new SiteSettings()
            {
                Description = "Coding for Charity",
                PostsPerFeed = 10,
                PostsPerPage = 5,
                Title = "AZGiveCamp"
            };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using PPTail.Entities;
using PPTail.Interfaces;

namespace PPTail.Data.WordpressFiles
{
    public class Repository : Interfaces.IContentRepository
    {
        readonly string _dataFilePath;

        public Repository(string dataFilePath)
        {
            _dataFilePath = dataFilePath;
        }

        public IEnumerable<ContentItem> GetAllPages()
        {
            var result = new List<ContentItem>();

            var authors = GetAllUsers();

            var fullPath = System.IO.Path.Combine(_dataFilePath, "pages.json");
            var pagesJson = System.IO.File.ReadAllText(fullPath);
            var pages = Newtonsoft.Json.JsonConvert.DeserializeObject<Page[]>(pagesJson);

            foreach (var page in pages)
            {
                string authorName = authors.Single(a => a.Key == page.author).Value;
                result.Add(new ContentItem()
                {
                    Author = authorName,
                    ByLine = $"by {authorName}",
                    CategoryIds = new List<System.Guid>(),
                    Content = page.content.rendered,
                    Description = page.excerpt.rendered,
                    Id = System.Guid.NewGuid(),
                    IsPublished = (page.status.ToLower() == "publish"),
                    PublicationDate = page.date,
                    LastModificationDate = page.modified,
                    ShowInList = (page.menu_order > 0),
                    Tags = new List<string>(),
                    Title = page.title.rendered,
                    Slug = page.slug
                });
            }

            return result;
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

        private IEnumerable<KeyValuePair<int, string>> GetAllUsers()
        {
            // TODO: Implement for real
            return new List<KeyValuePair<int, string>>()
            {
                new KeyValuePair<int, string>(11, "User11"),
                new KeyValuePair<int, string>(21, "User21"),
                new KeyValuePair<int, string>(31, "User31")
            };
        }
    }
}

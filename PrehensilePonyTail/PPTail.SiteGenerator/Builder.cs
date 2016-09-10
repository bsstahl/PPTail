using PPTail.Entities;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.SiteGenerator
{
    public class Builder
    {
        private readonly IContentRepository _contentRepo;
        private readonly IPageGenerator _pageGen;

        public Builder(IContentRepository contentRepo, IPageGenerator pageGen, string pageFilenameExtension)
        {
            _contentRepo = contentRepo;
        }

        public IEnumerable<SiteFile> Build()
        {
            var result = new List<SiteFile>();

            var posts = _contentRepo.GetAllPosts();
            foreach (var post in posts)
            {
                // All all content pages to the results
                result.Add(new SiteFile()
                {
                    RelativeFilePath = ".\\Posts\\",
                    Content = ""
                });
            }

            var pages = _contentRepo.GetAllPages();
            foreach (var page in pages)
            {
                // All all content pages to the results
                result.Add(new SiteFile()
                {
                    RelativeFilePath = ".\\Pages\\",
                    Content = ""
                });
            }

            return result;
        }
    }
}

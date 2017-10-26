using PPTail.Interfaces;
using System;
using PPTail.Entities;
using System.Collections.Generic;

namespace PPTail.Service.BlogPosts.Orchestration
{
    public class Provider : IPostPageGenerator
    {
        IContentEncoder _contentEncoder;
        IContentItemPageGenerator _contentItemPageGen;
        IRedirectProvider _redirectProvider;

        public Provider(IContentEncoder contentEncoder, IContentItemPageGenerator contentItemPageGen, IRedirectProvider redirectProvider)
        {
            _contentEncoder = contentEncoder;
            _contentItemPageGen = contentItemPageGen;
            _redirectProvider = redirectProvider;
        }

        public IEnumerable<SiteFile> GetPostPages(ContentPageSource pageSource)
        {
            if (pageSource == null)
                throw new ArgumentNullException(nameof(pageSource));

            if (pageSource.ContentItem == null)
                throw new ArgumentNullException(nameof(pageSource.ContentItem));

            if (pageSource.Settings == null)
                throw new ArgumentNullException(nameof(pageSource.Settings));

            var result = new List<SiteFile>();

            var post = pageSource.ContentItem;
            var settings = pageSource.Settings;

            if (string.IsNullOrWhiteSpace(post.Slug))
                post.Slug = _contentEncoder.UrlEncode(post.Title);

            // Add the post page
            string postFileName = $"{post.Slug}.{settings.OutputFileExtension}";
            string postFilePath = System.IO.Path.Combine("Posts", postFileName);
            var postPageTemplateType = Enumerations.TemplateType.PostPage;
            result.Add(new SiteFile()
            {
                RelativeFilePath = postFilePath,
                SourceTemplateType = postPageTemplateType,
                Content = _contentItemPageGen.Generate(pageSource.SidebarContent, pageSource.NavigationContent, post, postPageTemplateType, "..", false)
            });

            // Add the permalink page
            string permalinkFileName = $"{_contentEncoder.HTMLEncode(post.Id.ToString())}.{settings.OutputFileExtension}";
            string permalinkFilePath = System.IO.Path.Combine("Permalinks", permalinkFileName);
            string redirectFilePath = System.IO.Path.Combine("..", postFilePath);
            result.Add(new SiteFile()
            {
                RelativeFilePath = permalinkFilePath,
                SourceTemplateType = Enumerations.TemplateType.Redirect,
                Content = _redirectProvider.GenerateRedirect(redirectFilePath)
            });

            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PPTail.Entities;
using PPTail.Interfaces;

namespace PPTail.Service.BlogPosts
{
    [Route("")]
    public class ValuesController : Controller
    {
        // I'm alive
        [HttpGet]
        public bool Get()
        {
            return true;
        }

        // Submit a blog post for page generation
        [HttpPost]
        public IEnumerable<SiteFile> Post(IContentEncoder contentEncoder, IContentItemPageGenerator contentItemPageGen, IRedirectProvider redirectProvider, [FromBody]ContentPageSource pageSource)
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
                post.Slug = contentEncoder.UrlEncode(post.Title);

            // Add the post page
            string postFileName = $"{post.Slug}.{settings.OutputFileExtension}";
            string postFilePath = System.IO.Path.Combine("Posts", postFileName);
            var postPageTemplateType = Enumerations.TemplateType.PostPage;
            result.Add(new SiteFile()
            {
                RelativeFilePath = postFilePath,
                SourceTemplateType = postPageTemplateType,
                Content = contentItemPageGen.Generate(pageSource.SidebarContent, pageSource.NavigationContent, post, postPageTemplateType, "..", false)
            });

            // Add the permalink page
            string permalinkFileName = $"{contentEncoder.HTMLEncode(post.Id.ToString())}.{settings.OutputFileExtension}";
            string permalinkFilePath = System.IO.Path.Combine("Permalinks", permalinkFileName);
            string redirectFilePath = System.IO.Path.Combine("..", postFilePath);
            result.Add(new SiteFile()
            {
                RelativeFilePath = permalinkFilePath,
                SourceTemplateType = Enumerations.TemplateType.Redirect,
                Content = redirectProvider.GenerateRedirect(redirectFilePath)
            });

            return result;
        }

    }
}

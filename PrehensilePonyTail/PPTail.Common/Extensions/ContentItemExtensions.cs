using PPTail.Entities;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PPTail.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace PPTail.Extensions
{
    public static class ContentItemExtensions
    {
        //public static string GetLinkUrl(this ContentItem post, string pathToRoot, string outputFileExtension)
        //{
        //    string filename = $"{post.Slug.ToString()}.{outputFileExtension}";
        //    return System.IO.Path.Combine(pathToRoot, "Posts", filename).ToHttpSlashes();
        //}

        //public static string GetPermalink(this ContentItem post, string pathToRoot, string outputFileExtension, string linkText)
        //{
        //    string filename = $"{post.Id.ToString()}.{outputFileExtension}";
        //    string url = System.IO.Path.Combine(pathToRoot, "Permalinks", filename).ToHttpSlashes();
        //    return $"<a href=\"{url}\" rel=\"bookmark\">{linkText}</a>";
        //}
    }
}

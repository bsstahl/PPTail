using Microsoft.AspNetCore.Http;
using PPTail.Extensions;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PPTail.Exceptions;

namespace PPTail.Web.Permalinks
{
    public class DasBlogCompatibility
    {
        const string _dasBlogPostsFile = "post.aspx";

        RequestDelegate _next;
        IServiceProvider _serviceProvider;

        public DasBlogCompatibility(RequestDelegate next, IServiceProvider serviceProvider)
        {
            if (next == null)
                throw new ArgumentNullException(nameof(next));

            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));

            _serviceProvider = serviceProvider;
            _serviceProvider.ValidateService<IPostLocator>();

            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            string path = context.Request.Path.Value.ToLower();
            if (path.EndsWith(_dasBlogPostsFile))
            {
                var request = context.Request;
                var query = request.Query;
                var queryId = query["id"];

                Guid id;
                if (Guid.TryParse(queryId, out id))
                {
                    var postLocator = _serviceProvider.GetService<IPostLocator>();
                    try
                    {
                        string url = postLocator.GetUrlByPostId(id);
                        context.Response.Redirect(url, true);
                    }
                    catch (PostNotFoundException ex)
                    {
                        context.Response.Headers.AddHeader("ErrorMessage", ex.Message);
                        context.Response.Headers.AddHeader("PostId", ex.PostId.ToString());
                        context.Response.StatusCode = 404;
                    }
                }
                else
                    context.Response.StatusCode = 404;
            }
            else
                await _next.Invoke(context);
        }


    }
}

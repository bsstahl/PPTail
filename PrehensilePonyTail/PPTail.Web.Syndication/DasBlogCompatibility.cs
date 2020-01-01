using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace PPTail.Web.Syndication
{
    public class DasBlogCompatibility
    {
        const String _dasBlogSyndicationFile = "syndication.axd";
        const String _currentSyndicationFile = "syndication.xml";

        RequestDelegate _next;

        public DasBlogCompatibility(RequestDelegate next)
        {
            if (next == null)
                throw new ArgumentNullException(nameof(next));

            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            String path = context.Request.Path.Value.ToLower();
            if (path.EndsWith(_dasBlogSyndicationFile))
            {
                String newPath = path.Replace(_dasBlogSyndicationFile, _currentSyndicationFile);
                context.Response.Redirect(newPath, true);
            }
            else
                await _next.Invoke(context);
        }


    }
}

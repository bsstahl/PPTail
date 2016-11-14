using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseDasBlogSyndication(this IApplicationBuilder app)
        {
            return app.UseMiddleware<PPTail.Web.Syndication.DasBlogCompatibility>();
        }
    }


}

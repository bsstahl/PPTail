using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Data.FileSystem.Test
{
    public static class Extensions
    {
        public static IContentRepository Create(this IContentRepository ignore)
        {
            return new PPTail.Data.FileSystem.Repository();
        }
    }
}

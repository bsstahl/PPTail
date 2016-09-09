using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Generator.T4Html.Test
{
    public static class Extensions
    {
        public static IPageGenerator Create(this IPageGenerator ignore)
        {
            return new PPTail.Generator.T4Html.PageGenerator();
        }
    }
}

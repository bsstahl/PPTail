﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PPTail.Entities;

namespace PPTail.Generator.HomePage.Test
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class FakeNavProvider : Interfaces.INavigationProvider
    {
        public String CreateNavigation(IEnumerable<ContentItem> pages, String homeUrl, String outputFileExtension)
        {
            return "<div class=\"menu\">Place Nav Here</div>";
        }
    }
}

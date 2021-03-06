﻿using PPTail.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPTail.Generator.Navigation
{
    public static class StringBuilderExtensions
    {
        internal static StringBuilder AppendRootLink(this StringBuilder sb, string linkText, string uri)
        {
            sb.AppendLine($"<li class=\"nav-item\">");
            sb.AppendLine($"<a class=\"nav-link\" href=\"{uri}\">{linkText}</a>");
            sb.AppendLine("</li>");
            return sb;
        }

        internal static StringBuilder AppendChildLink(this StringBuilder sb, string linkText, string uri)
        {
            return sb.AppendLine($"<a class=\"dropdown-item\" href=\"{uri}\">{linkText}</a>");
        }
    }
}

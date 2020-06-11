﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PPTail.Generator.Navigation
{
    public static class StringBuilderExtensions
    {
        public static StringBuilder AppendRootLink(this StringBuilder sb, string linkText, string uri, bool isActive = false)
        {
            string activeClass = isActive ? "active" : "";
            sb.AppendLine($"<li class=\"nav-item {activeClass}\">");
            sb.AppendLine($"<a class=\"nav-link\" href=\"{uri}\">{linkText}</a>");
            sb.AppendLine("</li>");
            return sb;
        }

        public static StringBuilder AppendChildLink(this StringBuilder sb, string linkText, string uri)
        {
            return sb.AppendLine($"<a class=\"dropdown-item\" href=\"{uri}\">{linkText}</a>");
        }
    }
}

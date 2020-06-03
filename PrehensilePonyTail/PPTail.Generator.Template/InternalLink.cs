using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PPTail.Generator.Template
{
    internal class InternalLink
    {
        public string LinkText { get; set; }
        public string PathToRoot { get; set; }
        public string RelativePath { get; set; }
        public string FileName { get; set; }

        readonly ILinkProvider _linkProvider;

        public InternalLink(ILinkProvider linkProvider)
        {
            _linkProvider = linkProvider;
        }

        public InternalLink(ILinkProvider linkProvider, String linkText, string pathToRoot, string relativePath, string fileName)
            :this(linkProvider)
        {
            this.LinkText = linkText;
            this.PathToRoot = pathToRoot;
            this.RelativePath = relativePath;
            this.FileName = fileName;
        }

        public override String ToString()
        {
            string linkUrl = _linkProvider.GetUrl(this.PathToRoot, this.RelativePath, this.FileName);
            string result = $"<a href=\"{linkUrl}\">{this.LinkText}</a>";
            return result;
        }

    }
}

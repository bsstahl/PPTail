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

        public String FileNameWithoutExtension 
        { 
            get => System.IO.Path.GetFileNameWithoutExtension(this.FileName);
        }

        public String FileExtension
        {
            get => System.IO.Path.GetExtension(this.FileName).Trim('.');
        }

        public String AsImageEmbedding()
        {
            string src = _linkProvider.GetUrl(this.PathToRoot, this.RelativePath, this.FileNameWithoutExtension, this.FileExtension);
            
            string result = $"<img src=\"{src}\" ";
            if (this.HasLinkText())
                result += $"alt=\"{this.LinkText}\"";
            result += " />";

            return result;
        }

        public string AsUrl(bool addDefaultFileExtension = false)
        {
            return this.GetUrl(addDefaultFileExtension);
        }

        public string AsLink(bool addDefaultFileExtension = false)
        {
            return  $"<a href=\"{this.GetUrl(addDefaultFileExtension)}\">{this.LinkText}</a>";
        }

        private string GetUrl(bool addDefaultFileExtension)
        {
            return addDefaultFileExtension
                ? _linkProvider.GetUrl(this.PathToRoot, this.RelativePath, this.FileName)
                : _linkProvider.GetUrl(this.PathToRoot, this.RelativePath, this.FileNameWithoutExtension, this.FileExtension);
        }

        public override String ToString()
        {
            return this.AsLink(true);
        }

        private bool HasLinkText()
        {
            return (!String.IsNullOrWhiteSpace(this.LinkText));
        }

    }
}

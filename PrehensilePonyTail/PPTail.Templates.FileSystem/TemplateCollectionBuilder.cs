using PPTail.Entities;
using PPTail.Enumerations;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPTail.Templates.FileSystem
{
    public class TemplateCollectionBuilder
    {
        private readonly List<Entities.Template> _templates = new List<Entities.Template>();

        private readonly IFile _fileProvider;
        private readonly String _rootTemplatePath;

        public TemplateCollectionBuilder(IFile fileProvider, string rootPath)
        {
            _fileProvider = fileProvider;
            _rootTemplatePath = rootPath;
        }

        public IEnumerable<Entities.Template> Build()
        {
            return _templates;
        }

        public TemplateCollectionBuilder AddTemplate(TemplateType templateType, string filename)
        {
            string filePath = System.IO.Path.Combine(_rootTemplatePath, filename);
            return this.AddTemplate(new Entities.Template()
            {
                Content = _fileProvider.ReadAllText(filePath),
                TemplateType = templateType
            });
        }

        public TemplateCollectionBuilder AddTemplate(Entities.Template template)
        {
            _templates.Add(template);
            return this;
        }
    }
}

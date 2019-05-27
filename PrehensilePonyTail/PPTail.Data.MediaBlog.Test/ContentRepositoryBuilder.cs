using Moq;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using PPTail.Common.Builders;

namespace PPTail.Data.MediaBlog.Test
{
    internal class ContentRepositoryBuilder
    {
        readonly IServiceCollection _container;

        ISettings _settingsService = null;
        IFile _fileService = null;
        IDirectory _directoryService = null;

        public ContentRepositoryBuilder()
            :this(new ServiceCollection()) { }

        private ContentRepositoryBuilder(IServiceCollection container)
        {
            _container = container;
        }

        internal IContentRepository Build()
        {
            if (_settingsService != null)
                _container.AddSingleton<ISettings>(c => _settingsService);

            if (_directoryService != null)
                _container.AddSingleton<IDirectory>(c => _directoryService);

            if (_fileService != null)
                _container.AddSingleton<IFile>(c => _fileService);

            return new PPTail.Data.MediaBlog.Repository(_container.BuildServiceProvider());
        }

        internal ContentRepositoryBuilder UseGenericDirectory()
        {
            _directoryService = Mock.Of<IDirectory>();
            return this;
        }

        internal ContentRepositoryBuilder AddDirectoryService(IDirectory directoryService)
        {
            _directoryService = directoryService;
            return this;
        }

        internal ContentRepositoryBuilder AddFileService(IFile fileService)
        {
            _fileService = fileService;
            return this;
        }

        internal ContentRepositoryBuilder AddSettingsService(ISettings settings)
        {
            _settingsService = settings;
            return this;
        }

        internal ContentRepositoryBuilder UseGenericSettings()
        {
            return this.AddSettingsService(new SettingsBuilder().UseGenericValues().Build());
        }

    }
}

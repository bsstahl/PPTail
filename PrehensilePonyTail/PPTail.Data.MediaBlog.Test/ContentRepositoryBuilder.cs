using Moq;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using PPTail.Builders;

namespace PPTail.Data.MediaBlog.Test
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal class ContentRepositoryBuilder
    {
        readonly IServiceCollection _container;

        IFile _fileService = null;
        IDirectory _directoryService = null;

        public ContentRepositoryBuilder()
            :this(new ServiceCollection()) { }

        private ContentRepositoryBuilder(IServiceCollection container)
        {
            _container = container;
        }

        internal IContentRepository Build(string connectionString)
        {
            if (_directoryService != null)
                _container.AddSingleton<IDirectory>(c => _directoryService);

            if (_fileService != null)
                _container.AddSingleton<IFile>(c => _fileService);

            return new Repository(_container.BuildServiceProvider(), connectionString);
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

    }
}

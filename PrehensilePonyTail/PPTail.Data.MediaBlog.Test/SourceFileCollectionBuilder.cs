﻿using PPTail.Builders;
using PPTail.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPTail.Data.MediaBlog.Test
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class SourceFileCollectionBuilder
    {
        readonly List<SourceFile> _sourceFiles = new List<SourceFile>();

        public IEnumerable<SourceFile> Build()
        {
            return _sourceFiles;
        }

        public SourceFileCollectionBuilder AddFile(SourceFile file)
        {
            _sourceFiles.Add(file);
            return this;
        }

        public SourceFileCollectionBuilder AddFiles(IEnumerable<SourceFile> files)
        {
            _sourceFiles.AddRange(files);
            return this;
        }

        public SourceFileCollectionBuilder AddRandomFiles(Int32 count, String relativePath)
        {
            for (Int32 i = 0; i < count; i++)
            {
                this.AddFile(new SourceFileBuilder()
                    .UseRandomValues(relativePath));
            }

            return this;
        }
    }
}

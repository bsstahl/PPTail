using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PPTail.Builders;
using PPTail.Entities;
using TestHelperExtensions;

namespace PPTail.Data.MediaBlog.Test
{
    public static class SourceFileBuilderExtensions
    {
        public static SourceFileBuilder UseRandomFileName(this SourceFileBuilder builder)
        {
            builder.FileName(string.Empty.GetRandom());
            return builder;
        }

        public static SourceFileBuilder UseRandomContents(this SourceFileBuilder builder)
        {
            builder
                .Contents(string.Empty.GetRandom().Select(c => Convert.ToByte(c)).ToArray());
            return builder;
        }

        public static SourceFileBuilder UseRandomValues(this SourceFileBuilder builder, String relativePath)
        {
            builder
                .UseRandomFileName()
                .RelativePath(relativePath)
                .UseRandomContents();

            return builder;
        }
    }
}

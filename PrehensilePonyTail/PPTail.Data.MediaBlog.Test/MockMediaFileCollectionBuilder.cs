using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using TestHelperExtensions;

namespace PPTail.Data.MediaBlog.Test
{
    internal class MockMediaFileCollectionBuilder
    {
        readonly List<(Guid Id, String Contents)> _posts = new List<(Guid Id, String Contents)>();

        internal IEnumerable<MockMediaFile> Build(String rootPath)
        {
            String postPath = System.IO.Path.Combine(rootPath, "posts");
            var mediaFiles = new List<MockMediaFile>();
            foreach (var (id, contents) in _posts)
            {
                mediaFiles.Add(new MockMediaFile()
                {
                    Id = id,
                    Extension = "json",
                    FolderPath = postPath,
                    Contents = contents
                });
            }
            return mediaFiles;
        }

        internal MockMediaFileCollectionBuilder AddRandomPosts(Int32 count)
        {
            for (Int32 i = 0; i < count; i++)
            {
                this.AddRandomPost();
            }
            return this;
        }

        internal MockMediaFileCollectionBuilder AddPost(String json)
        {
            _posts.Add((Guid.NewGuid(), json));
            return this;
        }

        internal MockMediaFileCollectionBuilder AddRandomPost()
        {
            if (2.GetRandom() == 0)
            {
                this.AddRandomFlickrPost();
            }
            else
            {
                this.AddRandomYouTubePost();
            }

            return this;
        }

        public MockMediaFileCollectionBuilder AddRandomFlickrPost()
        {
            return this.AddPost(new MediaPostBuilder()
                .UseRandomFlickrPost()
                .Build());
        }

        public MockMediaFileCollectionBuilder AddRandomYouTubePost()
        {
            return this.AddPost(new MediaPostBuilder()
                .UseRandomYouTubePost()
                .Build());
        }

    }
}

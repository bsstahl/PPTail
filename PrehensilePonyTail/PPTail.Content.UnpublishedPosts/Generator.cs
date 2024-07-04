using System;
using System.Linq;
using PPTail.Builders;
using PPTail.Entities;
using PPTail.Interfaces;

namespace PPTail.Content.UnpublishedPosts;

public class Generator : IContentItemGenerator
{
    private readonly Guid _id = Guid.Parse("0c71a0c9-db98-4f4b-8fa2-5949269b487b");
    private readonly string _title = "Unpublished Posts";
    private readonly string _description = "All posts that are Work-in-Progress";
    private readonly string _slug = "unpublished-posts";

    private readonly IContentRepository _repo;

    public Generator(IContentRepository repo)
    {
        _repo = repo;
    }

    public ContentItem Generate()
    {
        var posts = _repo.GetAllPosts()
            .Where(p => !p.IsPublished && p.BuildIfNotPublished);

        string content = posts.ToMarkdown(_repo.GetSiteSettings().OutputFileExtension);

        return new ContentItemBuilder()
            .Id(_id)
            .BuildIfNotPublished(true)
            .Content(content)
            .Description(_description)
            .IsPublished(true)
            .LastModificationDate(DateTime.UtcNow)
            .PublicationDate(DateTime.UtcNow)
            .ShowInList(false)
            .Slug(_slug)
            .Title(_title)
            .Build();
    }
}

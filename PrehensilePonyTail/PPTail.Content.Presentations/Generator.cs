using PPTail.Entities;
using PPTail.Interfaces;
using PPTail.Extensions;
using System;
using System.Linq;
using PPTail.Builders;
using PPTail.Content.Presentations.Extensions;

namespace PPTail.Content.Presentations;

public class Generator : IContentItemGenerator
{
    private readonly Guid _id = Guid.Parse("d8a2b9c5-a0ea-4cee-b1cc-e10719e10675");
    private readonly string _title = "Local Presentations";
    private readonly string _slug = "presentations";
    private readonly string _description = "Local and/or WIP Presentations";

    private readonly IContentRepository _repo;

    public Generator(IContentRepository repo)
    {
        _repo = repo;
    }

    public ContentItem Generate()
    {
        var presentationPaths = _repo.GetSiteSettings()
            .AdditionalFilePaths
            .Where(p => p.IndexOf("Presentations", StringComparison.OrdinalIgnoreCase) >= 0);

        var additionalFiles = _repo.GetFoldersContents(presentationPaths, true);

        var presentations = additionalFiles
            .Where(p => p.FileName == "index.html");

        string content = presentations.ToMarkdown();

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

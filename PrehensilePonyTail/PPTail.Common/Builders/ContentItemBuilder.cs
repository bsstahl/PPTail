using System;
using System.Collections.Generic;
using PPTail.Entities;

namespace PPTail.Builders;

public class ContentItemBuilder
{
    private readonly ContentItem _item = new();

    public ContentItem Build()
    {
        return _item;
    }

    public ContentItemBuilder Id(Guid value)
    {
        _item.Id = value;
        return this;
    }

    public ContentItemBuilder Author(string value)
    {
        _item.Author = value;
        return this;
    }

    public ContentItemBuilder Title(string value)
    {
        _item.Title = value;
        return this;
    }

    public ContentItemBuilder Description(string value)
    {
        _item.Description = value;
        return this;
    }

    public ContentItemBuilder Teaser(string value)
    {
        _item.Teaser = value;
        return this;
    }

    public ContentItemBuilder Content(string value)
    {
        _item.Content = value;
        return this;
    }

    public ContentItemBuilder IsPublished(bool value)
    {
        _item.IsPublished = value;
        return this;
    }

    public ContentItemBuilder BuildIfNotPublished(bool value)
    {
        _item.BuildIfNotPublished = value;
        return this;
    }

    public ContentItemBuilder ShowInList(bool value)
    {
        _item.ShowInList = value;
        return this;
    }

    public ContentItemBuilder PublicationDate(DateTime value)
    {
        _item.PublicationDate = value;
        return this;
    }

    public ContentItemBuilder LastModificationDate(DateTime value)
    {
        _item.LastModificationDate = value;
        return this;
    }

    public ContentItemBuilder Slug(string value)
    {
        _item.Slug = value;
        return this;
    }

    public ContentItemBuilder Tags(IEnumerable<string> value)
    {
        _item.Tags = value;
        return this;
    }

    public ContentItemBuilder CategoryIds(IEnumerable<Guid> value)
    {
        _item.CategoryIds = value;
        return this;
    }

    public ContentItemBuilder MenuOrder(int value)
    {
        _item.MenuOrder = value;
        return this;
    }

    public ContentItemBuilder ByLine(string value)
    {
        _item.ByLine = value;
        return this;
    }
}
